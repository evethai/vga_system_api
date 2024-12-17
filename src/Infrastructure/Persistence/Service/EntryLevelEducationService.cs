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
using Domain.Model.Consultant;
using Domain.Model.EntryLevelEducation;
using Domain.Model.Response;
using Domain.Model.TimeSlot;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Service
{
    public class EntryLevelEducationService : IEntryLevelEducationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EntryLevelEducationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get entry level education with paginate
        public async Task<ResponseEntryLevelEducationModel> GetListEntryLevelsWithPaginateAsync(EntryLevelEducationSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.EntryLevelEducationRepository.BuildFilterAndOrderBy(searchModel);
            var entryLevel = await _unitOfWork.EntryLevelEducationRepository
                .GetBySearchAsync(
                    filter,
                    orderBy,
                    pageIndex: searchModel.currentPage,
                    pageSize: searchModel.pageSize
                );

            var total = await _unitOfWork.EntryLevelEducationRepository.CountAsync(filter);
            var listEntryLevels = _mapper.Map<List<EntryLevelEducationViewModel>>(entryLevel);
            return new ResponseEntryLevelEducationModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                entryLevels = listEntryLevels,
            };
        }
        #endregion

        #region Get entry level education by id
        public async Task<ResponseModel> GetEntryLevelByIdAsync(Guid entryLevelId)
        {
            try
            {
                var entryLevel = await _unitOfWork.EntryLevelEducationRepository.GetByIdGuidAsync(entryLevelId)
                    ?? throw new NotExistsException();
                var result = _mapper.Map<EntryLevelEducationViewModel>(entryLevel);
                return new ResponseModel
                {
                    Message = $"Lấy cấp độ đầu vào với id '{entryLevelId}' thành công",
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

        #region Create entry level
        public async Task<ResponseModel> CreateEntryLevelAsync(EntryLevelEducationPostModel postModel)
        {
            try
            {
                var entryLevel = _mapper.Map<EntryLevelEducation>(postModel);
                await _unitOfWork.EntryLevelEducationRepository.AddAsync(entryLevel);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<EntryLevelEducationViewModel>(entryLevel);
                return new ResponseModel
                {
                    Message = "Cấp độ đầu vào được tạo thành công",
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

        #region Update entry level
        public async Task<ResponseModel> UpdateEntryLevelAsync(EntryLevelEducationPutModel putModel, Guid entryLevelId)
        {
            try
            {
                var entryLevel = await _unitOfWork.EntryLevelEducationRepository.GetByIdGuidAsync(entryLevelId)
                        ?? throw new NotExistsException();
                _mapper.Map(putModel, entryLevel);
                await _unitOfWork.EntryLevelEducationRepository.UpdateAsync(entryLevel);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<EntryLevelEducationViewModel>(entryLevel);
                return new ResponseModel
                {
                    Message = $"Cấp độ đầu vào với id '{entryLevelId}' đã được cập nhật thành công",
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

        #region Delete entry level
        public async Task<ResponseModel> DeleteEntryLevelAsync(Guid entryLevelId)
        {
            try
            {
                var entryLevel = await _unitOfWork.EntryLevelEducationRepository.GetByIdGuidAsync(entryLevelId)
                       ?? throw new NotExistsException();
                entryLevel.Status = false;
                await _unitOfWork.EntryLevelEducationRepository.UpdateAsync(entryLevel);
                await _unitOfWork.SaveChangesAsync();
                var result = _mapper.Map<EntryLevelEducationViewModel>(entryLevel);
                return new ResponseModel
                {
                    Message = $"Cấp độ đầu vào với id '{entryLevelId}' đã được xóa thành công",
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

    }
}
