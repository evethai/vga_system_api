using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Model.PersonalTest;
using Domain.Model.Response;

namespace Infrastructure.Persistence.Service
{
    public class PersonalTestService : IPersonalTestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PersonalTestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> CreatePersonalTest(PersonalTestPostModel model)
        {
            var personalTest = _mapper.Map<PersonalTest>(model);
            personalTest.CreateAt = DateTime.UtcNow;
            personalTest.Status = true;
            await _unitOfWork.PersonalTestRepository.AddAsync(personalTest);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Create personal test success",
                Data = personalTest
            };
        }

        public async Task<ResponseModel> UpdatePersonalTest(Guid id, PersonalTestPostModel model)
        {
            var personalTest = await _unitOfWork.PersonalTestRepository.GetByIdGuidAsync(id);
            if (personalTest == null)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Personal test not found"
                };
            }
            personalTest.Name = model.Name;
            personalTest.Description = model.Description;
            await _unitOfWork.PersonalTestRepository.UpdateAsync(personalTest);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Update personal test success",
                Data = personalTest
            };
        }
    }
}
