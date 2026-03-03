using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBV.Shared.Events
{
    public interface IUserDeleteEvent
    {
        string UserId { get; }
    }
}
