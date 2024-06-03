using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Services.SignalRHubs
{
    [Authorize]
    public class ChatsHub: Hub
    {
        public async override Task OnConnectedAsync()
        {            
            var userId = Context.UserIdentifier ?? string.Empty;            
            if (!string.IsNullOrWhiteSpace(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }            
            await base.OnConnectedAsync();  
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = Context.UserIdentifier ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
