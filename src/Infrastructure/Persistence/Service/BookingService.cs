using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Booking;
using Domain.Model.Consultant;
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
                        Message = "The consultation time does not exist or is already booked"
                    };
                }

                var consultantWallet = consultationTime.Day.Consultant.Account.Wallet;

                if (consultantWallet == null)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Consultant wallet not found"
                    };
                }

                var priceOnSlot = consultationTime.Day.Consultant.ConsultantLevel.PriceOnSlot;

                var student = await _unitOfWork.StudentRepository
                    .SingleOrDefaultAsync(
                        predicate: s => s.Id.Equals(studentId), 
                        include: s => s.Include(st => st.Account.Wallet)) 
                    ?? throw new Exception("Student does not exist");

                if (student.Account.Wallet == null)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Student wallet not found"
                    };
                }

                var studentWallet = student.Account.Wallet;

                if (studentWallet.GoldBalance < priceOnSlot)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Insufficient balance in the wallet to book the consultation time"
                    };
                }

                Expression<Func<Booking, bool>> exsitingTimeSlotFilter = x =>
                    x.StudentId.Equals(studentId) &&
                    x.ConsultationTime.TimeSlotId.Equals(consultationTime.TimeSlotId) &&
                    x.ConsultationTime.ConsultationDayId.Equals(consultationTime.ConsultationDayId);
                var existingBookingWithSameTimeSlot = await _unitOfWork.BookingRepository
                    .SingleOrDefaultAsync(
                        predicate: exsitingTimeSlotFilter
                    );

                if (existingBookingWithSameTimeSlot != null)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "You have already booked another consultation time with the same time slot on this day"
                    };
                }

                var booking = new Booking
                {
                    Id = Guid.NewGuid(),
                    ConsultationTimeId = consultationTimeId,
                    StudentId = studentId,
                    Status = true
                };

                consultationTime.Status = (int)ConsultationTimeStatusEnum.Booked;

                consultationTime.Bookings.Add(booking);

                studentWallet.GoldBalance -= (int)priceOnSlot;

                consultantWallet.GoldBalance += (int)priceOnSlot;

                await _unitOfWork.ConsultationTimeRepository.UpdateAsync(consultationTime);
                await _unitOfWork.WalletRepository.UpdateAsync(studentWallet);
                await _unitOfWork.WalletRepository.UpdateAsync(consultantWallet);
                await _unitOfWork.SaveChangesAsync();

                
                var studentTransactionPostModel = new TransactionPostModel(studentWallet.Id, (int)priceOnSlot);
                await _unitOfWork.TransactionRepository
                    .CreateTransactionWhenUsingGold(TransactionType.Using, studentTransactionPostModel);

                var consultantTransactionPostModel = new TransactionPostModel(consultantWallet.Id, (int)priceOnSlot);
                await _unitOfWork.TransactionRepository
                    .CreateTransactionWhenUsingGold(TransactionType.Receiving, consultantTransactionPostModel);

                var result = _mapper.Map<BookingViewModel>(booking);
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Consultation time booked successfully.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while book consultation time: {ex.Message}"
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
                ) ?? throw new Exception($"Booking not found with id '{bookingId}'");

                var result = _mapper.Map<BookingViewModel>(booking);
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Booking retrieved successfully.",
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
