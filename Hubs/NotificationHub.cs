using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace CBVSignalR.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            foreach (var claim in Context.User.Claims)
            {
                Console.WriteLine($"{claim.Type} = {claim.Value}");
            }
            Console.WriteLine($"UserId: {userId}");
            await base.OnConnectedAsync();
        }

        public async Task JoinGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }
    }
}
