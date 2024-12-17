using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
            var exitUniversity = await _unitOfWork.UniversityRepository.GetByIdGuidAsync(UniversityId) 
                ?? throw new Exception("Không tìm thấy ID trường đại học");            
            await _unitOfWork.AdmissionInformationRepository.CreateListAdmissionInformation(UniversityId, postModel);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Tạo thông tin nhập học thành công",
                IsSuccess = true,
                Data = postModel
            };
        }

        public async Task<ResponseModel> GetAdmissionInformationByIdAsync(int Id)
        {
            var info = await _unitOfWork.AdmissionInformationRepository.
                SingleOrDefaultAsync(predicate: c => c.Id == Id,
                include: a => a.Include(a => a.University).ThenInclude(s=>s.Account).Include(a=>a.Major).Include(a=>a.AdmissionMethod)) 
                ?? throw new Exception("Không tìm thấy ID");
            var result = _mapper.Map<AdmissionInformationModel>(info);
            return new ResponseModel
            {
                Message = "Nhận Theo Id Thành Công",
                IsSuccess = true,
                Data = result
            };
        }
        public async Task<ResponseAdmissionInformationModel> GetListAdmissionInformationAsync(AdmissionInformationSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.AdmissionInformationRepository.BuildFilterAndOrderByAdmissionInformation(searchModel);
            var admissionInfo = await _unitOfWork.AdmissionInformationRepository
                .GetBySearchAsync(filter, orderBy,
                s=>s.Include(s=>s.AdmissionMethod)
                .Include(s=>s.University).ThenInclude(s=>s.Account)
                .Include(s=>s.Major),
                pageIndex: searchModel.currentPage,
                pageSize: searchModel.pageSize);
            var total = await _unitOfWork.AdmissionInformationRepository.CountAsync(filter);
            var listInfo = _mapper.Map<List<AdmissionInformationModel>>(admissionInfo);
            return new ResponseAdmissionInformationModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                _admissionInformationModel = listInfo
            };
        }

        public async Task<ResponseModel> DeleteAdmissionInformationAsync(int Id)
        {
            var exitAdmissionInfo = await _unitOfWork.AdmissionInformationRepository.GetByIdAsync(Id) 
                ?? throw new Exception("Không tìm thấy ID thông tin tuyển sinh") ;
            await _unitOfWork.AdmissionInformationRepository.DeleteAsync(exitAdmissionInfo);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Xóa thông tin nhập học thành công",
                IsSuccess = true
            };
        }

        public async Task<ResponseModel> UpdateAdmissionInformationAsync(List<AdmissionInformationPutModel> putModel)
        {
            var result = await _unitOfWork.AdmissionInformationRepository.CheckAdmissionInformation(putModel);
            if(result  == false)
            {
                throw new Exception("Lỗi");
            }
            return new ResponseModel
            {
                Message = "Cập nhật thông tin tuyển sinh thành công",
                IsSuccess = true,
                Data = putModel
            };
        }        
    }
}
