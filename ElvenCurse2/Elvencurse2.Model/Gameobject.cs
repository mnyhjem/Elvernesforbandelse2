using Elvencurse2.Model.Engine;
using Microsoft.Xna.Framework;

namespace Elvencurse2.Model
{
    public class Gameobject
    {
        protected IElvenGame ElvenGame;

        public virtual Vector2 Position { get; set; }
        public string ConnectionId { get; set; }

        public Gameobject(IElvenGame elvenGame)
        {
            ElvenGame = elvenGame;
        }

        public virtual void Update(Utilities.GameTime gameTime)
        {

        }
    }
}
