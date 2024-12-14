using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Booking;
using Domain.Model.Response;
using Domain.Model.Transaction;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly VgaDbContext _context;
        public BookingRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public (Expression<Func<Booking, bool>> filter, Func<IQueryable<Booking>, IOrderedQueryable<Booking>> orderBy)
            BuildFilterAndOrderBy(BookingSearchModel searchModel)
        {
            Expression<Func<Booking, bool>> filter = p => true;
            Func<IQueryable<Booking>, IOrderedQueryable<Booking>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.consultantName))
            {
                filter = filter.And(p => p.ConsultationTime.Day.Consultant.Account.Name.Contains(searchModel.consultantName));
            }
            if (!string.IsNullOrEmpty(searchModel.studentName))
            {
                filter = filter.And(p => p.Student.Account.Name.Contains(searchModel.studentName));
            }
            if (searchModel.studentId.HasValue)
            {
                filter = filter.And(p => p.StudentId.Equals(searchModel.studentId.Value));
            }
            if (searchModel.consultantId.HasValue)
            {
                filter = filter.And(p => p.ConsultationTime.Day.ConsultantId.Equals(searchModel.consultantId.Value));
            }
            if (searchModel.universityId.HasValue)
            {
                filter = filter.And(p => p.ConsultationTime.Day.Consultant
                .ConsultantRelations.Any(cr => cr.UniversityId.Equals(searchModel.universityId.Value)));
            }
            if (searchModel.status.HasValue)
            {
                filter = filter.And(p => p.Status.Equals(searchModel.status.Value));
            }
            if (searchModel.Day.HasValue)
            {
                filter = filter.And(p => p.ConsultationTime.Day.Day.Equals(searchModel.Day.Value));
            }
            if (searchModel.dayInWeek.HasValue)
            {
                DateOnly inputDate = searchModel.dayInWeek.Value;
                DateOnly startOfWeek = inputDate.AddDays(-(int)inputDate.DayOfWeek + (int)DayOfWeek.Monday);
                DateOnly endOfWeek = startOfWeek.AddDays(6);

                filter = filter.And(cd => cd.ConsultationTime.Day.Day >= startOfWeek && cd.ConsultationTime.Day.Day <= endOfWeek);
            }
            if (searchModel.sortByDay.HasValue && searchModel.sortByDay.Value)
            {
                orderBy = query => searchModel.descending.HasValue && searchModel.descending.Value
                    ? query.OrderByDescending(b => b.ConsultationTime.Day.Day)
                    : query.OrderBy(b => b.ConsultationTime.Day.Day);
            }
            return (filter, orderBy);
        }

        public async Task<List<Booking>> GetAllBookingsWithDetailsAsync()
        {
            return await _context.Booking
                .Include(b => b.ConsultationTime)
                    .ThenInclude(ct => ct.SlotTime)
                .Include(b => b.ConsultationTime.Day)
                    .ThenInclude(cd => cd.Consultant)
                        .ThenInclude(e => e.Account)
                .Include(b => b.Student)
                    .ThenInclude(s => s.Account)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task SaveBookingDataAsync(
            ConsultationTime consultationTime,
            Wallet studentWallet,
            Wallet consultantWallet,
            Booking booking,
            Transaction studentTransaction)
        {
            // Update consultation time
            _context.ConsultationTime.Update(consultationTime);

            // Update student and consultant wallets
            _context.Wallet.Update(studentWallet);
            _context.Wallet.Update(consultantWallet);

            // Add booking and transactions
            await _context.Booking.AddAsync(booking);
            await _context.Transaction.AddAsync(studentTransaction);

            // Save all changes
            await _context.SaveChangesAsync();
        }

        public async Task<ResponseModel> ProcessBooking(Guid bookingId, BookingConsultantUpdateModel model)
        {
            var existedBooking = _context.Booking
                .Where(t => t.Id.Equals(bookingId))
                .Include(b => b.ConsultationTime.Day.Consultant.Account.Wallet)
                .Include(b => b.Student.Account.Wallet)
                .FirstOrDefault() ?? throw new NotExistsException();

            Wallet wallet = null;

            Notification notiPostModel = new Notification
            {
                CreatedAt = DateTime.UtcNow.AddHours(7),
                Status = Domain.Enum.NotiStatus.Unread
            };

            Transaction transaction = null;

            switch (model.Type)
            {
                case BookingStatus.Consulted:
                    wallet = _context.Wallet
                        .Where(w => w.Id.Equals(existedBooking.ConsultationTime.Day.Consultant.Account.Wallet.Id))
                        .FirstOrDefault() ?? throw new NotExistsException();

                    //update booking type 
                    existedBooking.Status = model.Type;
                    if (model.Comment != null || model.Comment != "")
                        existedBooking.Comment = model.Comment;
                    //update wallet
                    wallet.GoldBalance += (int)existedBooking.Price;

                    //create transaction
                    transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = wallet.Id,
                        TransactionType = TransactionType.Receiving,
                        Description = $"Bạn đã nhận {existedBooking.Price} điểm từ {existedBooking.Student.Account.Name}",
                        GoldAmount = (int)existedBooking.Price,
                        TransactionDateTime = DateTime.UtcNow.AddHours(7),
                    };

                    //create notification 
                    notiPostModel.AccountId = wallet.AccountId;
                    notiPostModel.Title = NotificationConstant.Title.BookingConsulted;
                    notiPostModel.Message = $"Đã hoàn thành buổi tư vấn với {existedBooking.Student.Account.Name}. " +
                        $"Bạn đã nhận được {existedBooking.Price} điểm từ buổi tư vấn.";

                    break;
                case BookingStatus.Canceled:
                    wallet = _context.Wallet
                        .Where(w => w.Id.Equals(existedBooking.Student.Account.Wallet.Id))
                        .FirstOrDefault() ?? throw new NotExistsException();

                    //update booking
                    existedBooking.Status = model.Type;
                    if (model.Comment != null || model.Comment != "")
                       throw new Exception("Comment is not null or empty");
                    existedBooking.Comment = model.Comment;
                    //update wallet
                    wallet.GoldBalance += (int)existedBooking.Price;

                    //create transaction
                    transaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = wallet.Id,
                        TransactionType = TransactionType.Receiving,
                        Description = $"Bạn đã nhận {existedBooking.Price} điểm từ {existedBooking.ConsultationTime.Day.Consultant.Account.Name}",
                        GoldAmount = (int)existedBooking.Price,
                        TransactionDateTime = DateTime.UtcNow.AddHours(7),
                    };
                    //create notification 
                    notiPostModel.AccountId = wallet.AccountId;
                    notiPostModel.Title = NotificationConstant.Title.BookingCanceled;
                    notiPostModel.Message = $"Buổi tư vấn đã bị hủy với lý do: {model.Comment}" +
                        $".Bạn sẽ nhận lại {existedBooking.Price} điểm đã sử dụng cho buổi tư vấn này.";
                    break;
                default:
                    throw new Exception("Appcepted type is Consulted(2) or Canceled(3) only");
            }

            _context.Wallet.Update(wallet);
            _context.Booking.Update(existedBooking);
            await _context.Transaction.AddAsync(transaction);
            await _context.Notification.AddAsync(notiPostModel);
            await _context.SaveChangesAsync();

            return new ResponseModel
            {
                Message = "Process booking is successfully",
                IsSuccess = true
            };
        }

        public async Task<ResponseModel> ProcessReport(Guid bookingId, BookingProcessReportModel model)
        {
            var existedBooking = _context.Booking
                .Where(t => t.Id.Equals(bookingId))
                .Include(b => b.ConsultationTime.Day.Consultant.Account.Wallet)
                .Include(b => b.Student.Account.Wallet)
                .FirstOrDefault() ?? throw new NotExistsException();

            var consultantWallet = _context.Wallet
                .Where(w => w.Id.Equals(existedBooking.ConsultationTime.Day.Consultant.Account.Wallet.Id))
                .FirstOrDefault() ?? throw new NotExistsException();
            var studentWallet = _context.Wallet
                .Where(w => w.Id.Equals(existedBooking.Student.Account.Wallet.Id))
                .FirstOrDefault() ?? throw new NotExistsException();

            Notification notiStudentPostModel = new Notification
            {
                CreatedAt = DateTime.UtcNow.AddHours(7),
                Status = Domain.Enum.NotiStatus.Unread
            };

            Notification notiConsultantPostModel = new Notification
            {
                CreatedAt = DateTime.UtcNow.AddHours(7),
                Status = Domain.Enum.NotiStatus.Unread
            };

            switch (model.Type)
            {
                case BookingStatus.Consulted:
                    //update booking type 
                    existedBooking.Status = model.Type;
                    existedBooking.Comment = model.Comment;
                    if (model.Image != null || model.Image != "")
                        existedBooking.Image = model.Image;

                    //create notification 
                    notiStudentPostModel.AccountId = studentWallet.AccountId;
                    notiStudentPostModel.Title = NotificationConstant.Title.BookingProcessConsult;
                    notiStudentPostModel.Message = $"Báo cáo buổi tư vấn đã bị từ chối với lý do: {model.Comment}.";

                    notiConsultantPostModel.AccountId = consultantWallet.AccountId;
                    notiConsultantPostModel.Title = NotificationConstant.Title.BookingProcessConsult;
                    notiConsultantPostModel.Message = $"Báo cáo buổi tư vấn đã bị từ chối với lý do: {model.Comment}.";

                    break;
                case BookingStatus.Canceled:
                    //update booking type
                    existedBooking.Status = model.Type;
                    existedBooking.Comment = model.Comment;
                    if (model.Image != null || model.Image != "")
                        existedBooking.Image = model.Image;

                    //update wallet
                    studentWallet.GoldBalance += (int)existedBooking.Price;
                    consultantWallet.GoldBalance -= (int)existedBooking.Price;

                    //create transaction
                    Transaction studentTransaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = studentWallet.Id,
                        TransactionType = TransactionType.Receiving,
                        Description = $"Bạn đã nhận {existedBooking.Price} điểm từ {consultantWallet.Account.Name}",
                        GoldAmount = (int)existedBooking.Price,
                        TransactionDateTime = DateTime.UtcNow.AddHours(7),
                    };

                    Transaction consultantTransaction = new Transaction
                    {
                        Id = Guid.NewGuid(),
                        WalletId = consultantWallet.Id,
                        TransactionType = TransactionType.Transferring,
                        Description = $"Bạn đã chuyển {existedBooking.Price} điểm đến {studentWallet.Account.Name}",
                        GoldAmount = (int)existedBooking.Price,
                        TransactionDateTime = DateTime.UtcNow.AddHours(7),
                    };

                    //create notification 
                    notiStudentPostModel.AccountId = studentWallet.AccountId;
                    notiStudentPostModel.Title = NotificationConstant.Title.BookingProcessCancel;
                    notiStudentPostModel.Message = $"Báo cáo buổi tư vấn đã xử lý thành công với lý do: {model.Comment}." +
                        $"Bạn sẽ nhận lại {existedBooking.Price} điểm sử dụng cho buổi tư vấn.";

                    notiConsultantPostModel.AccountId = consultantWallet.AccountId;
                    notiConsultantPostModel.Title = NotificationConstant.Title.BookingProcessCancel;
                    notiConsultantPostModel.Message = $"Báo cáo buổi tư vấn đã xử lý thành công với lý do: {model.Comment}." +
                        $"Bạn sẽ bị trừ {existedBooking.Price} điểm nhận được từ buổi tư vấn.";

                    _context.Wallet.Update(studentWallet);
                    _context.Wallet.Update(consultantWallet);

                    await _context.Transaction.AddAsync(studentTransaction);
                    await _context.Transaction.AddAsync(consultantTransaction);
                    break;
                default:
                    throw new Exception("Appcepted type is Consulted(2) or Canceled(3) only");
            }

            _context.Booking.Update(existedBooking);
            await _context.Notification.AddAsync(notiStudentPostModel);
            await _context.Notification.AddAsync(notiConsultantPostModel);
            await _context.SaveChangesAsync();

            return new ResponseModel
            {
                Message = "Process report is successfully",
                IsSuccess = true
            };
        }

    }
}
