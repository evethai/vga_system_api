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
using Domain.Model.Account;
using Domain.Model.Highschool;
using Domain.Model.Response;
using Domain.Model.University;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Service
{
    public class UniversityService : IUniversityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UniversityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseModel> CreateUniversityAsync(UniversityPostModel postModel)
        {
            var university = _mapper.Map<University>(postModel);
            RegisterAccountModel accountModel = new RegisterAccountModel(postModel.Name
                , postModel.Email
                , postModel.Password
                , postModel.Phone);
            var AccountId = await _unitOfWork.AccountRepository.CreateAccountAndWallet(accountModel, RoleEnum.University);
            university.AccountId = AccountId;
            var result = await _unitOfWork.UniversityRepository.AddAsync(university);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "University Created Successfully",
                IsSuccess = true,
                Data = postModel,
            };
        }

        public async Task<ResponseModel> DeleteUniversityAsync(Guid Id)
        {
            var exUniversity = await _unitOfWork.UniversityRepository.GetByIdGuidAsync(Id);
            if (exUniversity == null)
            {
                return new ResponseModel
                {
                    Message = "University Id is not found",
                    IsSuccess = false,
                };
            }
            var exAccount = await _unitOfWork.AccountRepository.GetByIdGuidAsync(exUniversity.AccountId);
            if (exAccount == null)
            {
                return new ResponseModel
                {
                    Message = "University Account Id is not found",
                    IsSuccess = false,
                };
            }
            exAccount.Status = AccountStatus.Blocked;
            await _unitOfWork.AccountRepository.UpdateAsync(exAccount);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<UniversityModel>(exUniversity);
            return new ResponseModel
            {
                Message = "Delete University is Successfully",
                IsSuccess = true,
                Data = Id
            };
        }

        public async Task<ResponseUniversityModel> GetListUniversityAsync(UniversitySearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.UniversityRepository.BuildFilterAndOrderBy(searchModel);
            var universities = await _unitOfWork.UniversityRepository
                .GetBySearchAsync(filter, orderBy,
                q => q.Include(s => s.Account)
                       .ThenInclude(a => a.Wallet)
                       .Include(a=>a.UniversityLocations),
                pageIndex: searchModel.currentPage,
                pageSize: searchModel.pageSize);
            var total = await _unitOfWork.UniversityRepository.CountAsync(filter);
            var listUniversity = _mapper.Map<List<UniversityModel>>(universities);
            return new ResponseUniversityModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                _universities = listUniversity,
            };
        }

        public async Task<UniversityModel> GetUniversityByIdAsync(Guid Id)
        {
            var university = await _unitOfWork.UniversityRepository.
                SingleOrDefaultAsync(predicate: c => c.Id.Equals(Id), 
                include: a => a.Include(a => a.Account).ThenInclude(a => a.Wallet)
                .Include(a=>a.UniversityLocations));
            return _mapper.Map<UniversityModel>(university);
        }

        public async Task<ResponseModel> UpdateUniversityAsync(UniversityPutModel putModel, Guid Id)
        {
            var exitUniversity = await _unitOfWork.UniversityRepository.GetByIdGuidAsync(Id);
            if (exitUniversity == null)
            {
                return new ResponseModel
                {
                    Message = "University Id is not found",
                    IsSuccess = false,
                };
            }
            exitUniversity.Description = putModel.Description;
            exitUniversity.Code = putModel.Code;
            exitUniversity.EstablishedYear = putModel.EstablishedYear;           
            var exitAccount = await _unitOfWork.AccountRepository.GetByIdGuidAsync(exitUniversity.AccountId);
            if (exitAccount == null)
            {
                return new ResponseModel
                {
                    Message = "University Account Id is not found",
                    IsSuccess = false,
                };
            }
            exitAccount.Name = putModel.Name;
            exitAccount.Phone = putModel.Phone;
            exitAccount.Email = putModel.Email;
            exitAccount.Password = PasswordUtil.HashPassword(putModel.Password);
            await _unitOfWork.AccountRepository.UpdateAsync(exitAccount);
            await _unitOfWork.UniversityRepository.UpdateAsync(exitUniversity);
            var result = _mapper.Map<UniversityModel>(exitUniversity);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "University Updated Successfully",
                IsSuccess = true,
                Data = putModel,
            };
        }

        public async Task<ResponseModel> CreateUniversityLocationAsync(Guid Id, List<UniversityLocationModel> universityLocationModels)
        {
            var exitUniversity = await _unitOfWork.UniversityRepository.CreateUniversityLocation(Id, universityLocationModels);
            return new ResponseModel
            {
                Message = "Create University Location Successfully",
                IsSuccess = true,
                Data = "Number location create is " + exitUniversity
            };
        }

        public async Task<ResponseModel> UpdateUniversityLocationAsync(int Id, UniversityLocationPutModel universityLocationModels)
        {
            var updateLocation = await _unitOfWork.UniversityRepository.UpdateUniversityLocation(Id, universityLocationModels);
            if(updateLocation == false)
            {
                return new ResponseModel
                {
                    Message = "Update University Location is fails",
                    IsSuccess = false
                };
            }
            return new ResponseModel
            {
                Message = "Update University Location Successfully",
                IsSuccess = true,
                Data = "University location update is " + Id
            };
        }

        public async Task<ResponseModel> DeleteUniversityLocationAsync(int Id)
        {
            var deleteLocation = await _unitOfWork.UniversityRepository.DeleteUniversityLocation(Id);
            if (deleteLocation == false)
            {
                return new ResponseModel
                {
                    Message = "Delete University Location is fails",
                    IsSuccess = false
                };
            }
            return new ResponseModel
            {
                Message = "Delete University Location Successfully",
                IsSuccess = true,
                Data = "University location Delete is " + Id
            };
        }
    }
}
