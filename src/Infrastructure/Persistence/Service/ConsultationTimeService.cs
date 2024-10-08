using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.ConsultationTime;
using Domain.Model.Response;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Service
{
    public class ConsultationTimeService : IConsultationTimeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ConsultationTimeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Create consultation time
        public async Task<ResponseModel> CreateConsultationTimeAsync(ConsultationTimePostModel postModel, Guid consultationDayId)
        {
            try
            {
                var consultationDay = await _unitOfWork.ConsultationDayRepository
                    .SingleOrDefaultAsync(
                    predicate: x => x.Id.Equals(consultationDayId),
                    include: query => query.Include(cd => cd.ConsultationTimes)
                    ) ?? throw new Exception($"Consultation day with id '{consultationDayId}' not exist");
                if (consultationDay.ConsultationTimes.Any(existingCt => existingCt.TimeSlotId == postModel.TimeSlotId))
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = $"New consultation time is existed with time slot id '{postModel.TimeSlotId}' .",
                    };
                }

                var consultationTime = _mapper.Map<ConsultationTime>(postModel);
                consultationTime.Id = Guid.NewGuid();
                consultationTime.ConsultationDayId = consultationDayId;
                consultationTime.Status = (int)ConsultationTimeStatusEnum.Available;

                await _unitOfWork.ConsultationTimeRepository.AddAsync(consultationTime);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<ConsultationTimeViewModel>(consultationTime);
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = $"New consultation time was created successfully.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while create consultation time: {ex.Message}"
                };
            }
        }
        #endregion

        #region Delete consultation time
        public async Task<ResponseModel> DeleteConsultationTimeAsync(Guid consultationTimeId)
        {
            try
            {
                var consultationTime = await _unitOfWork.ConsultationTimeRepository.GetByIdGuidAsync(consultationTimeId)
                    ?? throw new Exception($"Consultation time with id '{consultationTimeId}' not exist");
                if (consultationTime.Status == (int)ConsultationTimeStatusEnum.Booked)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Cannot delete the consultation time because it already booked."
                    };
                }
                consultationTime.Status = (int)ConsultationTimeStatusEnum.Deleted;

                await _unitOfWork.ConsultationTimeRepository.UpdateAsync(consultationTime);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Consultation time deleted successfully."
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while delete consultation time: {ex.Message}"
                };
            }
        }
        #endregion
    }
}
