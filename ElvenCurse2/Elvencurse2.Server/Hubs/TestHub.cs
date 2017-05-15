using System.Diagnostics;
using System.Security.Claims;
using Elvencurse2.Server.Utilities;
using Microsoft.AspNet.SignalR;

namespace Elvencurse2.Server.Hubs
{
    [Authorize]
    public class TestHub : Hub
    {
        public void Send(string message)
        {
            var userid = ((ClaimsPrincipal)Context.User).GetUserId();
            var username = ((ClaimsPrincipal)Context.User).GetUsername();

            Trace.WriteLine(username+" siger noget");

            Clients.All.addNewMessage(username + ": " + message);
        }
    }
}
