using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Certification;
using Domain.Model.ConsultationTime;
using Domain.Model.Response;

namespace Infrastructure.Persistence.Service
{
    public class CertificationService : ICertificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CertificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get by id
        public async Task<ResponseModel> GetCertificationByIdAsync(int id)
        {
            try
            {
                var consultationTime = await _unitOfWork.CertificationRepository
                    .GetByIdAsync(id) ?? throw new NotExistsException();

                var result = _mapper.Map<CertificationViewModel>(consultationTime);
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = $"Lấy bằng cấp với id '{id}' thành công",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while get certification time by id: {ex.Message}"
                };
            }
        }
        #endregion

        #region Create certification time
        public async Task<ResponseModel> CreateCertificationAsync(CertificationPostModel postModel)
        {
            try
            {
                var consultant = await _unitOfWork.ConsultantRepository.GetByIdGuidAsync(postModel.ConsultantId) ?? throw new NotExistsException();

                var certi = _mapper.Map<Certification>(postModel);

                await _unitOfWork.CertificationRepository.AddAsync(certi);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<CertificationViewModel>(certi);
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = $"Bằng cấp đã được tạo thành công",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while create certification time: {ex.Message}"
                };
            }
        }
        #endregion

        #region Delete certification time
        public async Task<ResponseModel> DeleteCertificationAsync(int certificationId)
        {
            try
            {
                var certification = await _unitOfWork.CertificationRepository.GetByIdAsync(certificationId)
                    ?? throw new Exception($"Bằng cấp với id '{certificationId}' không tồn tại");
                
                await _unitOfWork.CertificationRepository.DeleteAsync(certification);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Bằng cấp đã được xóa thành công"
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while delete certification time: {ex.Message}"
                };
            }
        }
        #endregion

    }
}
