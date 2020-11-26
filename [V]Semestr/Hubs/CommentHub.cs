using _V_Semestr.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _V_Semestr.Hubs
{
    public class CommentHub : Hub
    {
        public string GetConnectionId() =>
            Context.ConnectionId;

    }
}
