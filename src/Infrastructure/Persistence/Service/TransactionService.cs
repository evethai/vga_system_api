using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Constants;
using Application.Common.Exceptions;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Highschool;
using Domain.Model.Notification;
using Domain.Model.Response;
using Domain.Model.Transaction;
using Microsoft.EntityFrameworkCore;

namespace Application.Interface.Service
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseTransactionModel> GetListTransactionAsync(TransactionSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.TransactionRepository.BuildFilterAndOrderBy(searchModel);
            var transaction = await _unitOfWork.TransactionRepository.GetBySearchAsync(filter, orderBy,
                q => q.Include(s => s.Wallet)
                       .ThenInclude(a => a.Account)
                        .ThenInclude(a => a.University),
                pageIndex: searchModel.currentPage,
                pageSize: searchModel.pageSize);
            var total = await _unitOfWork.TransactionRepository.CountAsync(filter);
            var listTransaction = _mapper.Map<List<TransactionModel>>(transaction);
            return new ResponseTransactionModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                transactions = listTransaction,
            };
        }

        #region Create withdraw
        public async Task<ResponseModel> CreateWithdrawAsync(Guid consultantId, int goldAmount)
        {
            try
            {
                var consultant = await _unitOfWork.ConsultantRepository.SingleOrDefaultAsync(
                        predicate: o => o.Id.Equals(consultantId),
                        include: q => q.Include(c => c.Account)
                                            .ThenInclude(a => a.Wallet)
                    ) ?? throw new NotExistsException();

                var admin = await _unitOfWork.AccountRepository.SingleOrDefaultAsync(
                    predicate: o => o.Role.Equals(RoleEnum.Admin)) ?? throw new NotExistsException();

                if (goldAmount > consultant.Account.Wallet.GoldBalance || goldAmount <= 0)
                    return new ResponseModel
                    {
                        Message = "Không đủ số tiền để rút",
                        IsSuccess = false,
                    };

                var responseModel = await _unitOfWork.TransactionRepository.CreateTransactionRequest(consultant.Account.Wallet.Id, goldAmount);

                NotificationPostModel notiPostModel = new NotificationPostModel();
                notiPostModel.AccountId = admin.Id;
                notiPostModel.Title = NotificationConstant.Title.Request;
                notiPostModel.Message = $"Tư vấn viên {consultant.Account.Name} đã yêu cầu rút {goldAmount} điểm vào ngày {DateTime.UtcNow}";
                await _unitOfWork.NotificationRepository.CreateNotification(notiPostModel);
                await _unitOfWork.SaveChangesAsync();

                return responseModel;
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while create withdraw: {ex.Message}"
                };
            }
        }
        #endregion

        #region Process withdraw request
        public async Task<ResponseModel> ProcessWithdrawRequestAsync(Guid transactionId, TransactionProcessRequestModel model)
        {
            try
            {
                var transaction = await _unitOfWork.TransactionRepository.SingleOrDefaultAsync(
                        predicate: o => o.Id.Equals(transactionId),
                        include: q => q.Include(c => c.Wallet).ThenInclude(a => a.Account)
                    ) ?? throw new NotExistsException();

                if (transaction.TransactionType != TransactionType.Request)
                    throw new Exception("Transaction is not Request");

                var responseModel = await _unitOfWork.TransactionRepository.ProcessWithdrawRequest(transactionId, model);

                return responseModel;
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while process withdraw: {ex.Message}"
                };
            }
        }
        #endregion
    }
}
