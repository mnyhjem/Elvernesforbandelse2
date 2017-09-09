using System;
using System.Xml.Serialization;

namespace Elvencurse2.Model.Tilemap
{
    [Serializable]
    public class Terraintypes
    {
        [XmlElement("terrain")]
        public Terrain[] Terrains { get; set; }
    }
}
