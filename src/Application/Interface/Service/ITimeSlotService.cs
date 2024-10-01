using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Response;
using Domain.Model.TimeSlot;

namespace Application.Interface.Service
{
    public interface ITimeSlotService
    {
        Task<ResponseModel> GetTimeSlotByIdAsync(int timeSlotId);
        Task<ResponseModel> CreateTimeSlotAsync(TimeSlotPostModel postModel);
        Task<ResponseModel> UpdateTimeSlotAsync(int timeSlotId, TimeSlotPutModel putModel);
        Task<ResponseModel> DeleteTimeSlotAsync(int timeSlotId);
    }
}
