using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Enum;
using Domain.Model.Notification;

namespace Infrastructure.Persistence.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<NotificationModel>> GetNotiByAccountId(Guid accountId)
        {
            var notis = await _unitOfWork.NotificationRepository.GetListAsync(predicate: x=>x.AccountId == accountId, orderBy: x=>x.OrderBy(x=>x.Status));
            var result = _mapper.Map<IEnumerable<NotificationModel>>(notis);
            return result;
        }


    }
}
