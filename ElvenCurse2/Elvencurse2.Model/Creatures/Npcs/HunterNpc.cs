﻿using Elvencurse2.Model.Engine;

namespace Elvencurse2.Model.Creatures.Npcs
{
    public class HunterNpc : Creature
    {
        public HunterNpc(IElvenGame elvenGame, IWorldservice worldservice) : base(elvenGame, worldservice, Creaturetype.Hunter)
        {
        }
    }
}
