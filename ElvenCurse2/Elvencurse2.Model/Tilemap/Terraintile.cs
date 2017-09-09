using System;
using System.Xml.Serialization;

namespace Elvencurse2.Model.Tilemap
{
    [Serializable]
    public class Terraintile
    {
        public int SortId
        {
            get
            {
                var str = Id.ToString();
                if (str.Length == 1)
                {
                    str = str + "0";
                }
                return int.Parse(str);
            }
        }

        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("terrain")]
        public string Terrain { get; set; }

        public string TerrainFormatted
        {
            get
            {
                var result = "";
                var arr = Terrain.Split(',');
                for (int index = 0; index < arr.Length; index++)
                {
                    var s = arr[index];

                    var e = s;
                    if (string.IsNullOrEmpty(s))
                    {
                        e = "-1";
                    }
                    result += string.Format("{0}", e);

                    if (index < arr.Length - 1)
                    {
                        result += ", ";
                    }
                }

                return result;
            }
        }
    }
}
