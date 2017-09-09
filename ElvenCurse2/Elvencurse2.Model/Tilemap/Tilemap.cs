using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Elvencurse2.Model.Tilemap
{
    [Serializable]
    [XmlRoot("map")]
    public class Tilemap
    {
        [XmlAttribute("version")]
        public string Version { get; set; }
        [XmlAttribute("orientation")]
        public string Orientation { get; set; }
        [XmlAttribute("renderorder")]
        public string Renderorder { get; set; }
        [XmlAttribute("width")]
        public int Width { get; set; }
        [XmlAttribute("height")]
        public int Height { get; set; }
        [XmlAttribute("tilewidth")]
        public int Tilewidth { get; set; }
        [XmlAttribute("tileheight")]
        public int Tileheight { get; set; }
        [XmlAttribute("nextobjectid")]
        public int NextObjectid { get; set; }

        [XmlElement("tileset")]
        public Tileset[] Tilesets { get; set; }

        [XmlElement("layer")]
        public Layer[] Layers { get; set; }

        [XmlElement("properties")]
        public PropertyCollection Properties { get; set; }

        public bool HasTerrainreferences
        {
            get { return Tilesets.Any(a => a.IsTerrainreference); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="terrains">Hvis null, laver vi filen rigtigt. Hvis ikke null, prøver vi at finde det rigtige terrain, og indsætter værdierne for dette terrain istedet for at linke til filen</param>
        /// <returns></returns>
        public string GetJson(List<Terrainfile> terrains = null)
        {
            var sb = new StringBuilder();

            sb.AppendFormat(@"{{ ""height"":{0}," + Environment.NewLine, Height);
            if (Layers.Length > 0)
            {
                sb.AppendFormat(@"""layers"":[" + Environment.NewLine);
                for (int index = 0; index < Layers.Length; index++)
                {
                    var layer = Layers[index];
                    sb.AppendFormat(@"{{" + Environment.NewLine);

                    sb.AppendFormat(@"""data"":[{0}]," + Environment.NewLine, string.Join(", ", layer.Data));
                    sb.AppendFormat(@"""height"":{0}," + Environment.NewLine, layer.Height);
                    sb.AppendFormat(@"""name"":""{0}""," + Environment.NewLine, layer.Name);
                    sb.AppendFormat(@"""opacity"":1," + Environment.NewLine);

                    sb.Append(GetPropertiesSection(layer.Properties));

                    sb.AppendFormat(@"""type"":""tilelayer""," + Environment.NewLine);
                    sb.AppendFormat(@"""visible"":true," + Environment.NewLine);
                    sb.AppendFormat(@"""width"":{0}," + Environment.NewLine, layer.Width);
                    sb.AppendFormat(@"""x"":0," + Environment.NewLine);
                    sb.AppendFormat(@"""y"":0" + Environment.NewLine);

                    sb.AppendFormat(@"}}");
                    if (index < Layers.Length - 1)
                    {
                        sb.AppendFormat(@",");
                        sb.AppendFormat(Environment.NewLine);
                    }
                }
                sb.AppendFormat(@"]," + Environment.NewLine);
            }

            sb.AppendFormat(@"""nextobjectid"":{0}," + Environment.NewLine, NextObjectid);
            sb.AppendFormat(@"""orientation"":""{0}""," + Environment.NewLine, Orientation);

            sb.Append(GetPropertiesSection(Properties));

            sb.AppendFormat(@"""renderorder"":""{0}""," + Environment.NewLine, Renderorder);
            sb.AppendFormat(@"""tileheight"":{0}," + Environment.NewLine, Tileheight);

            if (Tilesets.Length > 0)
            {
                sb.AppendFormat(@"""tilesets"":[" + Environment.NewLine);
                for (var index = 0; index < Tilesets.Length; index++)
                {
                    var tileset = Tilesets[index];
                    sb.AppendFormat(@"{{" + Environment.NewLine);

                    if (tileset.IsTerrainreference)
                    {
                        Terrainfile foundTerrainfile = null;
                        if (terrains != null)
                        {
                            foundTerrainfile = terrains.FirstOrDefault(a => a.Filename == tileset.Source);
                        }

                        sb.AppendFormat(@"""firstgid"":{0}," + Environment.NewLine, tileset.FirstGid);
                        if (foundTerrainfile == null)
                        {
                            sb.AppendFormat(@"""source"":""{0}""", tileset.Source);
                        }
                        else
                        {
                            var tilecount = (foundTerrainfile.Tileset.Image.Width/foundTerrainfile.Tileset.Tilewidth)* (foundTerrainfile.Tileset.Image.Height / foundTerrainfile.Tileset.Tileheight);

                            sb.AppendFormat(@"""columns"":{0}," + Environment.NewLine, foundTerrainfile.Tileset.Columns);
                            sb.AppendFormat(@"""image"":""{0}""," + Environment.NewLine, foundTerrainfile.Tileset.Image.Source);
                            sb.AppendFormat(@"""imageheight"":{0}," + Environment.NewLine, foundTerrainfile.Tileset.Image.Height);
                            sb.AppendFormat(@"""imagewidth"":{0}," + Environment.NewLine, foundTerrainfile.Tileset.Image.Width);
                            sb.AppendFormat(@"""margin"":0," + Environment.NewLine);
                            sb.AppendFormat(@"""name"":""{0}""," + Environment.NewLine, foundTerrainfile.Tileset.Name);
                            sb.AppendFormat(@"""spacing"":0," + Environment.NewLine);
                            sb.AppendFormat(@"""tilecount"":{0}," + Environment.NewLine, tilecount);
                            sb.AppendFormat(@"""tileheight"":{0}," + Environment.NewLine, foundTerrainfile.Tileset.Tileheight);
                            sb.AppendFormat(@"""tilewidth"":{0}", foundTerrainfile.Tileset.Tilewidth);
                        }
                        
                    }
                    else
                    {
                        sb.AppendFormat(@"""columns"":{0}," + Environment.NewLine, tileset.Columns);
                        sb.AppendFormat(@"""firstgid"":{0}," + Environment.NewLine, tileset.FirstGid);
                        sb.AppendFormat(@"""image"":""{0}""," + Environment.NewLine, tileset.Image.Source);
                        sb.AppendFormat(@"""imageheight"":{0}," + Environment.NewLine, tileset.Image.Height);
                        sb.AppendFormat(@"""imagewidth"":{0}," + Environment.NewLine, tileset.Image.Width);
                        sb.AppendFormat(@"""margin"":0," + Environment.NewLine);
                        sb.AppendFormat(@"""name"":""{0}""," + Environment.NewLine, tileset.Name);
                        sb.AppendFormat(@"""spacing"":0," + Environment.NewLine);

                        if (tileset.Terraintypes != null)
                        {
                            sb.AppendFormat(@"""terrains"":[" + Environment.NewLine);
                            for (int i = 0; i < tileset.Terraintypes.Terrains.Length; i++)
                            {
                                var terrain = tileset.Terraintypes.Terrains[i];

                                sb.AppendFormat(@"{{" + Environment.NewLine);
                                sb.AppendFormat(@"""name"":""{0}""," + Environment.NewLine, terrain.Name);
                                sb.AppendFormat(@"""tile"":{0}" + Environment.NewLine, terrain.Tile);
                                sb.AppendFormat(@"}}");

                                if (i < tileset.Terraintypes.Terrains.Length - 1)
                                {
                                    sb.AppendFormat(@",");
                                    sb.AppendFormat(Environment.NewLine);
                                }
                            }
                            sb.AppendFormat(@"]," + Environment.NewLine);
                        }

                        sb.AppendFormat(@"""tilecount"":{0}," + Environment.NewLine, tileset.Tilecount);
                        sb.AppendFormat(@"""tileheight"":{0}," + Environment.NewLine, tileset.Tileheight);

                        if (tileset.Terraintiles != null)
                        {
                            sb.AppendFormat(@"""tiles"":" + Environment.NewLine);
                            sb.AppendFormat(@"{{" + Environment.NewLine);

                            var orderedTerraintiles = tileset.Terraintiles.OrderBy(a => a.SortId).ToArray();

                            for (int i = 0; i < tileset.Terraintiles.Length; i++)
                            {
                                var tile = orderedTerraintiles[i];
                                
                                sb.AppendFormat(@"""{0}"":" + Environment.NewLine, tile.Id);
                                sb.AppendFormat(@"{{" + Environment.NewLine);
                                sb.AppendFormat(@"""terrain"":[{0}]" + Environment.NewLine, tile.TerrainFormatted);
                                sb.AppendFormat(@"}}");

                                if (i < tileset.Terraintiles.Length - 1)
                                {
                                    sb.AppendFormat(@",");
                                    sb.AppendFormat(Environment.NewLine);
                                }
                            }
                            sb.AppendFormat(Environment.NewLine);
                            sb.AppendFormat(@"}}," + Environment.NewLine);
                        }

                        sb.AppendFormat(@"""tilewidth"":{0}", tileset.Tilewidth);
                        if (!string.IsNullOrEmpty(tileset.Image.Transparentcolor))
                        {
                            sb.AppendFormat("," + Environment.NewLine);
                            sb.AppendFormat(@"""transparentcolor"":""#{0}""", tileset.Image.Transparentcolor);
                        }
                    }

                    sb.AppendFormat(Environment.NewLine + @"}}");

                    if (index < Tilesets.Length - 1)
                    {
                        sb.AppendFormat(@",");
                        sb.AppendFormat(Environment.NewLine);
                    }
                }
                sb.AppendFormat(@"]," + Environment.NewLine);
            }

            sb.AppendFormat(@"""tilewidth"":{0}," + Environment.NewLine, Tilewidth);
            sb.AppendFormat(@"""version"":{0}," + Environment.NewLine, Version.Split('.')[0]);
            sb.AppendFormat(@"""width"":{0}" + Environment.NewLine, Width);
            sb.AppendFormat(@"}}" + Environment.NewLine);
            
            return sb.ToString();
        }

        private string GetPropertiesSection(PropertyCollection properties)
        {
            var sb = new StringBuilder();

            if (properties != null && properties.Properties.Length > 0)
            {
                sb.AppendFormat(@"""properties"":" + Environment.NewLine);
                sb.AppendFormat(@"{{" + Environment.NewLine);
                for (var i = 0; i < properties.Properties.Length; i++)
                {
                    var property = properties.Properties[i];
                    if (property.Type == "string")
                    {
                        sb.AppendFormat(@"""{0}"":""{1}""", property.Name, property.Value);
                    }
                    else
                    {
                        sb.AppendFormat(@"""{0}"":{1}", property.Name, property.Value);
                    }
                    
                    if (i < properties.Properties.Length - 1)
                    {
                        sb.AppendFormat(@",");
                    }
                    sb.AppendFormat(Environment.NewLine);
                }
                sb.AppendFormat(@"}}," + Environment.NewLine);

                sb.AppendFormat(@"""propertytypes"":" + Environment.NewLine);
                sb.AppendFormat(@"{{" + Environment.NewLine);
                for (var i = 0; i < properties.Properties.Length; i++)
                {
                    var property = properties.Properties[i];
                    sb.AppendFormat(@"""{0}"":""{1}""", property.Name, property.Type);
                    if (i < properties.Properties.Length - 1)
                    {
                        sb.AppendFormat(@",");
                    }
                    sb.AppendFormat(Environment.NewLine);
                }
                sb.AppendFormat(@"}}," + Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
