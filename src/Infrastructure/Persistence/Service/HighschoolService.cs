using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
        var highschool = await _unitOfWork.HighschoolRepository.
            SingleOrDefaultAsync(predicate: c => c.Id.Equals(HighschoolId), include: a => a.Include(a => a.Account).ThenInclude(a=>a.Wallet)) ?? throw new Exception("Id is not found");
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
        var checkIdRegion = _unitOfWork.RegionRepository.GetByIdGuidAsync(postModel.RegionId) ?? throw new Exception("Region id is not found");
        var highschool = _mapper.Map<HighSchool>(postModel);
        RegisterAccountModel accountModel = new RegisterAccountModel(
            postModel.Name
            ,postModel.Email
            , postModel.Password
            , postModel.Phone
            ,postModel.Image_Url);
        var AccountId = await _unitOfWork.AccountRepository.CreateAccountAndWallet(accountModel, RoleEnum.HighSchool);
        highschool.AccountId = AccountId;
        await _unitOfWork.HighschoolRepository.AddAsync(highschool);
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
        var exitHighschool = await _unitOfWork.HighschoolRepository
            .GetByIdGuidAsync(Id) ?? throw new Exception("Id is not found");
        exitHighschool.Address = putModel.Address;
        var exitRegion = await _unitOfWork.RegionRepository
            .GetByIdGuidAsync(putModel.RegionId) ?? throw new Exception("Region id is not found");
        exitHighschool.RegionId = exitRegion.Id;
        var exitAccount = await _unitOfWork.AccountRepository
            .GetByIdGuidAsync(exitHighschool.AccountId) ?? throw new Exception("Highschool Account Id is not found");
        await _unitOfWork.AccountRepository.checkPhoneAndMail(exitAccount.Id,putModel.Email, putModel.Phone);
        exitAccount.Name = putModel.Name;
        exitAccount.Phone = putModel.Phone;
        exitAccount.Email = putModel.Email;
        exitAccount.Image_Url = putModel.Image_Url;
        await _unitOfWork.AccountRepository.UpdateAsync(exitAccount);      
        await _unitOfWork.HighschoolRepository.UpdateAsync(exitHighschool);
        await _unitOfWork.SaveChangesAsync();
        return new ResponseModel
        {
            Message = " Highschool Updated Successfully",
            IsSuccess = true,
            Data = putModel,
        };
    }

    public async Task<ResponseModel> DeleteHighschoolAsync(Guid Id)
    {
        var exHighschool = await _unitOfWork.HighschoolRepository.GetByIdGuidAsync(Id) ?? throw new Exception("Id is not found");
        var exAccount = await _unitOfWork.AccountRepository.GetByIdGuidAsync(exHighschool.AccountId) ?? throw new Exception("Highschool Acccount Id is not found");
        exAccount.Status = AccountStatus.Blocked;
        await _unitOfWork.AccountRepository.UpdateAsync(exAccount);
        await _unitOfWork.SaveChangesAsync();
        return new ResponseModel
        {
            Message = " Highschool Delete Successfully",
            IsSuccess = true
        };
    }
}
