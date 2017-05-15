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
            Gameobject = new Gameobject(null);
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
