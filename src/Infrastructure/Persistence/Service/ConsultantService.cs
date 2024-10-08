using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Utils;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Consultant;
using Domain.Model.Response;
using Domain.Model.Student;

namespace Infrastructure.Persistence.Service
{
    public class ConsultantService : IConsultantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ConsultantService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get consultant by id
        public async Task<ResponseModel> GetConsultantByIdAsync(Guid consultantId)
        {
            try
            {
                var consultant = await _unitOfWork.ConsultantRepository.GetByIdGuidAsync(consultantId)
                    ?? throw new Exception($"Consultant not found by id: {consultantId}");
                var result = _mapper.Map<ConsultantViewModel>(consultant);
                return new ResponseModel
                {
                    Message = $"Get consultant by id '{consultantId}' successfull",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while get consultant by id: {ex.Message}"
                };
            }
        }
        #endregion

        #region Create consultant
        public async Task<ResponseModel> CreateConsultantAsync(ConsultantPostModel postModel)
        {
            try
            {
                var roleId = await _unitOfWork.RoleRepository
                    .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.Name.Equals(RoleEnum.Expert.ToString()));

                var consultantLevel = await _unitOfWork.ConsultantLevelRepository.GetByIdAsync(postModel.ConsultantLevelId) ??
                    throw new Exception("Consultant level not exist");

                var consultant = _mapper.Map<Consultant>(postModel);
                consultant.Id = Guid.NewGuid();
                consultant.Account = new Account
                {
                    Id = Guid.NewGuid(),
                    Email = postModel.Email,
                    Phone = postModel.Phone,
                    Password = PasswordUtil.HashPassword(postModel.Phone),
                    RoleId = roleId,
                    Status = AccountStatus.Active,
                    CreateAt = DateTime.UtcNow
                };
                consultant.Account.Wallet = new Wallet
                {
                    Id = Guid.NewGuid(),
                    GoldBalance = 0,
                    AccountId = consultant.Account.Id,
                };

                consultant.Status = true;

                await _unitOfWork.ConsultantRepository.AddAsync(consultant);
                await _unitOfWork.SaveChangesAsync();
                var result = _mapper.Map<ConsultantViewModel>(consultant);
                return new ResponseModel
                {
                    Message = $"Create consultant successfull",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while create consultant: {ex.Message}"
                };
            }
        }
        #endregion

        #region Update consultant
        public async Task<ResponseModel> UpdateConsultantAsync(Guid consultantId, ConsultantPutModel putModel)
        {
            try
            {
                var consultant = await _unitOfWork.ConsultantRepository.GetByIdGuidAsync(consultantId)
                    ?? throw new Exception($"Consultant not found by id: {consultantId}");
                _mapper.Map(putModel, consultant);
                await _unitOfWork.ConsultantRepository.UpdateAsync(consultant);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<ConsultantViewModel>(consultant);
                return new ResponseModel
                {
                    Message = $"Consultant with id '{consultantId}' was updated successfully",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while update consultant: {ex.Message}"
                };
            }
        }
        #endregion

        #region Delete consultant
        public async Task<ResponseModel> DeleteConsultantAsync(Guid consultantId)
        {
            try
            {
                var consultant = await _unitOfWork.ConsultantRepository.GetByIdGuidAsync(consultantId)
                ?? throw new Exception($"Consultant not found by id: {consultantId}");

                consultant.Status = false;
                await _unitOfWork.ConsultantRepository.UpdateAsync(consultant);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<ConsultantViewModel>(consultant);
                return new ResponseModel
                {
                    Message = $"Consultant with id '{consultantId}' was deleted successfully",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while delete consultant: {ex.Message}"
                };
            }
        }
        #endregion

        #region Get consultants with paginate
        public async Task<ResponseConsultantModel> GetListConsultantsWithPaginateAsync(ConsultantSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.ConsultantRepository.BuildFilterAndOrderBy(searchModel);
            var consultant = await _unitOfWork.ConsultantRepository
                .GetByConditionAsync(filter, orderBy, pageIndex: searchModel.currentPage, pageSize: searchModel.pageSize);
            var total = await _unitOfWork.ConsultantRepository.CountAsync(filter);
            var listConsultants = _mapper.Map<List<ConsultantViewModel>>(consultant);
            return new ResponseConsultantModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                consultants = listConsultants,
            };
        }
        #endregion
    }
}
