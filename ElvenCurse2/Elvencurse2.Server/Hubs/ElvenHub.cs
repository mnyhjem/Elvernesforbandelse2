using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Elvencurse2.Model.Enums;
using Microsoft.AspNet.SignalR;

namespace Elvencurse2.Server.Hubs
{
    //[Authorize]
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
            Program.Game.Gameobjects.Remove(p);// todo burde være dispose, og dispose burde så fjerne sig selv og sende "jeg er væk" ud til folk...

            return base.OnDisconnected(stopCalled);
        }

        public void EnterWorld()
        {
            Program.Game.EnterWorld(Context.ConnectionId);
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
