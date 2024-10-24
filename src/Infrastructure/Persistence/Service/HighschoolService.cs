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
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Persistence.Service;
public class HighschoolService : IHighschoolService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public HighschoolService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<HighschoolModel> GetHighschoolByIdAsync(Guid HighschoolId)
    {
        var highschool = await _unitOfWork.HighschoolRepository.GetByIdGuidAsync(HighschoolId);
        return _mapper.Map<HighschoolModel>(highschool);
    }

    public async Task<ResponseHighSchoolModel> GetListHighSchoolAsync(HighschoolSearchModel searchModel)
    {
        var (filter, orderBy) = _unitOfWork.HighschoolRepository.BuildFilterAndOrderBy(searchModel);
        var highSchools = await _unitOfWork.HighschoolRepository
            .GetBySearchAsync(filter, orderBy,
            q => q.Include(s => s.Account)
                   .ThenInclude(a => a.Wallet),
            pageIndex: searchModel.currentPage,
            pageSize: searchModel.pageSize);
        var total = await _unitOfWork.HighschoolRepository.CountAsync(filter);
        var listHighschool = _mapper.Map<List<HighschoolModel>>(highSchools);
        return new ResponseHighSchoolModel
        {
            total = total,
            currentPage = searchModel.currentPage,
            highschools = listHighschool,
        };
    }
    public async Task<ResponseModel> CreateHighschoolAsync(HighschoolPostModel postModel)
    {
        var highschool = _mapper.Map<HighSchool>(postModel);
        RegisterAccountModel accountModel = new RegisterAccountModel(postModel.Name,postModel.Email
            , postModel.Password
            , postModel.Phone);
        var AccountId = await _unitOfWork.AccountRepository.CreateAccountAndWallet(accountModel, RoleEnum.HighSchool);
        highschool.AccountId = AccountId;
        var result = await _unitOfWork.HighschoolRepository.AddAsync(highschool);
        await _unitOfWork.SaveChangesAsync();
        return new ResponseModel
        {
            Message = " Highschool Created Successfully",
            IsSuccess = true,
            Data = postModel,
        };
    }

    public async Task<ResponseModel> UpdateHighschoolAsync(HighschoolPutModel putModel, Guid Id)
    {
        var exitHighschool = await _unitOfWork.HighschoolRepository.GetByIdGuidAsync(Id);
        if (exitHighschool == null)
        {
            return new ResponseModel
            {
                Message = "Highschool Id is not found",
                IsSuccess = true,
            };
        }
        //exitHighschool.LocationDetail = putModel.LocationDetail;
        //exitHighschool.Name = putModel.Name;
        exitHighschool.RegionId = putModel.RegionId;
        var exitAccount = await _unitOfWork.AccountRepository.GetByIdGuidAsync(exitHighschool.AccountId);
        if (exitAccount == null)
        {
            return new ResponseModel
            {
                Message = "Highschool Account Id is not found",
                IsSuccess = true,
            };
        }
        exitAccount.Phone = putModel.Phone;
        exitAccount.Email = putModel.Email;
        exitAccount.Password = PasswordUtil.HashPassword(putModel.Password);
        await _unitOfWork.AccountRepository.UpdateAsync(exitAccount);      
        await _unitOfWork.HighschoolRepository.UpdateAsync(exitHighschool);
        await _unitOfWork.SaveChangesAsync();
        return new ResponseModel
        {
            Message = " Highschool Updated Successfully",
            IsSuccess = true,
            Data = exitHighschool,
        };
    }

    public async Task<ResponseModel> DeleteHighschoolAsync(Guid Id)
    {
        var exHighschool = await _unitOfWork.HighschoolRepository.GetByIdGuidAsync(Id);
        if (exHighschool == null)
        {
            return new ResponseModel
            {
                Message = "Highschool Id is not found",
                IsSuccess = true,
            };
        }
        var exAccount = await _unitOfWork.AccountRepository.GetByIdGuidAsync(exHighschool.AccountId);
        if (exAccount == null)
        {
            return new ResponseModel
            {
                Message = "Highschool Acccount Id is not found",
                IsSuccess = true,
            };
        }
        exAccount.Status = AccountStatus.Blocked;
        await _unitOfWork.AccountRepository.UpdateAsync(exAccount);
        await _unitOfWork.SaveChangesAsync();
        return new ResponseModel
        {

            Message = " Highschool Delete Successfully",
            IsSuccess = true,
            Data = exHighschool
        };
    }
}
