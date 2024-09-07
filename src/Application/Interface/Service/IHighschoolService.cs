using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Highschool;
using Domain.Model.Response;

namespace Application.Interface.Service;
public interface IHighschoolService
{
    Task<ResponseHighSchoolModel> GetListHighSchoolAsync(HighschoolSearchModel searchModel);
    Task<HighschoolModel> GetHighschoolByIdAsync(Guid HighschoolId);
    Task<ResponseModel> CreateHighschoolAsyns(HighschoolPostModel postModel);
    Task<ResponseModel> UpdateHighschoolAsynsl(HighschoolPutModel putModel);
    Task<ResponseModel> DeleteHighschool(Guid HighschoolId);
}
