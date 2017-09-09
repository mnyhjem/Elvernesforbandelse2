using Elvencurse2.Model.Engine;

namespace Elvencurse2.Model
{
    public class Player : Creature
    {
        

        public Player(IElvenGame elvenGame, IWorldservice worldservice) : base(elvenGame, worldservice)
        {
        }

        public int AccumulatedExperience { get; set; }

        
    }
}
