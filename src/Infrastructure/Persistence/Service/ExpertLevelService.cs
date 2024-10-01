using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Model.ExpertLevel;
using Domain.Model.Response;
using Domain.Model.TimeSlot;

namespace Infrastructure.Persistence.Service
{
    public class ExpertLevelService : IExpertLevelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ExpertLevelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get expert level by id
        public async Task<ResponseModel> GetExpertLevelByIdAsync(int expertLevelId)
        {
            var expertLevel = await _unitOfWork.ExpertLevelRepository.GetByIdAsync(expertLevelId)
                ?? throw new Exception($"Expert level not found by id: {expertLevelId}");
            var result = _mapper.Map<ExpertLevelViewModel>(expertLevel);
            return new ResponseModel
            {
                Message = $"Get expert level by id '{expertLevelId}' successfull",
                IsSuccess = true,
                Data = result,
            };
        }
        #endregion

        #region Create new expert level
        public async Task<ResponseModel> CreateExpertLevelAsync(ExpertLevelPostModel postModel)
        {
            var expertLevel = _mapper.Map<ExpertLevel>(postModel);
            await _unitOfWork.ExpertLevelRepository.AddAsync(expertLevel);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Expert level was created successfully",
                IsSuccess = true,
                Data = expertLevel,
            };
        }
        #endregion

        #region Update expert level
        public async Task<ResponseModel> UpdateExpertLevelAsync(int expertLevelId, ExpertLevelPutModel putModel)
        {
            var expertLevel = await _unitOfWork.ExpertLevelRepository.GetByIdAsync(expertLevelId)
                ?? throw new Exception($"Expert level not found by id: {expertLevelId}");
            _mapper.Map(putModel, expertLevel);
            await _unitOfWork.ExpertLevelRepository.UpdateAsync(expertLevel);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<ExpertLevelViewModel>(expertLevel);
            return new ResponseModel
            {
                Message = $"Expert level with id '{expertLevelId}' was updated successfully",
                IsSuccess = true,
                Data = result,
            };
        }
        #endregion

        #region Delete expert level
        public async Task<ResponseModel> DeleteExpertLevelAsync(int expertLevelId)
        {
            var expertLevel = await _unitOfWork.ExpertLevelRepository.GetByIdAsync(expertLevelId)
                   ?? throw new Exception($"Expert level not found by id: {expertLevelId}");
            expertLevel.Status = false;
            await _unitOfWork.ExpertLevelRepository.UpdateAsync(expertLevel);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<ExpertLevelViewModel>(expertLevel);
            return new ResponseModel
            {
                Message = $"Expert level with id '{expertLevelId}' was deleted successfully",
                IsSuccess = true,
                Data = result,
            };
        }
        #endregion

    }
}
