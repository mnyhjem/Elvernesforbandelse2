using Elvencurse2.Model;
using Elvencurse2.Model.Creatures;
using ElvenCurse2.Client.Model;

namespace Elvencurse2.Client.Model
{
    public static class EntityHelper
    {
        public static Creatureentity CreateEntity(ElvenCurse2 game, Payload payload)
        {
            Creatureentity e = null;

            switch (payload.Gameobject.Type)
            {
                case Creaturetype.Wolf:
                    e = new Wolf(game, payload);
                    break;

                case Creaturetype.Bunny:
                    e = new Bunny(game, payload);
                    break;

                default:
                    e = new Creatureentity(game, payload);
                    e.CreateSprite();
                    break;
            }

            e.ConnectionId = payload.Gameobject.ConnectionId;
            e.Uuid = payload.Gameobject.Uuid;

            return e;
        }
    }
}
