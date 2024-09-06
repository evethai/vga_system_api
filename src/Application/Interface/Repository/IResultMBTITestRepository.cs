using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model;
using Domain.Model.MBTI;

namespace Application.Interface.Repository
{
    public interface IResultMBTITestRepository : IGenericRepository<Result>
    {
        Task<ResultModel> CalculateResultMBTITest(StudentSelectedModel result);
    }
}
