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
using Domain.Model.Major;
using Domain.Model.Response;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Service
{
    public class MajorService : IMajorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MajorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get list with paginate
        public async Task<ResponseMajorModel> GetListMajorsWithPaginateAsync(MajorSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.MajorRepository.BuildFilterAndOrderBy(searchModel);
            var major = await _unitOfWork.MajorRepository
                .GetBySearchAsync(
                    filter,
                    orderBy,
                    include: q => q.Include(m => m.MajorCategory),
                    pageIndex: searchModel.currentPage,
                    pageSize: searchModel.pageSize
                );

            var total = await _unitOfWork.MajorRepository.CountAsync(filter);
            var listMajors = _mapper.Map<List<MajorViewModel>>(major);
            return new ResponseMajorModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                majors = listMajors,
            };
        }
        #endregion

        #region Get by id
        public async Task<ResponseModel> GetMajorByIdAsync(Guid majorId)
        {
            try
            {
                var major = await _unitOfWork.MajorRepository
                    .SingleOrDefaultAsync(
                        predicate: m => m.Id.Equals(majorId),
                        include: query => query.Include(m => m.MajorCategory)
                    ) ?? throw new NotExistsException();

                var result = _mapper.Map<MajorViewModel>(major);
                return new ResponseModel
                {
                    Message = $"Lấy ngành với id '{major}' thành công",
                    IsSuccess = true,
                    Data = result,
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while get major by id: {ex.Message}"
                };
            }
        }
        #endregion

        #region Create
        public async Task<ResponseModel> CreateMajorAsync(MajorPostModel postModel)
        {
            try
            {
                var major = _mapper.Map<Major>(postModel);
                await _unitOfWork.MajorRepository.AddAsync(major);
                await _unitOfWork.SaveChangesAsync();
                var result = _mapper.Map<MajorViewModel>(major);
                return new ResponseModel
                {
                    Message = "Ngành được tạo thành công",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while create major: {ex.Message}"
                };
            }
        }
        #endregion

        #region Update
        public async Task<ResponseModel> UpdateMajorAsync(MajorPutModel putModel, Guid majorId)
        {
            try
            {
                var major = await _unitOfWork.MajorRepository
                    .SingleOrDefaultAsync(
                        predicate: m => m.Id.Equals(majorId),
                        include: query => query.Include(m => m.MajorCategory)
                    ) ?? throw new NotExistsException();
                _mapper.Map(putModel, major);
                await _unitOfWork.MajorRepository.UpdateAsync(major);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<MajorViewModel>(major);
                return new ResponseModel
                {
                    Message = $"Ngành với id '{majorId}' đã được cập nhật thành công",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while update major: {ex.Message}"
                };
            }
        }
        #endregion

        #region Delete
        public async Task<ResponseModel> DeleteMajorAsync(Guid majorId)
        {
            try
            {
                var major = await _unitOfWork.MajorRepository.GetByIdGuidAsync(majorId)
                       ?? throw new NotExistsException();
                major.Status = false;
                await _unitOfWork.MajorRepository.UpdateAsync(major);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<MajorViewModel>(major);
                return new ResponseModel
                {
                    Message = $"Ngành với id '{majorId}' đã được xóa thành công",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while delete major: {ex.Message}"
                };
            }
        }
        #endregion

    }
}
