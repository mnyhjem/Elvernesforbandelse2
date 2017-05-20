using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Elvencurse2.Model.Enums;
using Elvencurse2.Server.Utilities;
using Microsoft.AspNet.SignalR;

namespace Elvencurse2.Server.Hubs
{
    [Authorize]
    public class ElvenHub : Hub
    {
        public ElvenHub()
        {
            Trace.WriteLine("Creating");

            //Program.Game.GameChanged += Game_GameChanged;

            Program.Game.CurrentHub = this;
        }

        public void Ping()
        {
            Clients.Caller.Pong(DateTime.Now);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var p = Program.Game.Gameobjects.FirstOrDefault(a => a.ConnectionId == Context.ConnectionId);
            if (p != null)
            {
                Trace.WriteLine($"Fjerner {p.Name} med id {p.ConnectionId}");
                Program.Game.Gameobjects.Remove(p);// todo burde være dispose, og dispose burde så fjerne sig selv og sende "jeg er væk" ud til folk...
            }
            
            return base.OnDisconnected(stopCalled);
        }

        public void EnterWorld()
        {
            var userid = ((ClaimsPrincipal)Context.User).GetUserId();
            var username = ((ClaimsPrincipal)Context.User).GetUsername();

            if (!Program.Game.EnterWorld(userid, Context.ConnectionId))
            {
                // disconnect brugeren..
                return;
            }
            Trace.WriteLine(string.Format("Ny spiller {0} Spillere i alt {1}", Context.ConnectionId, Program.Game.Gameobjects.Count));
        }

        public void MovePlayer(Direction direction)
        {
            Program.Game.MovePlayer(Context.ConnectionId, direction);
        }

        public void StopMovePlayer()
        {
            Program.Game.StopMovePlayer(Context.ConnectionId);
        }

        protected override void Dispose(bool disposing)
        {
            Trace.WriteLine("Disposing");
            base.Dispose(disposing);
        }
    }
}
