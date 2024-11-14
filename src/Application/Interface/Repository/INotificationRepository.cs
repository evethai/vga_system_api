using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Notification;

namespace Application.Interface.Repository
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<Notification> CreateNotification(NotificationPostModel model);
    }
}
