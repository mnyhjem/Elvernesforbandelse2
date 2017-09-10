using Elvencurse2.Model.Engine;
using Elvencurse2.Model.Utilities;

namespace Elvencurse2.Model
{
    public class Player : Creature
    {
        

        public Player(IElvenGame elvenGame, IWorldservice worldservice) : base(elvenGame, worldservice)
        {
        }

        public int AccumulatedExperience
        {
            get { return _accumulatedExperience; }
            set
            {
                _accumulatedExperience = value;
                Level = ExperienceCalculations.CurrentlevelFromAccumulatedXp(_accumulatedExperience);
            }
        }

        private int _accumulatedExperience;


    }
}
