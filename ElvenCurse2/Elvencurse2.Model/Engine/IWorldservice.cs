using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Elvencurse2.Model.Engine
{
    public interface IWorldservice
    {
        List<Worldsection> Worldsections { get; }
        Location PositionToLocation(Vector2 newPosition, Worldsection map);
    }
}
