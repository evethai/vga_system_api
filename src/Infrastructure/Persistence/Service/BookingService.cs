using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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
                    x.ConsultationTime.Day.Day.Equals(consultationTime.Day.Day);

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
                    Status = true
                };

                consultationTime.Status = (int)ConsultationTimeStatusEnum.Booked;
                studentWallet.GoldBalance -= (int)priceOnSlot;
                consultantWallet.GoldBalance += (int)priceOnSlot;

                // Create transactions
                var studentTransaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    WalletId = studentWallet.Id,
                    TransactionType = TransactionType.Using,
                    Description = $"Bạn đã sử dụng {priceOnSlot} Gold để đặt tư vấn",
                    GoldAmount = (int)priceOnSlot,
                    TransactionDateTime = DateTime.UtcNow
                };

                var consultantTransaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    WalletId = consultantWallet.Id,
                    TransactionType = TransactionType.Receiving,
                    Description = $"Bạn đã nhận {priceOnSlot} Gold từ buổi tư vấn",
                    GoldAmount = (int)priceOnSlot,
                    TransactionDateTime = DateTime.UtcNow
                };

                // Call consolidated method in the repository
                await _unitOfWork.BookingRepository.SaveBookingDataAsync(
                    consultationTime,
                    studentWallet,
                    consultantWallet,
                    booking,
                    studentTransaction,
                    consultantTransaction
                );

                NotificationPostModel notiPostModel = new NotificationPostModel();
                notiPostModel.AccountId = consultationTime.Day.ConsultantId;
                notiPostModel.Title = "Lịch tư vấn đã được đặt";
                notiPostModel.Message = $"Lịch tư vấn của bạn vào slot từ {consultationTime.SlotTime.StartTime} đến {consultationTime.SlotTime.EndTime} vào ngày {consultationTime.Day.Day} đã được đặt thành công bởi {student.Account.Name}.";

                await _unitOfWork.NotificationRepository.CreateNotification(notiPostModel);

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
                    Message = $"An error occurred while booking consultation time: {ex.Message}"
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
                    Message = $"An error occurred while get booking by id: {ex.Message}"
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
                        Message = "No bookings found."
                    };
                }

                var result = _mapper.Map<List<BookingViewModel>>(bookings);

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Bookings retrieved successfully.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while get all bookings: {ex.Message}"
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
    }
}
