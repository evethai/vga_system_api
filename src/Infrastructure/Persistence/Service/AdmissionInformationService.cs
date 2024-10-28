using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Model.AdmissionInformation;
using Domain.Model.Highschool;
using Domain.Model.Response;
using Domain.Model.University;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Service
{
    public class AdmissionInformationService : IAdmissionInformationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AdmissionInformationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> CreateAdmissionInformationAsync(Guid UniversityId, List<AdmissionInformationPostModel> postModel)
        {
            var exitUniversity = await _unitOfWork.UniversityRepository.GetByIdGuidAsync(UniversityId);
            if (exitUniversity == null)
            {
                return new ResponseModel
                {
                    Message = "University Id is not found",
                    IsSuccess = false
                };
            }
            await _unitOfWork.AdmissionInformationRepository.CreateListAdmissionInformation(UniversityId, postModel);
            return new ResponseModel
            {
                Message = "Create Admission Information is successfully",
                IsSuccess = true,
                Data = postModel
            };
        }

        public async Task<ResponseModel> GetAdmissionInformationByIdAsync(int Id)
        {
            var info = await _unitOfWork.AdmissionInformationRepository.
                SingleOrDefaultAsync(predicate: c => c.Id == Id,
                include: a => a.Include(a => a.University).Include(a=>a.Major).Include(a=>a.AdmissionMethod));
            if (info == null)
            {
                return new ResponseModel
                {
                    Message = "Id is not found",
                    IsSuccess = false
                };
            }
            var result = _mapper.Map<AdmissionInformationModel>(info);
            return new ResponseModel
            {
                Message = "Get By Id is Successfully",
                IsSuccess = true,
                Data = result
            };
        }

        public async Task<ResponseAdmissionInformationModel> GetListAdmissionInformationAsync(AdmissionInformationSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.AdmissionInformationRepository.BuildFilterAndOrderByAdmissionInformation(searchModel);
            var admissionInfo = await _unitOfWork.AdmissionInformationRepository
                .GetBySearchAsync(filter, orderBy,
                q => q.Include(s => s.AdmissionMethod).Include(a=>a.Major),
                pageIndex: searchModel.currentPage,
                pageSize: searchModel.pageSize);
            var total = await _unitOfWork.AdmissionInformationRepository.CountAsync(filter);
            var listInfo = _mapper.Map<List<AdmissionInformationModel>>(admissionInfo);
            return new ResponseAdmissionInformationModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                _admissionInformationModels = listInfo,
            };
        }

        public async Task<ResponseModel> DeleteAdmissionInformationAsync(int Id)
        {
            var exitAdmissionInfo = await _unitOfWork.AdmissionInformationRepository.GetByIdAsync(Id);
            if (exitAdmissionInfo == null)
            {
                return new ResponseModel
                {
                    Message = "Admission Information Id is not found",
                    IsSuccess = false
                };
            }
            await _unitOfWork.AdmissionInformationRepository.DeleteAsync(exitAdmissionInfo);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Delete Admission Information is Successfully",
                IsSuccess = true
            };
        }

        public async Task<ResponseModel> UpdateAdmissionInformationAsync(int Id, AdmissionInformationPutModel putModel)
        {
            var exitInfor =  await _unitOfWork.AdmissionInformationRepository.GetByIdAsync(Id);
            if (exitInfor == null)
            {
                return new ResponseModel
                {
                    Message = "Id is not found",
                    IsSuccess = false,
                };
            }
            var checkInfo = await _unitOfWork.AdmissionInformationRepository.CheckAdmissionInformation(putModel);
            if (checkInfo == false)
            {
                return new ResponseModel
                {
                    Message = "Major Id or Method Id is not found",
                    IsSuccess = false,
                };
            }
            exitInfor.AdmissionMethodId = putModel.AdmissionMethodId;
            exitInfor.MajorId = putModel.MajorId;
            exitInfor.QuantityTarget = putModel.QuantityTarget;
            exitInfor.TuitionFee = putModel.TuitionFee;
            exitInfor.Year = putModel.Year;
            await _unitOfWork.AdmissionInformationRepository.UpdateAsync(exitInfor);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Update Admission Information is successfully",
                IsSuccess = true,
                Data = putModel
            };
        }        
    }
}
