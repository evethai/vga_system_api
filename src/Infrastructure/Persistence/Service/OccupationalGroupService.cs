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
using Domain.Model.OccupationalGroup;
using Domain.Model.Response;

namespace Infrastructure.Persistence.Service
{
    public class OccupationalGroupService : IOccupationalGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OccupationalGroupService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get list with paginate
        public async Task<ResponseOccupationalGroupModel> GetListOccupationalGroupsWithPaginateAsync(OccupationalGroupSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.OccupationalGroupRepository.BuildFilterAndOrderBy(searchModel);
            var occupationalGroup = await _unitOfWork.OccupationalGroupRepository
                .GetBySearchAsync(
                    filter,
                    orderBy,
                    pageIndex: searchModel.currentPage,
                    pageSize: searchModel.pageSize
                );

            var total = await _unitOfWork.OccupationalGroupRepository.CountAsync(filter);
            var listoccupationalGroups = _mapper.Map<List<OccupationalGroupViewModel>>(occupationalGroup);
            return new ResponseOccupationalGroupModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                occupationalGroups = listoccupationalGroups,
            };
        }
        #endregion

        #region Get by id
        public async Task<ResponseModel> GetOccupationalGroupByIdAsync(Guid occupationalGroupId)
        {
            try
            {
                var occupationalGroup = await _unitOfWork.OccupationalGroupRepository.GetByIdGuidAsync(occupationalGroupId)
                    ?? throw new NotExistsException();
                var result = _mapper.Map<OccupationalGroupViewModel>(occupationalGroup);
                return new ResponseModel
                {
                    Message = $"Lấy nhóm nghề với id '{occupationalGroupId}' thành công",
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

        #region Create
        public async Task<ResponseModel> CreateOccupationalGroupAsync(OccupationalGroupPostModel postModel)
        {
            try
            {
                var occupationalGroup = _mapper.Map<OccupationalGroup>(postModel);
                await _unitOfWork.OccupationalGroupRepository.AddAsync(occupationalGroup);
                await _unitOfWork.SaveChangesAsync();
                var result = _mapper.Map<OccupationalGroupViewModel>(occupationalGroup);
                return new ResponseModel
                {
                    Message = "Nhóm nghề được tạo thành công",
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

        #region Update
        public async Task<ResponseModel> UpdateOccupationalGroupAsync(OccupationalGroupPutModel putModel, Guid occupationalGroupId)
        {
            try
            {
                var occupationalGroup = await _unitOfWork.OccupationalGroupRepository.GetByIdGuidAsync(occupationalGroupId)
                        ?? throw new NotExistsException();
                _mapper.Map(putModel, occupationalGroup);
                await _unitOfWork.OccupationalGroupRepository.UpdateAsync(occupationalGroup);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<OccupationalGroupViewModel>(occupationalGroup);
                return new ResponseModel
                {
                    Message = $"Nhóm nghề với id '{occupationalGroupId}' đã được cập nhật thành công",
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

        #region Delete
        public async Task<ResponseModel> DeleteOccupationalGroupAsync(Guid occupationalGroupId)
        {
            try
            {
                var occupationalGroup = await _unitOfWork.OccupationalGroupRepository.GetByIdGuidAsync(occupationalGroupId)
                       ?? throw new NotExistsException();
                occupationalGroup.Status = false;
                await _unitOfWork.OccupationalGroupRepository.UpdateAsync(occupationalGroup);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<OccupationalGroupViewModel>(occupationalGroup);
                return new ResponseModel
                {
                    Message = $"Nhóm nghề với id '{occupationalGroupId}' đã được xóa thành công",
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
