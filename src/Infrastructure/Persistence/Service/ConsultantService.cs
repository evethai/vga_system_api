using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Utils;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Account;
using Domain.Model.Consultant;
using Domain.Model.Response;
using Microsoft.EntityFrameworkCore;

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
                var consultant = await _unitOfWork.ConsultantRepository
                    .SingleOrDefaultAsync(
                    predicate: o => o.Id.Equals(consultantId),
                    include: q => q.Include(c => c.Account).ThenInclude(a => a.Wallet)
                                    .Include(c => c.University)
                                    .Include(c => c.ConsultantLevel)
                                    .Include(c => c.Certifications)
                    ) ?? throw new NotExistsException();

                var result = _mapper.Map<ConsultantViewModel>(consultant);
                return new ResponseModel
                {
                    Message = $"Lấy người tư vấn với id '{consultantId}' thành công",
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
                //var roleId = await _unitOfWork.RoleRepository
                //    .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.Name.Equals(RoleEnum.Consultant.ToString()));

                var consultantLevel = await _unitOfWork.ConsultantLevelRepository.GetByIdAsync(postModel.ConsultantLevelId)
                    ?? throw new NotExistsException();
                var university = await _unitOfWork.UniversityRepository.GetByIdGuidAsync(postModel.UniversityId)
                    ?? throw new NotExistsException();

                var listCertificate = new List<Certification>();

                var consultant = _mapper.Map<Consultant>(postModel);

                RegisterAccountModel accountModel = new RegisterAccountModel(
                    postModel.Name,
                    postModel.Email,
                    postModel.Password,
                    postModel.Phone);
                var accountId = await _unitOfWork.AccountRepository.CreateAccountAndWallet(accountModel, RoleEnum.Consultant);

                //consultant.Id = Guid.NewGuid();
                consultant.AccountId = accountId;

                foreach (var certi in postModel.Certifications)
                {
                    var certification = _mapper.Map<Certification>(certi);
                    listCertificate.Add(certification);
                }
                consultant.Certifications = listCertificate;

                await _unitOfWork.ConsultantRepository.AddAsync(consultant);
                await _unitOfWork.SaveChangesAsync();
                var result = _mapper.Map<ConsultantViewModel>(consultant);
                return new ResponseModel
                {
                    Message = $"Người tư vấn mới đã được tạo thành công",
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
                var consultant = await _unitOfWork.ConsultantRepository
                    .SingleOrDefaultAsync(
                    predicate: o => o.Id.Equals(consultantId),
                    include: q => q.Include(c => c.Account).ThenInclude(a => a.Wallet)
                                    .Include(c => c.University)
                                    .Include(c => c.ConsultantLevel)
                                    .Include(c => c.Certifications)
                    ) ?? throw new NotExistsException();

                var exAccountConsultant = await _unitOfWork.AccountRepository.GetByIdGuidAsync(consultant.AccountId)
                    ?? throw new NotExistsException();
                _mapper.Map(putModel, consultant);

                var listNewCertificate = new List<Certification>();

                if (putModel.Certifications != null)
                {
                    foreach (var certification in putModel.Certifications)
                    {
                        if (certification.Id.HasValue)
                        {
                            var existingCertification = consultant.Certifications
                                .FirstOrDefault(s => s.Id == certification.Id.Value) ?? throw new NotExistsException();

                            _mapper.Map(certification, existingCertification);
                        }
                        else
                        {
                            var newCertification = _mapper.Map<Certification>(certification);
                            newCertification.ConsultantId = consultantId;
                            listNewCertificate.Add(newCertification);
                        }
                    }
                    if (listNewCertificate.Count > 0)
                        await _unitOfWork.CertificationRepository.AddRangeAsync(listNewCertificate);

                    var CertificationToRemove = consultant.Certifications
                        .Where(s => !putModel.Certifications.Any(c => c.Id == s.Id))
                        .ToList();

                    foreach (var certification in CertificationToRemove)
                    {
                        consultant.Certifications.Remove(certification);
                    }
                }

                await _unitOfWork.ConsultantRepository.UpdateAsync(consultant);
                await _unitOfWork.AccountRepository.UpdateAsync(exAccountConsultant);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<ConsultantViewModel>(consultant);
                return new ResponseModel
                {
                    Message = $"Người tư vấn với id '{consultantId}' đã được cập nhật thành công",
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
                    ?? throw new NotExistsException();

                var exAccountConsultant = await _unitOfWork.AccountRepository.GetByIdGuidAsync(consultant.AccountId)
                    ?? throw new NotExistsException();
                exAccountConsultant.Status = AccountStatus.Blocked;
                //await _unitOfWork.ConsultantRepository.UpdateAsync(consultant);
                await _unitOfWork.AccountRepository.UpdateAsync(exAccountConsultant);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<ConsultantViewModel>(consultant);
                return new ResponseModel
                {
                    Message = $"Người tư vấn với id '{consultantId}' đã được xóa thành công",
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
                .GetBySearchAsync(
                    filter,
                    orderBy,
                    include: q => q.Include(s => s.Account)
                                        .ThenInclude(a => a.Wallet)
                                    .Include(s => s.ConsultantLevel)
                                    .Include(s => s.University)
                                    .Include(s => s.Certifications),
                    pageIndex: searchModel.currentPage,
                    pageSize: searchModel.pageSize
                );

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
