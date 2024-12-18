﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Booking;
using Domain.Model.Consultant;
using Domain.Model.Notification;
using Domain.Model.Response;
using Domain.Model.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Service
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Book consultation time
        public async Task<ResponseModel> BookConsultationTimeAsync(Guid consultationTimeId, Guid studentId)
        {
            try
            {
                // Get consultation time and include necessary relationships
                Expression<Func<ConsultationTime, bool>> exsitingConsultationTimeFilter = x =>
                    x.Id.Equals(consultationTimeId) &&
                    x.Status.Equals((int)ConsultationTimeStatusEnum.Available);

                var consultationTime = await _unitOfWork.ConsultationTimeRepository
                    .SingleOrDefaultAsync(
                        predicate: exsitingConsultationTimeFilter,
                        include: q => q
                            .Include(ct => ct.Day.Consultant)
                            .Include(ct => ct.Day.Consultant.ConsultantLevel)
                            .Include(ct => ct.Day.Consultant.Account.Wallet)
                            .Include(ct => ct.SlotTime)
                    );

                if (consultationTime == null)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Khoảng thời gian tư vấn đã được đặt hoặc không tồn tại"
                    };
                }

                var consultantWallet = consultationTime.Day.Consultant.Account.Wallet;
                var priceOnSlot = consultationTime.Day.Consultant.ConsultantLevel.PriceOnSlot;

                var student = await _unitOfWork.StudentRepository
                    .SingleOrDefaultAsync(
                        predicate: s => s.Id.Equals(studentId),
                        include: s => s.Include(st => st.Account.Wallet))
                    ?? throw new NotExistsException();

                var studentWallet = student.Account.Wallet;

                if (studentWallet.GoldBalance < priceOnSlot)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Số dư trong ví của học sinh không đủ để thực hiện đặt lịch"
                    };
                }

                // Check for existing bookings
                Expression<Func<Booking, bool>> exsitingTimeSlotFilter = x =>
                    x.StudentId.Equals(studentId) &&
                    x.ConsultationTime.TimeSlotId.Equals(consultationTime.TimeSlotId) &&
                    x.ConsultationTime.Day.Day.Equals(consultationTime.Day.Day) &&
                    x.Status.Equals(true);

                var existingBookingWithSameTimeSlot = await _unitOfWork.BookingRepository
                    .SingleOrDefaultAsync(predicate: exsitingTimeSlotFilter);

                if (existingBookingWithSameTimeSlot != null)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Bạn đã đặt một khoảng thời gian tư vấn tương tự trong ngày này"
                    };
                }

                // Create booking and update statuses
                var booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    ConsultationTimeId = consultationTimeId,
                    StudentId = studentId,
                    Status = BookingStatus.NotYet_Consulted,
                    Price = priceOnSlot
                };

                consultationTime.Status = (int)ConsultationTimeStatusEnum.Booked;
                studentWallet.GoldBalance -= (int)priceOnSlot;
                //consultantWallet.GoldBalance += (int)priceOnSlot;

                // Create transactions
                var studentTransaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    WalletId = studentWallet.Id,
                    TransactionType = TransactionType.Using,
                    Description = $"Bạn đã sử dụng {priceOnSlot} điểm để đặt tư vấn",
                    GoldAmount = (int)priceOnSlot,
                    TransactionDateTime = DateTime.UtcNow.AddHours(7)
                };

                //var consultantTransaction = new Transaction
                //{
                //    Id = Guid.NewGuid(),
                //    WalletId = consultantWallet.Id,
                //    TransactionType = TransactionType.Receiving,
                //    Description = $"Bạn đã nhận {priceOnSlot} điểm từ buổi tư vấn",
                //    GoldAmount = (int)priceOnSlot,
                //    TransactionDateTime = DateTime.UtcNow.AddHours(7)
                //};

                // Call consolidated method in the repository
                await _unitOfWork.BookingRepository.SaveBookingDataAsync(
                    consultationTime,
                    studentWallet,
                    consultantWallet,
                    booking,
                    studentTransaction
                );

                NotificationPostModel notiPostModel = new NotificationPostModel();
                notiPostModel.AccountId = consultationTime.Day.Consultant.AccountId;
                notiPostModel.Title = NotificationConstant.Title.NewBooking;
                notiPostModel.Message = $"Lịch tư vấn của bạn vào slot từ {consultationTime.SlotTime.StartTime} đến {consultationTime.SlotTime.EndTime} vào ngày {consultationTime.Day.Day} đã được đặt thành công bởi {student.Account.Name}.";

                NotificationPostModel notiStudentPostModel = new NotificationPostModel();
                notiStudentPostModel.AccountId = student.AccountId;
                notiStudentPostModel.Title = NotificationConstant.Title.BookingConsultant;
                notiStudentPostModel.Message = $"Bạn đã đặt tư vấn vào slot từ {consultationTime.SlotTime.StartTime} đến {consultationTime.SlotTime.EndTime} vào ngày {consultationTime.Day.Day} với tư vấn viên {consultationTime.Day.Consultant.Account.Name}.";

                await _unitOfWork.NotificationRepository.CreateNotification(notiPostModel);
                await _unitOfWork.NotificationRepository.CreateNotification(notiStudentPostModel);
                var result = _mapper.Map<BookingViewModel>(booking);
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Đặt khoảng thời gian tư vấn thành công",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        #endregion


        #region Get booking by id
        public async Task<ResponseModel> GetBookingByIdAsync(Guid bookingId)
        {
            try
            {
                var booking = await _unitOfWork.BookingRepository.SingleOrDefaultAsync(
                    predicate: b => b.Id == bookingId,
                    include: q => q.Include(b => b.ConsultationTime)
                                   .ThenInclude(ct => ct.SlotTime)
                                   .Include(b => b.ConsultationTime.Day.Consultant)
                                   .ThenInclude(e => e.Account)
                                   .Include(b => b.Student)
                                   .ThenInclude(s => s.Account)
                ) ?? throw new NotExistsException();

                var result = _mapper.Map<BookingViewModel>(booking);
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Lấy lịch đã đặt thành công",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        #endregion

        #region Get all bookings
        public async Task<ResponseModel> GetAllBookingsAsync()
        {
            try
            {
                var bookings = await _unitOfWork.BookingRepository.GetAllBookingsWithDetailsAsync();

                if (bookings == null || !bookings.Any())
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Không tìm thấy đặt chỗ nào."
                    };
                }

                var result = _mapper.Map<List<BookingViewModel>>(bookings);

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Đã lấy danh sách đặt chỗ thành công.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        #endregion

        #region Get bookings with paginate
        public async Task<ResponseBookingModel> GetListBookingsWithPaginateAsync(BookingSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.BookingRepository.BuildFilterAndOrderBy(searchModel);
            var booking = await _unitOfWork.BookingRepository
                .GetBySearchAsync(
                    filter,
                    orderBy,
                    include: q => q
                        .Include(b => b.ConsultationTime)
                            .ThenInclude(ct => ct.SlotTime)
                        .Include(b => b.ConsultationTime.Day)
                            .ThenInclude(cd => cd.Consultant)
                                .ThenInclude(e => e.Account)
                        .Include(b => b.Student)
                            .ThenInclude(s => s.Account),
                    pageIndex: searchModel.currentPage,
                    pageSize: searchModel.pageSize
                );

            var total = await _unitOfWork.BookingRepository.CountAsync(filter);
            var listBookings = _mapper.Map<List<BookingViewModel>>(booking);
            return new ResponseBookingModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                bookings = listBookings,
            };
        }
        #endregion

        #region Process booking status
        public async Task<ResponseModel> ProcessBookingAsync(Guid bookingId, BookingConsultantUpdateModel model)
        {
            try
            {
                var booking = await _unitOfWork.BookingRepository.GetByIdGuidAsync(bookingId)
                    ?? throw new NotExistsException();

                if (booking.Status != BookingStatus.NotYet_Consulted)
                    throw new Exception("Đặt chỗ không có trạng thái 'Chưa tham vấn'");

                var responseModel = await _unitOfWork.BookingRepository.ProcessBooking(bookingId, model);

                return responseModel;
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        #endregion

        #region Report booking
        public async Task<ResponseModel> ReportBookingAsync(Guid bookingId, BookingStudentReportModel model)
        {
            try
            {
                var booking = await _unitOfWork.BookingRepository.SingleOrDefaultAsync(
                    predicate: b => b.Id == bookingId,
                    include: q => q.Include(b => b.ConsultationTime.Day.Consultant.Account)
                                    .Include(b =>b.Student.Account)
                                    .Include(b => b.ConsultationTime.SlotTime)
                    ) ?? throw new NotExistsException();

                var studentName= booking.Student.Account.Name;
                var consultantName = booking.ConsultationTime.Day.Consultant.Account.Name;

                if (booking.Status != BookingStatus.Consulted && booking.Status != BookingStatus.NotYet_Consulted)
                    throw new Exception("Đặt chỗ không có trạng thái 'Đã tham vấn' hoặc 'Chưa tham vấn'");

                var dateTimeStart = new DateTime(
                        booking.ConsultationTime.Day.Day.Year,
                        booking.ConsultationTime.Day.Day.Month,
                        booking.ConsultationTime.Day.Day.Day,
                        booking.ConsultationTime.SlotTime.StartTime.Hour,
                        booking.ConsultationTime.SlotTime.StartTime.Minute,
                        booking.ConsultationTime.SlotTime.StartTime.Second,
                        DateTimeKind.Utc
                    );

                var dateTimeNow = DateTime.UtcNow.AddHours(7);
                if (dateTimeStart > dateTimeNow)
                    throw new Exception("Bạn chỉ có thể báo cáo sau giờ bắt đầu tư vấn.");

                var admin = await _unitOfWork.AccountRepository.SingleOrDefaultAsync(
                    predicate: o => o.Role.Equals(RoleEnum.Admin)) ?? throw new NotExistsException();

                booking.Status = BookingStatus.Reported;
                booking.Comment = model.Comment;
                booking.Image= model.Image;

                NotificationPostModel notiPostModel = new NotificationPostModel();
                notiPostModel.AccountId = admin.Id;
                notiPostModel.Title = NotificationConstant.Title.BookingReported;
                notiPostModel.Message = $"Học sinh {studentName} đã báo cáo buổi tư vấn với tư vấn viên {consultantName}";

                NotificationPostModel notiConsultantPostModel = new NotificationPostModel();
                notiConsultantPostModel.AccountId = booking.ConsultationTime.Day.Consultant.Account.Id;
                notiConsultantPostModel.Title = NotificationConstant.Title.BookingReported;
                notiConsultantPostModel.Message = $"Học sinh {studentName} đã báo cáo buổi tư vấn với bạn";


                await _unitOfWork.BookingRepository.UpdateAsync(booking);
                await _unitOfWork.NotificationRepository.CreateNotification(notiPostModel);
                await _unitOfWork.NotificationRepository.CreateNotification(notiConsultantPostModel);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<BookingViewModel>(booking);
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Báo cáo lịch thành công",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        #endregion

        #region Process report booking
        public async Task<ResponseModel> ProcessReportBookingAsync(Guid bookingId, BookingProcessReportModel model)
        {
            try
            {
                var booking = await _unitOfWork.BookingRepository.SingleOrDefaultAsync(
                    predicate: b => b.Id == bookingId,
                    include: q => q.Include(b => b.ConsultationTime.Day).Include(b => b.ConsultationTime.SlotTime)
                    )?? throw new NotExistsException();

                if (booking.Status != BookingStatus.Reported)
                    throw new Exception("Đặt phòng không có trạng thái 'Đã báo cáo'");

                var responseModel = await _unitOfWork.BookingRepository.ProcessReport(bookingId, model);

                return responseModel;
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        #endregion
    }
}
