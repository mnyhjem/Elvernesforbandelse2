using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Elvencurse2.Model.Engine
{
    public interface IWorldservice
    {
        List<Worldsection> Worldsections { get; }
        Location PositionToLocation(Vector2 newPosition, Worldsection map);

        /// <summary>
        /// Tjekker om den nye position kan betrædes i verdenen
        /// </summary>
        /// <param name="newPosition"></param>
        /// <param name="playerLocation"></param>
        /// <returns>Location objekt hvis positionen er ok, ellers null</returns>
        Location ValidatePosition(Vector2 newPosition, Location playerLocation);


        Point GetRandomPoint(Location origin, int maxDistanceFromOrigin);
    }
}
