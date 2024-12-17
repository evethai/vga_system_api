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
using Domain.Model.Response;
using Domain.Model.WorkSkills;

namespace Infrastructure.Persistence.Service
{
    public class WorkSkillsService : IWorkSkillsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WorkSkillsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get list with paginate
        public async Task<ResponseWorkSkillsModel> GetListWorkSkillsWithPaginateAsync(WorkSkillsSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.WorkSkillsRepository.BuildFilterAndOrderBy(searchModel);
            var workSkill = await _unitOfWork.WorkSkillsRepository
                .GetBySearchAsync(
                    filter,
                    orderBy,
                    pageIndex: searchModel.currentPage,
                    pageSize: searchModel.pageSize
                );

            var total = await _unitOfWork.WorkSkillsRepository.CountAsync(filter);
            var listworkSkills = _mapper.Map<List<WorkSkillsViewModel>>(workSkill);
            return new ResponseWorkSkillsModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                workSkills = listworkSkills,
            };
        }
        #endregion

        #region Get by id
        public async Task<ResponseModel> GetWorkSkillByIdAsync(Guid workSkillId)
        {
            try
            {
                var workSkill = await _unitOfWork.WorkSkillsRepository.GetByIdGuidAsync(workSkillId)
                    ?? throw new NotExistsException();
                var result = _mapper.Map<WorkSkillsViewModel>(workSkill);
                return new ResponseModel
                {
                    Message = $"Lấy kỹ năng nghề nghiệp với id '{workSkillId}' thành công",
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
        public async Task<ResponseModel> CreateWorkSkillAsync(WorkSkillsPostModel postModel)
        {
            try
            {
                var workSkill = _mapper.Map<WorkSkills>(postModel);
                await _unitOfWork.WorkSkillsRepository.AddAsync(workSkill);
                await _unitOfWork.SaveChangesAsync();
                var result = _mapper.Map<WorkSkillsViewModel>(workSkill);
                return new ResponseModel
                {
                    Message = "Kỹ năng nghề nghiệp được tạo thành công",
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
        public async Task<ResponseModel> UpdateWorkSkillAsync(WorkSkillsPutModel putModel, Guid workSkillId)
        {
            try
            {
                var workSkill = await _unitOfWork.WorkSkillsRepository.GetByIdGuidAsync(workSkillId)
                        ?? throw new NotExistsException();
                _mapper.Map(putModel, workSkill);
                await _unitOfWork.WorkSkillsRepository.UpdateAsync(workSkill);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<WorkSkillsViewModel>(workSkill);
                return new ResponseModel
                {
                    Message = $"Kỹ năng nghề nghiệp  với id '{workSkillId}' đã được cập nhật thành công",
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
        public async Task<ResponseModel> DeleteWorkSkillAsync(Guid workSkillId)
        {
            try
            {
                var workSkill = await _unitOfWork.WorkSkillsRepository.GetByIdGuidAsync(workSkillId)
                       ?? throw new NotExistsException();
                workSkill.Status = false;
                await _unitOfWork.WorkSkillsRepository.UpdateAsync(workSkill);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<WorkSkillsViewModel>(workSkill);
                return new ResponseModel
                {
                    Message = $"Kỹ năng nghề nghiệp  với id '{workSkillId}' đã được xóa thành công",
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
