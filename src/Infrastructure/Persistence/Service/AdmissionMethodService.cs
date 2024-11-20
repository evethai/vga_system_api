using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface.Service;
using Application.Interface;
using AutoMapper;
using Domain.Model.AdmissionInformation;
using Domain.Model.Response;
using Microsoft.EntityFrameworkCore;
using Domain.Entity;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Infrastructure.Persistence.Service
{
    public class AdmissionMethodService : IAdmissionMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdmissionMethodService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> CreateAdmissionMethodAsync(AdmissionMethodPostModel postModel)
        {
            var method = _mapper.Map<AdmissionMethod>(postModel);
            await _unitOfWork.AdmissionMethodRepository.AddAsync(method);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Create Admission Method is successfully",
                IsSuccess = true,
                Data = postModel
            };
        }

        public async Task<ResponseModel> DeleteAdmissionMethodAsync(Guid Id)
        {
            var exit = await _unitOfWork.AdmissionMethodRepository.GetByIdGuidAsync(Id) ?? throw new Exception("Admission Method Id is not found");            
            exit.Status = false;
            await _unitOfWork.AdmissionMethodRepository.UpdateAsync(exit);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Delete Admission Method is Successfully",
                IsSuccess = true
            };
        }

        public async Task<ResponseModel> GetAdmissionMethodById(Guid Id)
        {
            var exitMethod = await _unitOfWork.AdmissionMethodRepository.GetByIdGuidAsync(Id) ??  throw new Exception("Id is not found");           
            var result = _mapper.Map<AdmissionMethodModel>(exitMethod);
            return new ResponseModel
            {
                Message = "Get AdmissionMethod Successfully",
                IsSuccess = true,
                Data = result
            };
        }

        public async Task<ResponseAdmissionMethodModel> GetListAdmissionMethodAsync(AdmissionMethodSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.AdmissionMethodRepository.BuildFilterAndOrderByAdmissionMethod(searchModel);
            var admissionMethod = await _unitOfWork.AdmissionMethodRepository.GetBySearchAsync
                (filter, orderBy,
                pageIndex: searchModel.currentPage,
                pageSize: searchModel.pageSize);
            var total = await _unitOfWork.AdmissionMethodRepository.CountAsync(filter);
            var listInfo = _mapper.Map<List<AdmissionMethod>>(admissionMethod);
            return new ResponseAdmissionMethodModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                _admissionMethodModels = listInfo,
            };
        }

        public async Task<ResponseModel> UpdateAdmissionMethodAsync(Guid Id, AdmissionMethodPutModel putModel)
        {
            var exitMethod = await _unitOfWork.AdmissionMethodRepository.GetByIdGuidAsync(Id) ?? throw new Exception("Admisson Method Id is not found");           
            exitMethod.Description = putModel.Description;
            exitMethod.Name = putModel.Name;
            await _unitOfWork.AdmissionMethodRepository.UpdateAsync(exitMethod);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Admission Method Id is successfully",
                IsSuccess = true,
                Data = putModel
            };
        }
    }
}
