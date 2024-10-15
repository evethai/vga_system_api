using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Notification;

namespace Application.Interface.Service
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationModel>> GetNotiByAccountId(Guid accountId);
    }
}
