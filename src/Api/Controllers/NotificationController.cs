using Api.Constants;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet(ApiEndPointConstant.Notification.NotificationEndpoint)]
        public async Task<IActionResult> GetNotiByAccountId(Guid id)
        {
            var result = await _notificationService.GetNotiByAccountId(id);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateNotification(int id, NotiStatus status)
        {
            var result = await _notificationService.UpdateNotification(id, status);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> AddNotification(NotificationPostModel model)
        {
            var result = await _notificationService.AddNotification(model);
            return Ok(result);
        }
    }
}
