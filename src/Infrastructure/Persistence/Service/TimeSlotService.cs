﻿using System;
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
                    ?? throw new Exception($"Time slot not found by id: {timeSlotId}");
                var result = _mapper.Map<TimeSlotViewModel>(timeSlot);
                return new ResponseModel
                {
                    Message = $"Get time slot by id '{timeSlotId}' successfull",
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
                var timeSlot = _mapper.Map<TimeSlot>(postModel);
                timeSlot.Status = true;
                await _unitOfWork.TimeSlotRepository.AddAsync(timeSlot);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel
                {
                    Message = "Time slot was created successfully",
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
                        ?? throw new Exception($"Time slot not found by id: {timeSlotId}");
                _mapper.Map(putModel, timeSlot);
                await _unitOfWork.TimeSlotRepository.UpdateAsync(timeSlot);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<TimeSlotViewModel>(timeSlot);
                return new ResponseModel
                {
                    Message = $"Time slot with id '{timeSlotId}' was updated successfully",
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
                       ?? throw new Exception($"Time slot not found by id: {timeSlotId}");
                timeSlot.Status = false;
                await _unitOfWork.TimeSlotRepository.UpdateAsync(timeSlot);
                await _unitOfWork.SaveChangesAsync();
                var result = _mapper.Map<TimeSlotViewModel>(timeSlot);
                return new ResponseModel
                {
                    Message = $"Time slot with id '{timeSlotId}' was deleted successfully",
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

        #region
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
    }
}
