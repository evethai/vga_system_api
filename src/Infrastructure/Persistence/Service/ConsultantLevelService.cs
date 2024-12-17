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
using Domain.Model.ConsultantLevel;
using Domain.Model.Response;
using Domain.Model.Student;
using Domain.Model.TimeSlot;
using Microsoft.EntityFrameworkCore;

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
                    ?? throw new NotExistsException();
                var result = _mapper.Map<ConsultantLevelViewModel>(consultantLevel);
                return new ResponseModel
                {
                    Message = $"Lấy cấp độ người tư vấn với id '{consultantLevelId}' thành công",
                    IsSuccess = true,
                    Data = result,
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
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
                consultantLevel.Status = true;
                await _unitOfWork.ConsultantLevelRepository.AddAsync(consultantLevel);
                await _unitOfWork.SaveChangesAsync();
                var result = _mapper.Map<ConsultantLevelViewModel>(consultantLevel);
                return new ResponseModel
                {
                    Message = "Cấp độ người tư cấn tạo thành công",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
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
                    ?? throw new NotExistsException();
                if (putModel.PriceOnSlot.HasValue && putModel.PriceOnSlot == 0)
                {
                    putModel.PriceOnSlot = consultantLevel.PriceOnSlot;
                }
                _mapper.Map(putModel, consultantLevel);
                await _unitOfWork.ConsultantLevelRepository.UpdateAsync(consultantLevel);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<ConsultantLevelViewModel>(consultantLevel);
                return new ResponseModel
                {
                    Message = $"Cấp độ người tư vấn với id '{consultantLevelId}' đã được cập nhật thành công",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
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
                    ?? throw new NotExistsException();
                consultantLevel.Status = false;
                await _unitOfWork.ConsultantLevelRepository.UpdateAsync(consultantLevel);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<ConsultantLevelViewModel>(consultantLevel);
                return new ResponseModel
                {
                    Message = $"Cấp độ người tư vấn với id '{consultantLevelId}' đã được xóa thành công",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        #endregion

        #region Get list consultant level with paginate
        public async Task<ResponseConsultantLevelModel> GetListConsultantLevelWithPaginateAsync(ConsultantLevelSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.ConsultantLevelRepository.BuildFilterAndOrderBy(searchModel);
            var levels = await _unitOfWork.ConsultantLevelRepository
                .GetBySearchAsync(
                    filter, 
                    orderBy,
                    pageIndex: searchModel.currentPage,
                    pageSize: searchModel.pageSize
                 );

            var total = await _unitOfWork.ConsultantLevelRepository.CountAsync(filter);
            var listLevel = _mapper.Map<List<ConsultantLevelViewModel>>(levels);
            return new ResponseConsultantLevelModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                consultantLevels = listLevel,
            };
        }
        #endregion

        #region Get all level async
        public async Task<ResponseModel> GetAllConsultantLevelAsync()
        {
            try
            {
                var levels = await _unitOfWork.ConsultantLevelRepository.GetAllAsync()
                    ?? throw new NotExistsException();
                var result = _mapper.Map<List<ConsultantLevelViewModel>>(levels);
                return new ResponseModel
                {
                    IsSuccess = true,
                    Data = result,
                    Message = "Consultant levels retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        #endregion
    }
}
