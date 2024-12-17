using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Model.Response;
using Domain.Model.TestType;

namespace Infrastructure.Persistence.Service
{
    public class TestTypeService : ITestTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TestTypeService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TestTypeModel>> GetAllTestTypes()
        {
            var testTypes = await _unitOfWork.TestTypeRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<TestTypeModel>>(testTypes);
            return result;

        }

        public async Task<TestTypeModel> GetTestTypeById(Guid id)
        {
            var testType = await _unitOfWork.TestTypeRepository.GetByIdGuidAsync(id);
            var result = _mapper.Map<TestTypeModel>(testType);
            return result;
        }

        public async Task<ResponseModel> UpdateTestTypeById(Guid id, int Point)
        {
            var testType = await _unitOfWork.TestTypeRepository.GetByIdGuidAsync(id);
            if(testType == null)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Test Type not found !"
                };
            }
            testType.Point = Point;
            await _unitOfWork.TestTypeRepository.UpdateAsync(testType);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Update successful .",
                Data = testType
            };
        }
    }
}
