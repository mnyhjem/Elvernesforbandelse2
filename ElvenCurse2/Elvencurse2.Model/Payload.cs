using Elvencurse2.Model.Creatures.Npcs;
using Elvencurse2.Model.Enums;
using Microsoft.Xna.Framework;

namespace Elvencurse2.Model
{
    public class Payload
    {
        public Payload()
        {
            
        }

        public Payload(dynamic dynpayload)
        {
            Type = (Payloadtype) dynpayload.Type;
            //Gameobject = (Gameobject) dynpayload.Gameobject;
            Gameobject = new Creature(null);
            ((Creature)Gameobject).Appearance = Newtonsoft.Json.JsonConvert.DeserializeObject<Creatures.CharacterAppearance>(dynpayload.Gameobject.Appearance.ToString());
            ((Creature) Gameobject).Equipment = Newtonsoft.Json.JsonConvert.DeserializeObject<Creatures.CharacterEquipment>(dynpayload.Gameobject.Equipment.ToString());

            ((Creature) Gameobject).Race = (CreatureRace) dynpayload.Gameobject.Race;
            ((Creature)Gameobject).Level = (int)dynpayload.Gameobject.Level;

            Gameobject.Position = new Vector2((float)dynpayload.Gameobject.Position.X, (float)dynpayload.Gameobject.Position.Y);
            Gameobject.ConnectionId = (string)dynpayload.Gameobject.ConnectionId;

            Receiver = (string) dynpayload.Receiver;
            Animation = (string) dynpayload.Animation;
        }

        public Payloadtype Type { get; set; }
        public Gameobject Gameobject { get; set; }
        public string Receiver { get; set; }
        public string Animation { get; set; }
    }
}
