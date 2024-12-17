using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Response;
using Domain.Model.TestType;

namespace Application.Interface.Service
{
    public interface ITestTypeService
    {
        Task<IEnumerable<TestTypeModel>> GetAllTestTypes();
        Task<TestTypeModel> GetTestTypeById(Guid id);
        Task<ResponseModel> UpdateTestTypeById(Guid id, int Point);
    }
}
