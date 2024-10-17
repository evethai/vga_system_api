using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Application.Common.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly UserConnectionManager _userConnectionManager;

        public NotificationHub(UserConnectionManager userConnectionManager)
        {
            _userConnectionManager = userConnectionManager;
        }

        public override Task OnConnectedAsync()
        {
            var claims = Context.User?.Claims;
            var nameClaim = claims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name) || c.Type.Equals(JwtRegisteredClaimNames.UniqueName));
            var accountId = nameClaim?.Value;

            if (accountId != null)
            {
                _userConnectionManager.AddConnection(accountId, Context.ConnectionId); 
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var claims = Context.User?.Claims;
            var nameClaim = claims?.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name) || c.Type.Equals(JwtRegisteredClaimNames.UniqueName));
            var accountId = nameClaim?.Value;

            if (accountId != null)
            {
                _userConnectionManager.RemoveConnection(accountId, Context.ConnectionId); 
            }

            return base.OnDisconnectedAsync(exception);
        }
    }

}
