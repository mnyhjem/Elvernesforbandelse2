using System;
using System.Xml.Serialization;

namespace Elvencurse2.Model.Tilemap
{
    [Serializable]
    public class PropertyCollection
    {
        [XmlElement("property")]
        public Property[] Properties { get; set; }
    }
}
