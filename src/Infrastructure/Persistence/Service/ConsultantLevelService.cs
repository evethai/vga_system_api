using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Model.ExpertLevel;
using Domain.Model.Response;
using Domain.Model.TimeSlot;

namespace Infrastructure.Persistence.Service
{
    public class ConsultantLevelService : IConsultantLevelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ConsultantLevelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get consultant level by id
        public async Task<ResponseModel> GetConsultantLevelByIdAsync(int consultantLevelId)
        {
            try
            {
                var consultantLevel = await _unitOfWork.ConsultantLevelRepository.GetByIdAsync(consultantLevelId)
                    ?? throw new Exception($"Consultant level not found by id: {consultantLevelId}");
                var result = _mapper.Map<ConsultantLevelViewModel>(consultantLevel);
                return new ResponseModel
                {
                    Message = $"Get consultant level by id '{consultantLevelId}' successfull",
                    IsSuccess = true,
                    Data = result,
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while get consultant level by id: {ex.Message}"
                };
            }
        }
        #endregion

        #region Create new consultant level
        public async Task<ResponseModel> CreateConsultantLevelAsync(ConsultantLevelPostModel postModel)
        {
            try
            {
                var consultantLevel = _mapper.Map<ConsultantLevel>(postModel);
                await _unitOfWork.ConsultantLevelRepository.AddAsync(consultantLevel);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel
                {
                    Message = "Consultant level was created successfully",
                    IsSuccess = true,
                    Data = consultantLevel,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while create consultant level: {ex.Message}"
                };
            }
        }
        #endregion

        #region Update consultant level
        public async Task<ResponseModel> UpdateConsultantLevelAsync(ConsultantLevelPutModel putModel, int consultantLevelId)
        {
            try
            {
                var consultantLevel = await _unitOfWork.ConsultantLevelRepository.GetByIdAsync(consultantLevelId)
                                ?? throw new Exception($"Consultant level not found by id: {consultantLevelId}");
                _mapper.Map(putModel, consultantLevel);
                await _unitOfWork.ConsultantLevelRepository.UpdateAsync(consultantLevel);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<ConsultantLevelViewModel>(consultantLevel);
                return new ResponseModel
                {
                    Message = $"Consultant level with id '{consultantLevelId}' was updated successfully",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while update consultant level: {ex.Message}"
                };
            }
        }
        #endregion

        #region Delete consultant level
        public async Task<ResponseModel> DeleteConsultantLevelAsync(int consultantLevelId)
        {
            try
            {
                var consultantLevel = await _unitOfWork.ConsultantLevelRepository.GetByIdAsync(consultantLevelId)
                       ?? throw new Exception($"Consultant level not found by id: {consultantLevelId}");
                consultantLevel.Status = false;
                await _unitOfWork.ConsultantLevelRepository.UpdateAsync(consultantLevel);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<ConsultantLevelViewModel>(consultantLevel);
                return new ResponseModel
                {
                    Message = $"Consultant level with id '{consultantLevelId}' was deleted successfully",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while dalete consultant level: {ex.Message}"
                };
            }
        }
        #endregion

    }
}
