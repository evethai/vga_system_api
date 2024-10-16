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
using Domain.Model.Response;
using Domain.Model.TimeSlot;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Service
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TimeSlotService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get time slot by id
        public async Task<ResponseModel> GetTimeSlotByIdAsync(int timeSlotId)
        {
            try
            {
                var timeSlot = await _unitOfWork.TimeSlotRepository.GetByIdAsync(timeSlotId)
                    ?? throw new NotExistsException();
                var result = _mapper.Map<TimeSlotViewModel>(timeSlot);
                return new ResponseModel
                {
                    Message = $"Lấy khoảng thời gian với id '{timeSlotId}' thành công",
                    IsSuccess = true,
                    Data = result,
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while get time slot by id: {ex.Message}"
                };
            }
        }
        #endregion

        #region Create new time slot
        public async Task<ResponseModel> CreateTimeSlotAsync(TimeSlotPostModel postModel)
        {
            try
            {
                if (postModel.EndTime < postModel.StartTime)
                    throw new NotExistsException();

                var timeSlot = _mapper.Map<TimeSlot>(postModel);
                timeSlot.Status = true;
                await _unitOfWork.TimeSlotRepository.AddAsync(timeSlot);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel
                {
                    Message = "Khoảng thời gian được tạo thành công",
                    IsSuccess = true,
                    Data = timeSlot,
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while create time slot: {ex.Message}"
                };
            }
        }
        #endregion

        #region Update time slot 
        public async Task<ResponseModel> UpdateTimeSlotAsync(TimeSlotPutModel putModel, int timeSlotId)
        {
            try
            {
                var timeSlot = await _unitOfWork.TimeSlotRepository.GetByIdAsync(timeSlotId)
                        ?? throw new NotExistsException();
                _mapper.Map(putModel, timeSlot);
                await _unitOfWork.TimeSlotRepository.UpdateAsync(timeSlot);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<TimeSlotViewModel>(timeSlot);
                return new ResponseModel
                {
                    Message = $"Khoảng thời gian với id '{timeSlotId}' đã được cập nhật thành công",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while update time slot: {ex.Message}"
                };
            }
        }
        #endregion

        #region Delete time slot
        public async Task<ResponseModel> DeleteTimeSlotAsync(int timeSlotId)
        {
            try
            {
                var timeSlot = await _unitOfWork.TimeSlotRepository.GetByIdAsync(timeSlotId)
                       ?? throw new NotExistsException();
                timeSlot.Status = false;
                await _unitOfWork.TimeSlotRepository.UpdateAsync(timeSlot);
                await _unitOfWork.SaveChangesAsync();
                var result = _mapper.Map<TimeSlotViewModel>(timeSlot);
                return new ResponseModel
                {
                    Message = $"Khoảng thời gian với id '{timeSlotId}' đã được xóa thành công",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while delete time slot: {ex.Message}"
                };
            }
        }
        #endregion

        #region Get all time slots
        public async Task<ResponseModel> GetAllTimeSlotsAsync()
        {
            try
            {
                var timeSlots = await _unitOfWork.TimeSlotRepository.GetAllAsync()
                    ?? throw new NotExistsException();
                var result = _mapper.Map<List<TimeSlotViewModel>>(timeSlots);
                return new ResponseModel
                {
                    IsSuccess = true,
                    Data = result,
                    Message = "Time slots retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while get all time slots: {ex.Message}"
                };
            }
        }
        #endregion

        #region  Get list time slots with paginate
        public async Task<ResponseTimeSlotModel> GetListTimeSlotsWithPaginateAsync(TimeSlotSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.TimeSlotRepository.BuildFilterAndOrderBy(searchModel);
            var slot = await _unitOfWork.TimeSlotRepository
                .GetBySearchAsync(
                    filter,
                    orderBy,
                    pageIndex: searchModel.currentPage,
                    pageSize: searchModel.pageSize
                );

            var total = await _unitOfWork.TimeSlotRepository.CountAsync(filter);
            var listSlots = _mapper.Map<List<TimeSlotViewModel>>(slot);
            return new ResponseTimeSlotModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                timeSlots = listSlots,
            };
        }
        #endregion
    }
}
