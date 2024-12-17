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
using Domain.Model.EntryLevelEducation;
using Domain.Model.MajorCategory;
using Domain.Model.Response;
using Domain.Model.TimeSlot;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Service
{
    public class MajorCategoryService : IMajorCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MajorCategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get list with paginate
        public async Task<ResponseMajorCategoryModel> GetListMajorCategorysWithPaginateAsync(MajorCategorySearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.MajorCategoryRepository.BuildFilterAndOrderBy(searchModel);
            var majorCategory = await _unitOfWork.MajorCategoryRepository
                .GetBySearchAsync(
                    filter,
                    orderBy,
                    pageIndex: searchModel.currentPage,
                    pageSize: searchModel.pageSize
                );

            var total = await _unitOfWork.MajorCategoryRepository.CountAsync(filter);
            var listMajorCategorys = _mapper.Map<List<MajorCategoryViewModel>>(majorCategory);
            return new ResponseMajorCategoryModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                majorCategorys = listMajorCategorys,
            };
        }
        #endregion

        #region Get by id
        public async Task<ResponseModel> GetMajorCategoryByIdAsync(Guid majorCategoryID)
        {
            try
            {
                var majorCategory = await _unitOfWork.MajorCategoryRepository.GetByIdGuidAsync(majorCategoryID)
                    ?? throw new NotExistsException();
                var result = _mapper.Map<MajorCategoryViewModel>(majorCategory);
                return new ResponseModel
                {
                    Message = $"Lấy nhóm ngành với id '{majorCategoryID}' thành công",
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
        public async Task<ResponseModel> CreateMajorCategoryAsync(MajorCategoryPostModel postModel)
        {
            try
            {
                var majorCategory = _mapper.Map<MajorCategory>(postModel);
                await _unitOfWork.MajorCategoryRepository.AddAsync(majorCategory);
                await _unitOfWork.SaveChangesAsync();
                var result = _mapper.Map<MajorCategoryViewModel>(majorCategory);
                return new ResponseModel
                {
                    Message = "Nhóm ngành được tạo thành công",
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
        public async Task<ResponseModel> UpdateMajorCategoryAsync(MajorCategoryPutModel putModel, Guid majorCategoryID)
        {
            try
            {
                var majorCategory = await _unitOfWork.MajorCategoryRepository.GetByIdGuidAsync(majorCategoryID)
                        ?? throw new NotExistsException();
                _mapper.Map(putModel, majorCategory);
                await _unitOfWork.MajorCategoryRepository.UpdateAsync(majorCategory);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<MajorCategoryViewModel>(majorCategory);
                return new ResponseModel
                {
                    Message = $"Nhóm ngành với id '{majorCategoryID}' đã được cập nhật thành công",
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
        public async Task<ResponseModel> DeleteMajorCategoryAsync(Guid majorCategoryID)
        {
            try
            {
                var majorCategory = await _unitOfWork.MajorCategoryRepository.GetByIdGuidAsync(majorCategoryID)
                       ?? throw new NotExistsException();
                majorCategory.Status = false;
                await _unitOfWork.MajorCategoryRepository.UpdateAsync(majorCategory);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<MajorCategoryViewModel>(majorCategory);
                return new ResponseModel
                {
                    Message = $"Nhóm ngành với id '{majorCategoryID}' đã được xóa thành công",
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
