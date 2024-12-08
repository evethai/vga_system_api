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
using Domain.Model.Response;
using Domain.Model.StudentChoice;

namespace Infrastructure.Persistence.Service
{
    public class StudentChoiceService : IStudentChoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public  StudentChoiceService (IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> CreateNewStudentCare (StudentChoicePostModel model)
        {
            string name = "";
            if (model.isMajor) 
            {
                name = await _unitOfWork.MajorRepository.SingleOrDefaultAsync(selector: a => a.Name, predicate: a => a.Id.Equals(model.MajorOrOccupationId));
                if (name == null)
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Major or Occupation Id is not Existed!"
                    };
            }
            else
            {
                name = await _unitOfWork.OccupationRepository.SingleOrDefaultAsync(selector: a => a.Name, predicate: a => a.Id.Equals(model.MajorOrOccupationId));
                if (name == null)
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Major or Occupation Id is not Existed!"
                    };
            }
            StudentChoice stChoice = new StudentChoice
            {
                StudentId = model.StudentId,
                MajorOrOccupationId = model.MajorOrOccupationId,
                MajorOrOccupationName = name,
                Rating = model.Rating,
                isMajor = model.isMajor,
                Type = StudentChoiceType.Care
            };
            var result = await _unitOfWork.StudentChoiceRepository.AddAsync(stChoice);
            await _unitOfWork.SaveChangesAsync();


            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Create new student care is success .",
                Data = result
            };
        }


        public async Task<ResponseModel> GetAllStudentCareById (Guid StudentId)
        {
            var result = await _unitOfWork.StudentChoiceRepository.GetListAsync(predicate: a => a.StudentId == StudentId && a.Type == StudentChoiceType.Care,orderBy: a => a.OrderByDescending(a => a.Rating));
            if (result == null)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "List care of student is null!"
                };
            }
            List<StudentCareModel>? listMajor = new List<StudentCareModel>();
            List<StudentCareModel>? listOcc = new List<StudentCareModel>();
            foreach (var stCare in result)
            {
                if (stCare.isMajor)
                {
                    listMajor.Add(new StudentCareModel
                    {
                        MajorOrOccupationId = stCare.MajorOrOccupationId,
                        MajorOrOccupationName = stCare.MajorOrOccupationName,
                        Rating = stCare.Rating
                    });
                }
                else
                {
                    listOcc.Add(new StudentCareModel
                    {
                        MajorOrOccupationId = stCare.MajorOrOccupationId,
                        MajorOrOccupationName = stCare.MajorOrOccupationName,
                        Rating = stCare.Rating
                    });
                }
            }
            StudentChoiceModel model = new StudentChoiceModel
            {
                listMajor = listMajor,
                listOccupation = listOcc
            };

            return new ResponseModel
            {
                IsSuccess = true,
                Message = "List Student care ",
                Data = model
            };

        }

    }
}
