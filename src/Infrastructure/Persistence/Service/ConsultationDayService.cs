using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.ConsultationDay;
using Domain.Model.Response;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Service
{
    public class ConsultationDayService : IConsultationDayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ConsultationDayService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get consultation day by id
        public async Task<ResponseModel> GetConsultationDayByIdAsync(Guid id)
        {
            var consultationDay = await _unitOfWork.ConsultationDayRepository
                .GetConsultationDayWithTimesByIdAsync(id) ?? throw new NotExistsException();

            var result = _mapper.Map<ConsultationDayViewModel>(consultationDay);
            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Consultation day retrieved successfully.",
                Data = result
            };
        }
        #endregion

        #region Create new consultation day with times
        public async Task<ResponseModel> CreateConsultationDayWithTimesAsync(ConsultationDayPostModel postModel)
        {
            if (postModel.Day <= DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "The consultation day must be in the future."
                };
            }

            Expression<Func<ConsultationDay, bool>> exsitingDayFilter = x =>
            x.ConsultantId.Equals(postModel.ExpertId) &&
            x.Day.Equals(postModel.Day);
            var existingDay = await _unitOfWork.ConsultationDayRepository
                .SingleOrDefaultAsync(
                predicate: exsitingDayFilter,
                include: query => query.Include(cd => cd.ConsultationTimes)
                );

            if (existingDay != null)
            {
                var newConsultationTimes = postModel.ConsultationTimes
                    .Where(ct => !existingDay.ConsultationTimes.Any(existingCt => existingCt.TimeSlotId == ct.TimeSlotId))
                    .Select(ct => new ConsultationTime
                    {
                        Id = Guid.NewGuid(),
                        ConsultationDayId = existingDay.Id,
                        TimeSlotId = ct.TimeSlotId,
                        Status = (int)ConsultationTimeStatusEnum.Available,
                        Note = ct.Note
                    }).ToList();

                if (newConsultationTimes.Any())
                {
                    await _unitOfWork.ConsultationTimeRepository.AddRangeAsync(newConsultationTimes);
                    await _unitOfWork.SaveChangesAsync();
                }

                var resultDayExisted = _mapper.Map<ConsultationDayViewModel>(existingDay);
                return new ResponseModel
                {
                    IsSuccess = newConsultationTimes.Any()
                              ? true
                              : false,
                    Message = newConsultationTimes.Any()
                              ? "New consultation times added successfully."
                              : "No new consultation times to add.",
                    Data = newConsultationTimes.Any()
                              ? resultDayExisted
                              : null,
                };
            }

            var consultationDay = _mapper.Map<ConsultationDay>(postModel);
            consultationDay.Id = Guid.NewGuid();
            consultationDay.Status = (int)ConsultationDayStatusEnum.Available;

            var consultationTimes = postModel.ConsultationTimes.Select(ct => new ConsultationTime
            {
                Id = Guid.NewGuid(),
                ConsultationDayId = consultationDay.Id,
                TimeSlotId = ct.TimeSlotId,
                Status = (int)ConsultationDayStatusEnum.Available,
                Note = ct.Note
            }).ToList();
            consultationDay.ConsultationTimes = consultationTimes;

            await _unitOfWork.ConsultationDayRepository.AddAsync(consultationDay);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<ConsultationDayViewModel>(consultationDay);
            return new ResponseModel
            {
                IsSuccess = true,
                Message = "New consultation day and times created successfully.",
                Data = result
            };
        }
        #endregion

        #region Delete consultation day
        public async Task<ResponseModel> DeleteConsultationDayAsync(Guid consultationDayId)
        {
            var consultationDay = await _unitOfWork.ConsultationDayRepository
                .SingleOrDefaultAsync(
                predicate: x => x.Id.Equals(consultationDayId),
                include: query => query.Include(cd => cd.ConsultationTimes)
                ) ?? throw new Exception($"Consultation day with id '{consultationDayId}' not exist");

            if (consultationDay.ConsultationTimes.Any(ct => ct.Status == (int)ConsultationTimeStatusEnum.Booked))
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Cannot delete the consultation day because one or more consultation times are already booked."
                };
            }

            consultationDay.Status = (int)ConsultationDayStatusEnum.Deleted;

            await _unitOfWork.ConsultationDayRepository.UpdateAsync(consultationDay);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Consultation day deleted successfully."
            };
        }
        #endregion

    }
}
