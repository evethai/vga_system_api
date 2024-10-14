using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.ConsultationDay;
using Domain.Model.Response;
using Domain.Model.TimeSlot;

namespace Application.Interface.Service
{
    public interface IConsultationDayService
    {
        Task<ResponseModel> GetConsultationDayByIdAsync(Guid id);
        Task<ResponseModel> CreateConsultationDayWithTimesAsync(ConsultationDayPostModel postModel);
        Task<ResponseModel> DeleteConsultationDayAsync(Guid consultationDayId);
        Task<ResponseConsultationDayModel> GetListConsultationDaysWithPaginateAsync(ConsultationDaySearchModel searchModel);
    }
}
