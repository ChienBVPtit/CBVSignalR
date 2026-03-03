using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBV.Shared.Events
{
    public interface IUserSyncEvent
    {
        string UserId { get; }
        string FullName { get; }
        string Username { get; }
        string? Email { get; }
        string? PhoneNumber { get; }
    }
}
