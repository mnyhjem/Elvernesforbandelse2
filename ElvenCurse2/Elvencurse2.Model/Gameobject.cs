using System;
using Elvencurse2.Model.Creatures;
using Elvencurse2.Model.Engine;
using Microsoft.Xna.Framework;

namespace Elvencurse2.Model
{
    public class Gameobject
    {
        protected IElvenGame ElvenGame;
        protected readonly IWorldservice _worldservice;

        public Creaturetype Type { get; private set; }

        public int Id { get; set; }

        public Guid Uuid { get; set; }

        public string Name { get; set; }

        public virtual Vector2 Position { get; set; }

        public Location Location { get; set; }

        public string ConnectionId { get; set; }

        protected Gameobject(IElvenGame elvenGame, IWorldservice worldservice, Creaturetype type)
        {
            ElvenGame = elvenGame;
            _worldservice = worldservice;
            Type = type;
            Uuid = Guid.NewGuid();
        }

        public virtual Payload Update(Utilities.GameTime gameTime)
        {
            return null;
        }
    }
}
