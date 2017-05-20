using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Elvencurse2.Engine.Factories;
using Elvencurse2.Model;
using Elvencurse2.Model.Creatures;
using Elvencurse2.Model.Creatures.Npcs;
using Microsoft.Xna.Framework;

namespace Elvencurse2.Engine.Services
{
    public class Worldservice
    {
        public Worldservice()
        {
            
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
                    npc = new HunterNpc(null);
                    break;

                case Creaturetype.Wolf:
                    npc = new Wolf(null);
                    break;

                case Creaturetype.Bunny:
                    npc = new Bunny(null);
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
                //var equipment = _itemsService.ReloadCharacterEquipment(Newtonsoft.Json.JsonConvert.DeserializeObject<CharacterEquipment>((string)dr["Equipment"]));
                //npc.Equipment = equipment;
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
    }
}
