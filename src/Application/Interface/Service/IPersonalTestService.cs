using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.PersonalTest;
using Domain.Model.Response;

namespace Application.Interface.Service
{
    public interface IPersonalTestService
    {
        Task<ResponseModel> CreatePersonalTest(PersonalTestPostModel model);
        Task<ResponseModel> UpdatePersonalTest(Guid id, PersonalTestPostModel model);
    }
}
