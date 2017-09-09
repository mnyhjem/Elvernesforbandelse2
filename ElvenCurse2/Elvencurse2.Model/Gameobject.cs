using Elvencurse2.Model.Engine;
using Microsoft.Xna.Framework;

namespace Elvencurse2.Model
{
    public class Gameobject
    {
        protected IElvenGame ElvenGame;
        protected readonly IWorldservice _worldservice;

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual Vector2 Position { get; set; }

        public Location Location { get; set; }

        public string ConnectionId { get; set; }

        public Gameobject(IElvenGame elvenGame, IWorldservice worldservice)
        {
            ElvenGame = elvenGame;
            _worldservice = worldservice;
        }

        public virtual Payload Update(Utilities.GameTime gameTime)
        {
            return null;
        }
    }
}
