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
using Domain.Model.Occupation;
using Domain.Model.OccupationalGroup;
using Domain.Model.Response;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence.Service
{
    public class OccupationService : IOccupationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OccupationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get list with paginate
        public async Task<ResponseOccupationModel> GetListOccupationsWithPaginateAsync(OccupationSearchModel searchModel)
        {
            var (filter, orderBy) = _unitOfWork.OccupationRepository.BuildFilterAndOrderBy(searchModel);
            var occupation = await _unitOfWork.OccupationRepository
                .GetBySearchAsync(
                    filter,
                    orderBy,
                    include: q => q.Include(o => o.EntryLevelEducation)
                                    .Include(o => o.OccupationalGroup)
                                    .Include(o => o.OccupationalSKills)
                                        .ThenInclude(os => os.WorkSkills),
                    pageIndex: searchModel.currentPage,
                    pageSize: searchModel.pageSize
                );

            var total = await _unitOfWork.OccupationRepository.CountAsync(filter);
            var listOccupations = _mapper.Map<List<OccupationViewModel>>(occupation);
            return new ResponseOccupationModel
            {
                total = total,
                currentPage = searchModel.currentPage,
                occupations = listOccupations,
            };
        }
        #endregion

        #region Get by id
        public async Task<ResponseModel> GetOccupationByIdAsync(Guid occupationId,Guid studentId)
        {
            try
            {
                var occupation = await _unitOfWork.OccupationRepository
                    .SingleOrDefaultAsync(
                    predicate: o => o.Id.Equals(occupationId),
                    include: o => o.Include(o => o.EntryLevelEducation)
                                .Include(o => o.OccupationalGroup)
                                .Include(o => o.OccupationalSKills)
                                    .ThenInclude(os => os.WorkSkills)
                    ) ?? throw new NotExistsException();

                var result = _mapper.Map<OccupationViewModel>(occupation);
                var isCare = await _unitOfWork.StudentChoiceRepository.SingleOrDefaultAsync(predicate: x => x.StudentId == studentId && x.MajorOrOccupationId == occupationId && x.Type == Domain.Enum.StudentChoiceType.Care);
                if (isCare != null)
                {
                    result.IsCare = true;
                    result.CareLevel = isCare.Rating;
                }

                var numberLike = await _unitOfWork.StudentChoiceRepository.GetListAsync(predicate: x => x.MajorOrOccupationId == occupationId && x.Type == Domain.Enum.StudentChoiceType.Care && x.Rating > 0);
                if (numberLike != null)
                {
                    result.NumberCare = numberLike.Count();
                }
                return new ResponseModel
                {
                    Message = $"Lấy nghề với id '{occupation}' thành công",
                    IsSuccess = true,
                    Data = result,
                };

            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while get occupation by id: {ex.Message}"
                };
            }
        }
        #endregion

        #region Create
        public async Task<ResponseModel> CreateOccupationAsync(OccupationPostModel postModel)
        {
            try
            {
                var entryLevelEducation = await _unitOfWork.EntryLevelEducationRepository
                    .GetByIdGuidAsync(postModel.EntryLevelEducationId)
                    ?? throw new NotExistsException();

                var occupationalGroup = await _unitOfWork.OccupationalGroupRepository
                    .GetByIdGuidAsync(postModel.OccupationalGroupId)
                    ?? throw new NotExistsException();

                var listSkill = new List<OccupationalSKills>();

                var occupation = new Occupation
                {
                    Id = Guid.NewGuid(),
                    EntryLevelEducationId = postModel.EntryLevelEducationId,
                    OccupationalGroupId = postModel.OccupationalGroupId,
                    Name = postModel.Name,
                    Description = postModel.Description,
                    HowToWork = postModel.HowToWork,
                    WorkEnvironment = postModel.WorkEnvironment,
                    Education = postModel.Education,
                    PayScale = postModel.PayScale,
                    JobOutlook = postModel.JobOutlook,
                    Status = postModel.Status
                };

                foreach (var skillModel in postModel.OccupationalSkills)
                {
                    var workSkill = await _unitOfWork.WorkSkillsRepository
                        .GetByIdGuidAsync(skillModel.WorkSkillsId)
                        ?? throw new NotExistsException();

                    var occupationalSkill = new OccupationalSKills
                    {
                        Id = Guid.NewGuid(),
                        WorkSkillsId = skillModel.WorkSkillsId,
                        OccupationId = occupation.Id,
                        Content = skillModel.Content,
                        Status = skillModel.Status
                    };
                    listSkill.Add(occupationalSkill);
                }
                occupation.OccupationalSKills = listSkill;
                await _unitOfWork.OccupationRepository.AddAsync(occupation);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<OccupationViewModel>(occupation);
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Tạo nghề nghiệp thành công",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while creating the occupation: {ex.Message}"
                };
            }
        }
        #endregion

        #region Update
        public async Task<ResponseModel> UpdateOccupationAsync(Guid occupationId, OccupationPutModel putModel)
        {
            try
            {
                var occupation = await _unitOfWork.OccupationRepository
                    .SingleOrDefaultAsync(
                    predicate: o => o.Id.Equals(occupationId),
                    include: o => o.Include(o => o.EntryLevelEducation)
                                .Include(o => o.OccupationalGroup)
                                .Include(o => o.OccupationalSKills)
                                    .ThenInclude(os => os.WorkSkills)
                    ) ?? throw new NotExistsException();

                _mapper.Map(putModel, occupation);
                var listNewSkill = new List<OccupationalSKills>();

                if (putModel.Skills != null)
                {
                    foreach (var skillModel in putModel.Skills)
                    {
                        if (skillModel.Id.HasValue)
                        {
                            var existingSkill = occupation.OccupationalSKills
                                .FirstOrDefault(s => s.Id == skillModel.Id.Value) ?? throw new NotExistsException();

                            _mapper.Map(skillModel, existingSkill);
                        }
                        else
                        {
                            var newSkill = _mapper.Map<OccupationalSKills>(skillModel);
                            newSkill.Id = Guid.NewGuid();
                            newSkill.OccupationId = occupation.Id;
                            listNewSkill.Add(newSkill);
                        }
                    }
                    if (listNewSkill.Count > 0)
                        await _unitOfWork.OccupationSkillRepository.AddRangeAsync(listNewSkill);

                    var skillsToRemove = occupation.OccupationalSKills
                        .Where(s => !putModel.Skills.Any(um => um.Id == s.Id))
                        .ToList();

                    foreach (var skill in skillsToRemove)
                    {
                        occupation.OccupationalSKills.Remove(skill);
                    }
                }

                await _unitOfWork.OccupationRepository.UpdateAsync(occupation);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<OccupationViewModel>(occupation);
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Nghề được cập nhật thành công.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while update occupation by id: {ex.Message}"
                };
            }
        }
        #endregion

        #region Delete
        public async Task<ResponseModel> DeleteOccupationAsync(Guid occupationId)
        {
            try
            {
                var occupation = await _unitOfWork.OccupationRepository
                     .SingleOrDefaultAsync(
                     predicate: o => o.Id.Equals(occupationId),
                     include: o => o.Include(o => o.EntryLevelEducation)
                                 .Include(o => o.OccupationalGroup)
                                 .Include(o => o.OccupationalSKills)
                                     .ThenInclude(os => os.WorkSkills)
                     ) ?? throw new NotExistsException();

                occupation.Status = false;
                await _unitOfWork.OccupationRepository.UpdateAsync(occupation);
                await _unitOfWork.SaveChangesAsync();

                var result = _mapper.Map<OccupationViewModel>(occupation);
                return new ResponseModel
                {
                    Message = $"Nghề với id '{occupationId}' đã được xóa thành công",
                    IsSuccess = true,
                    Data = result,
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred while delete occupation: {ex.Message}"
                };
            }
        }
        #endregion
    }
}
