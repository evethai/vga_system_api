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
using Domain.Model.ConsultationDay;
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
                    ) ?? throw new Exception($"Ngày tư vấn với id '{consultationDayId}' không tồn tại");
                if (consultationDay.ConsultationTimes.Any(existingCt => existingCt.TimeSlotId == postModel.TimeSlotId))
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = $"Khoảng thời gian tư vấn đã tồn tại với id: '{postModel.TimeSlotId}'",
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
                    Message = $"Khoảng thời gian tư vấn đã được tạo thành công",
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
                    ?? throw new Exception($"Khoảng thời gian tư vấn với id '{consultationTimeId}' không tồn tại");
                if (consultationTime.Status == (int)ConsultationTimeStatusEnum.Booked)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Không thể xóa khoảng thời gian tư vấn này vì nó đã được đặt"
                    };
                }
                consultationTime.Status = (int)ConsultationTimeStatusEnum.Deleted;

                await _unitOfWork.ConsultationTimeRepository.UpdateAsync(consultationTime);
                await _unitOfWork.SaveChangesAsync();

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Khoảng thời gian tư vấn đã được xóa thành công"
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

        #region Get by id
        public async Task<ResponseModel> GetConsultationTimeByIdAsync(Guid id)
        {
            try
            {
                var consultationTime = await _unitOfWork.ConsultationTimeRepository
                    .SingleOrDefaultAsync(
                        predicate: ct => ct.Id.Equals(id),
                        include: ct => ct.Include(ct =>ct.SlotTime)
                    ) ?? throw new NotExistsException();

                var result = _mapper.Map<ConsultationTimeViewModel>(consultationTime);
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = $"Lấy khoảng thời gian tư vấn với id '{id}' thành công",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while get consultation time by id: {ex.Message}"
                };
            }
        }
        #endregion
    }
}
