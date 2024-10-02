using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Model.Highschool;
using Domain.Model.Response;
using Domain.Model.Wallet;

namespace Infrastructure.Persistence.Service
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WalletService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Wallet> GetWalletByIdAsync(Guid Id)
        {
            var wallet = await _unitOfWork.WalletRepository.GetByIdGuidAsync(Id);
            return _mapper.Map<Wallet>(wallet);
        }

        public async Task<ResponseModel> UpdateUsingGoldWalletAsync(WalletPutModel putModel)
        {
            var wallet = _mapper.Map<Wallet>(putModel);
            var result = await _unitOfWork. WalletRepository.UpdateAsync(wallet);
            _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Wallet Updated Successfully",
                IsSuccess = true,
                Data = wallet,
            };
        }
    }
}
