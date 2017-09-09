using System;
using System.Xml.Serialization;

namespace Elvencurse2.Model.Tilemap
{
    [Serializable]
    public class Terrain
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("tile")]
        public string Tile { get; set; }
    }
}
