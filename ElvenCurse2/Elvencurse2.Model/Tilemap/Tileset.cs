using System;
using System.Xml.Serialization;

namespace Elvencurse2.Model.Tilemap
{
    [Serializable]
    public class Tileset
    {
        [XmlAttribute("firstgid")]
        public int FirstGid { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("tilewidth")]
        public int Tilewidth { get; set; }
        [XmlAttribute("tileheight")]
        public int Tileheight { get; set; }
        [XmlAttribute("tilecount")]
        public int Tilecount { get; set; }
        [XmlAttribute("columns")]
        public int Columns { get; set; }

        [XmlElement("image")]
        public Image Image { get; set; }
        [XmlAttribute("source")]
        public string Source { get; set; }

        [XmlElement("terraintypes")]
        public Terraintypes Terraintypes { get; set; }

        [XmlElement("tile")]
        public Terraintile[] Terraintiles { get; set; }

        public bool IsTerrainreference
        {
            get { return Source?.EndsWith(".tsx") ?? false; }
        }
    }
}
