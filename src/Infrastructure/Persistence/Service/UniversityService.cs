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
                , postModel.Phone, postModel.Image_Url);
            var AccountId = await _unitOfWork.AccountRepository.CreateAccountAndWallet(accountModel, RoleEnum.University);
            university.AccountId = AccountId;
            var result = await _unitOfWork.UniversityRepository.AddAsync(university);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Đại học được thành lập thành công",
                IsSuccess = true,
                Data = postModel,
            };
        }

        public async Task<ResponseModel> DeleteUniversityAsync(Guid Id)
        {
            var exUniversity = await _unitOfWork.UniversityRepository.GetByIdGuidAsync(Id) ?? throw new Exception("Id is not found");
            var exAccount = await _unitOfWork.AccountRepository.GetByIdGuidAsync(exUniversity.AccountId) ?? throw new Exception("Account Id is not found");
            exAccount.Status = AccountStatus.Blocked;
            await _unitOfWork.AccountRepository.UpdateAsync(exAccount);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<UniversityModel>(exUniversity);
            return new ResponseModel
            {
                Message = "Xóa trường đại học thành công",
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

        public async Task<UniversityModelGetBy> GetUniversityByIdAsync(Guid Id)
        {
            var university = await _unitOfWork.UniversityRepository.
                SingleOrDefaultAsync(predicate: c => c.Id.Equals(Id),
                include: a => a.Include(a => a.Account).ThenInclude(a => a.Wallet)
                .Include(a => a.UniversityLocations)
                //.Include(a => a.Consultants).ThenInclude(a => a.Account)
                //.Include(a => a.Consultants).ThenInclude(a => a.ConsultantLevel)
                .Include(a => a.AdmissionInformation).ThenInclude(a => a.AdmissionMethod)
                .Include(a => a.AdmissionInformation).ThenInclude(a => a.Major));

            if (university == null)
            {
                throw new Exception("Không tìm thấy ID");
            }
            return _mapper.Map<UniversityModelGetBy>(university);
        }

        public async Task<ResponseModel> UpdateUniversityAsync(UniversityPutModel putModel, Guid Id)
        {
            var exitUniversity = await _unitOfWork.UniversityRepository.GetByIdGuidAsync(Id) ?? throw new Exception("Không tìm thấy ID");
            exitUniversity.Description = putModel.Description;
            exitUniversity.Code = putModel.Code;
            exitUniversity.EstablishedYear = putModel.EstablishedYear;           
            var exitAccount = await _unitOfWork.AccountRepository.GetByIdGuidAsync(exitUniversity.AccountId) ?? throw new Exception("Không tìm thấy ID Tài khoản");
            await _unitOfWork.AccountRepository.checkPhoneAndMail(exitAccount.Id,putModel.Email, putModel.Phone);
            exitAccount.Name = putModel.Name;
            exitAccount.Phone = putModel.Phone;
            exitAccount.Email = putModel.Email;
            exitAccount.Image_Url = putModel.Image_Url;
            await _unitOfWork.AccountRepository.UpdateAsync(exitAccount);
            await _unitOfWork.UniversityRepository.UpdateAsync(exitUniversity);
            var result = _mapper.Map<UniversityModel>(exitUniversity);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Đại học đã cập nhật thành công",
                IsSuccess = true,
                Data = putModel,
            };
        }

        public async Task<ResponseModel> CreateUniversityLocationAsync(Guid Id, List<UniversityLocationModel> universityLocationModels)
        {
            var exitUniversity = await _unitOfWork.UniversityRepository.CreateUniversityLocation(Id, universityLocationModels);
            return new ResponseModel
            {
                Message = "Đại học đã cập nhật thành công",
                IsSuccess = true,
                Data = "Số vị trí tạo là " + exitUniversity
            };
        }

        public async Task<ResponseModel> UpdateUniversityLocationAsync(int Id, UniversityLocationPutModel universityLocationModels)
        {
            var updateLocation = await _unitOfWork.UniversityRepository.UpdateUniversityLocation(Id, universityLocationModels);
            return new ResponseModel
            {
                Message = "Cập nhật vị trí trường đại học thành công",
                IsSuccess = true,
                Data = "Cập nhật vị trí trường đại học là " + Id
            };
        }

        public async Task<ResponseModel> DeleteUniversityLocationAsync(int Id)
        {
            var deleteLocation = await _unitOfWork.UniversityRepository.DeleteUniversityLocation(Id);
            return new ResponseModel
            {
                Message = "Xóa vị trí trường đại học thành công",
                IsSuccess = true,
                Data = "Vị trí trường đại học Xóa là " + Id
            };
        }
    }
}
