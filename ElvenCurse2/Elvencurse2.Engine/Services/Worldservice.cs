using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using Elvencurse2.Engine.Factories;
using Elvencurse2.Engine.Utilities;
using Elvencurse2.Model;
using Elvencurse2.Model.Creatures;
using Elvencurse2.Model.Creatures.Npcs;
using Elvencurse2.Model.Engine;
using Elvencurse2.Model.Tilemap;
using Microsoft.Xna.Framework;

namespace Elvencurse2.Engine.Services
{
    public class Worldservice: IWorldservice
    {
        private readonly ItemsService _itemsService;
        public List<Worldsection> Worldsections { get; private set; }
        public Location PositionToLocation(Vector2 newPosition, Worldsection map)
        {
            var location = new Location {Name = map.Name, Zone = map.Id};
            location.X = (int)newPosition.X / map.Tilemap.Tilewidth;
            location.Y = (int)newPosition.Y / map.Tilemap.Tileheight;

            return location;
        }


        public Worldservice(ItemsService itemsService)
        {
            _itemsService = itemsService;
            LoadWorld();
        }
        
        public List<Gameobject> GetAllNpcs()
        {
            var list = new List<Gameobject>();

            using (var con = DbFactory.GetConnection())
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = @"
select 
npc.Id,
npc.Mode,
npc.Name,
npc.Race,
npc.Status,
npc.Type,
loc.DefaultWorldsectionId, 
loc.DefaultX, 
loc.DefaultY,
loc.CurrentWorldsectionId, 
loc.CurrentX, 
loc.CurrentY,
npc.Level,
npc.Basehealth,
npc.Appearance,
npc.Equipment
from 
Npcs npc 
left outer join NpcLocations loc on npc.Id = loc.NpcId
";
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Gameobject npc = MapNpc(dr);

                            list.Add(npc);
                        }
                    }
                }
            }
            return list;
        }

        private Gameobject MapNpc(IDataReader dr)
        {
            Creature npc;

            switch ((Creaturetype)dr["type"])
            {
                case Creaturetype.Hunter:
                    npc = new HunterNpc(null, this);
                    break;

                case Creaturetype.Wolf:
                    npc = new Wolf(null, this);
                    break;

                case Creaturetype.Bunny:
                    npc = new Bunny(null, this);
                    break;

                default:
                    return null;
            }

            // standard items
            npc.Id = (int)dr["id"];
            npc.Name = (string)dr["name"];
            npc.Race = (CreatureRace)dr["race"];
            npc.Level = (int)dr["Level"];
            //npc.Status = (Creaturestatus) dr["status"];
            npc.Basehealth = (int)dr["BaseHealth"];
            npc.Location = new Location
            {
                Zone = (int)dr["CurrentWorldsectionId"],
                X = (int)dr["CurrentX"],
                Y = (int)dr["CurrentY"]
            };
            npc.DefaultLocation = new Location
            {
                Zone = (int)dr["DefaultWorldsectionId"],
                X = (int)dr["DefaultX"],
                Y = (int)dr["DefaultY"]
            };

            npc.Position = new Vector2(npc.Location.X * 32, npc.Location.Y * 32);

            npc.Appearance = Newtonsoft.Json.JsonConvert.DeserializeObject<CharacterAppearance>((string)dr["appearance"]);

            if (dr["Equipment"] == DBNull.Value)
            {
                npc.Equipment = GetDefaultEquipment(npc);
            }
            else
            {
                var equipment = _itemsService.ReloadCharacterEquipment(Newtonsoft.Json.JsonConvert.DeserializeObject<CharacterEquipment>((string)dr["Equipment"]));
                npc.Equipment = equipment;
            }

            return npc;
        }

        private static CharacterEquipment GetDefaultEquipment(Creature creature)
        {
            var e = new CharacterEquipment();
            if (creature.Appearance == null || creature.Appearance.Sex == Sex.Female)
            {
                e.Chest = new Item
                {
                    Category = Itemcategory.Wearable,
                    Type = 6,
                    Name = "Trist gammel kjole",
                    Description = "Denne kjole bør udskiftes hurtigst muligt",
                    Imagepath = "torso/dress_female/tightdress_black"
                };
            }
            else
            {
                e.Chest = new Item
                {
                    Category = Itemcategory.Wearable,
                    Type = 6,
                    Name = "Slidt lædervest",
                    Description = "Denne lædervest ville være bedre tjent med at fungere som taske",
                    Imagepath = "torso/leather/chest_male"
                };
            }
            return e;
        }

        private void LoadWorld()
        {
            Worldsections = new List<Worldsection>();
            using (var con = DbFactory.GetConnection())
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "select * from worldsections";
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Worldsections.Add(new Worldsection
                            {
                                Id = (int)dr["id"],
                                MapchangeDown = (int)dr["Mapchange_Down"],
                                MapchangeUp = (int)dr["Mapchange_Up"],
                                MapchangeRight = (int)dr["Mapchange_Right"],
                                MapchangeLeft = (int)dr["Mapchange_Left"],
                                Tilemap = ParseTilemap((string)dr["Json"]),
                                Name = (string)dr["Name"]
                            });
                        }
                    }
                }
            }
        }

        private Tilemap ParseTilemap(string mapdata)
        {
            try
            {
                var xRoot = new XmlRootAttribute();
                xRoot.ElementName = "map";

                var t = new XmlSerializer(typeof(Tilemap), xRoot);
                var map = t.Deserialize(mapdata.ToStream()) as Tilemap;
                return map;
            }
            catch (XmlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        private Tileset ParseTerrain(string data)
        {
            try
            {
                var xRoot = new XmlRootAttribute();
                xRoot.ElementName = "tileset";

                var t = new XmlSerializer(typeof(Tileset), xRoot);
                var terrain = t.Deserialize(data.ToStream()) as Tileset;
                return terrain;
            }
            catch (XmlException)
            {
                return null;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
