using System;
using System.Configuration;
using System.Data;
using Elvencurse2.Engine.Factories;
using Elvencurse2.Model;
using Elvencurse2.Model.Creatures;
using Elvencurse2.Model.Engine;
using Microsoft.Xna.Framework;
using MySql.Data.MySqlClient;

namespace Elvencurse2.Engine.Services
{
    public class Characterservice
    {
        private readonly ElvenGame _elvenGame;
        private readonly ItemsService _itemsService;
        private readonly IWorldservice _worldservice;

        public Characterservice(ElvenGame elvenGame, ItemsService itemsService, IWorldservice worldservice)
        {
            _elvenGame = elvenGame;
            _itemsService = itemsService;
            _worldservice = worldservice;
        }

        public Gameobject GetOnlineCharacterForUser(string userId)
        {
            using (var con = DbFactory.GetConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = @"
select 
c.Id, 
c.Name, 
WorldsectionId, 
X, 
Y,
ws.Name as worldsectionname,
c.Experience as AccumulatedExperience,
c.BaseHealth,
c.IsAlive,
c.Appearance,
c.Type,
c.Equipment
from 
Characters c 
left outer join Characterlocations loc on c.Id = loc.characterId 
left outer join Worldsections ws on ws.Id = loc.WorldsectionId
where 
c.UserId = @userId and 
c.IsOnline = 1
";
                    cmd.Parameters.Add(new MySqlParameter("userId", userId));
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var character = MapCharacter(dr);

                            return character;
                        }
                    }
                }
            }
            return null;
        }

        private Player MapCharacter(IDataRecord dr)
        {
            var character = new Player(_elvenGame, _worldservice)
            {
                Id = (int)dr["id"],
                Name = (string)dr["name"],
                AccumulatedExperience = (int)dr["AccumulatedExperience"],
                Basehealth = (int)dr["BaseHealth"]
            };



            if (!(bool)dr["IsAlive"])
            {
                character.SetHealth(0);
            }

            if (dr["Worldsectionid"] == DBNull.Value)
            {
                character.Location = GetDefaultLocation(character);
            }
            else
            {
                character.Location = new Location
                {
                    Zone = (int)dr["worldsectionid"],
                    X = (int)dr["x"],
                    Y = (int)dr["y"],
                    Name = (string)dr["worldsectionname"]
                };
            }

            character.Position = new Vector2(character.Location.X * 32, character.Location.Y * 32);

            character.Appearance = Newtonsoft.Json.JsonConvert.DeserializeObject<CharacterAppearance>((string)dr["appearance"]);

            if (dr["Equipment"] == DBNull.Value)
            {
                character.Equipment = GetDefaultEquipment(character);
            }
            else
            {
                var equipment = _itemsService.ReloadCharacterEquipment(Newtonsoft.Json.JsonConvert.DeserializeObject<CharacterEquipment>((string)dr["Equipment"]));
                character.Equipment = equipment;
            }

            return character;
        }

        private static CharacterEquipment GetDefaultEquipment(Player character)
        {
            var e = new CharacterEquipment();
            if (character.Appearance.Sex == Sex.Female)
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

        private static Location GetDefaultLocation(Player character)
        {
            return new Location
            {
                X = 80,
                Y = 30,
                Zone = 3,
                Name = "Igtegator"
            };
        }
    }
}
