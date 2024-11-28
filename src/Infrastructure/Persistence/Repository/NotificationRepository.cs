using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.Notification;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly VgaDbContext _context;
        public NotificationRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Notification> CreateNotification(NotificationPostModel model)
        {
            Notification notification = new Notification
            {
                AccountId = model.AccountId,
                Title = model.Title,
                Message = model.Message,
                CreatedAt = DateTime.UtcNow.AddHours(7),
                Status = Domain.Enum.NotiStatus.Unread
            };

            await AddAsync(notification);
            await _context.SaveChangesAsync();
            return notification; 
        }
    }
}
