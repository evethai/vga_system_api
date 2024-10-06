using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.ConsultationTime;
using Domain.Model.Response;

namespace Application.Interface.Service
{
    public interface IConsultationTimeService
    {
        Task<ResponseModel> CreateConsultationTimeAsync(ConsultationTimePostModel postModel, Guid consultationDayId);
        Task<ResponseModel> DeleteConsultationTimeAsync(Guid consultationTimeId);
    }
}
