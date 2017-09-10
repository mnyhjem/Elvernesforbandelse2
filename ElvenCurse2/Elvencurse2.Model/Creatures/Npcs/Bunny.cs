using Elvencurse2.Model.Engine;
using Elvencurse2.Model.Enums;
using Elvencurse2.Model.Utilities;

namespace Elvencurse2.Model.Creatures.Npcs
{
    public class Bunny : Creature
    {
        public Bunny(IElvenGame elvenGame, IWorldservice worldservice) : base(elvenGame, worldservice, Creaturetype.Bunny)
        {
        }

        public override Payload Update(GameTime gameTime)
        {
            Direction = Direction.North;
            IsMoving = true;

            return base.Update(gameTime);
        }
    }
}
