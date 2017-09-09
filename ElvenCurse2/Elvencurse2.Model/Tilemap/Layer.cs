using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Elvencurse2.Model.Tilemap
{
    [Serializable]
    public class Layer
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("width")]
        public int Width { get; set; }
        [XmlAttribute("height")]
        public int Height { get; set; }

        [XmlElement("data")]
        public string Rawdata { get; set; }

        public int[] Data
        {
            get
            {
                if (_data == null)
                {
                    // parse raw data..
                    var list = new List<int>();
                    foreach (var point in Rawdata.Trim().Split(','))
                    {
                        list.Add(int.Parse(point));
                    }
                    _data = list.ToArray();
                }
                return _data;
            }
            set { _data = value; }
        }

        private int[] _data;

        [XmlElement("properties")]
        public PropertyCollection Properties { get; set; }
    }
}
