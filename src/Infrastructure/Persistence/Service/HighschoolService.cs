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
using Domain.Model.Highschool;
using Domain.Model.Response;

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
        var highSchools = await _unitOfWork.HighschoolRepository.GetByConditionAsync(filter, orderBy, pageIndex: searchModel.currentPage, pageSize: searchModel.pageSize);
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
        var roleId = await _unitOfWork.RoleRepository.SingleOrDefaultAsync(selector: x=> x.Id,predicate: x=> x.Name.Equals(RoleEnum.HighSchool.ToString()));
        highschool.Account = new Account
        {
            Id = Guid.NewGuid(), // Create new GUID for Account
            Email = postModel.Email,
            Phone = postModel.Phone,
            Password = PasswordUtil.HashPassword(postModel.Password),
            RoleId = roleId,
            Status = AccountStatus.Active,
            CreateAt = DateTime.Now
        };      
        var result = await _unitOfWork.HighschoolRepository.AddAsync(highschool);
        _unitOfWork.SaveChangesAsync();
        return new ResponseModel
        {
            Message = " Highschool Created Successfully",
            IsSuccess = true,
            Data = postModel,
        };
    }

    public async Task<ResponseModel> UpdateHighschoolAsync(HighschoolPutModel putModel)
    {
        var highschool = _mapper.Map<HighSchool>(putModel);
        var result = await _unitOfWork.HighschoolRepository.UpdateAsync(highschool);
        _unitOfWork.SaveChangesAsync();
        return new ResponseModel
        {
            Message = " Highschool Updated Successfully",
            IsSuccess = true,
            Data = highschool,
        };
    }
}
