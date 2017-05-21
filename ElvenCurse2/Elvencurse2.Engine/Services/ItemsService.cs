using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Elvencurse2.Engine.Factories;
using Elvencurse2.Model;
using Elvencurse2.Model.Creatures;
using MySql.Data.MySqlClient;

namespace Elvencurse2.Engine.Services
{
    public class ItemsService
    {
        private readonly string _connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public ItemsService()
        {
            
        }

        public int SaveItem(Item item)
        {
            if (string.IsNullOrWhiteSpace(item.Imagepath))
            {
                item.Imagepath = "";
            }
            if (string.IsNullOrWhiteSpace(item.Description))
            {
                item.Description = "";
            }

            using (var con = DbFactory.GetConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SaveItem";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("Id", item.Id));
                    cmd.Parameters.Add(new MySqlParameter("Category", item.Category));
                    cmd.Parameters.Add(new MySqlParameter("Type", item.Type));
                    cmd.Parameters.Add(new MySqlParameter("Name", item.Name));
                    cmd.Parameters.Add(new MySqlParameter("Description", item.Description));
                    cmd.Parameters.Add(new MySqlParameter("ImagePath", item.Imagepath));

                    var r = cmd.ExecuteScalar();
                    if (r == null)
                    {
                        return item.Id;
                    }
                    else
                    {
                        return (int)(decimal)r;
                    }
                }
            }
        }

        public List<Item> GetItems()
        {
            var items = new List<Item>();
            using (var con = DbFactory.GetConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id,Category,Type,Name,Description,Imagepath from Items order by Id";
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            items.Add(MapItem(dr));
                        }
                    }
                }
            }
            return items;
        }

        public Item GetItem(int id)
        {
            using (var con = DbFactory.GetConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id,Category,Type,Name,Description,Imagepath from Items where Id = @id";
                    cmd.Parameters.Add(new MySqlParameter("id", id));
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            return MapItem(dr);
                        }
                    }
                }
            }
            return null;
        }

        private Item MapItem(IDataRecord dr)
        {
            var item = new Item
            {
                Id = (int)dr["Id"],
                Category = (Itemcategory)dr["Category"],
                Type = (int)dr["Type"],
                Name = (string)dr["Name"],
                Description = (string)dr["description"],
                Imagepath = (string)dr["imagepath"]
            };

            return item;
        }

        public CharacterEquipment ReloadCharacterEquipment(CharacterEquipment equipment)
        {
            var items = new List<Item>();
            using (var con = DbFactory.GetConnection(_connectionstring))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id,Category,Type,Name,Description,Imagepath from Items where Id in (@ids)";
                    cmd.Parameters.Add(new MySqlParameter("ids", string.Join(",", items.Select(a => a.Id))));
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            items.Add(MapItem(dr));
                        }
                    }
                }
            }

            if (items.FirstOrDefault(a => a.Id == equipment.Neck.Id) != null)
            {
                equipment.Neck = items.FirstOrDefault(a => a.Id == equipment.Neck.Id);
            }
            if (items.FirstOrDefault(a => a.Id == equipment.Belt.Id) != null)
            {
                equipment.Belt = items.FirstOrDefault(a => a.Id == equipment.Belt.Id);
            }
            if (items.FirstOrDefault(a => a.Id == equipment.Feet.Id) != null)
            {
                equipment.Feet = items.FirstOrDefault(a => a.Id == equipment.Feet.Id);
            }
            if (items.FirstOrDefault(a => a.Id == equipment.Hands.Id) != null)
            {
                equipment.Hands = items.FirstOrDefault(a => a.Id == equipment.Hands.Id);
            }

            if (items.FirstOrDefault(a => a.Id == equipment.Arms.Id) != null)
            {
                equipment.Arms = items.FirstOrDefault(a => a.Id == equipment.Arms.Id);
            }

            if (items.FirstOrDefault(a => a.Id == equipment.Head.Id) != null)
            {
                equipment.Head = items.FirstOrDefault(a => a.Id == equipment.Head.Id);
            }
            if (items.FirstOrDefault(a => a.Id == equipment.Legs.Id) != null)
            {
                equipment.Legs = items.FirstOrDefault(a => a.Id == equipment.Legs.Id);
            }
            if (items.FirstOrDefault(a => a.Id == equipment.Chest.Id) != null)
            {
                equipment.Chest = items.FirstOrDefault(a => a.Id == equipment.Chest.Id);
            }
            if (items.FirstOrDefault(a => a.Id == equipment.Weapon.Id) != null)
            {
                equipment.Weapon = items.FirstOrDefault(a => a.Id == equipment.Weapon.Id);
            }
            if (items.FirstOrDefault(a => a.Id == equipment.Shoulders.Id) != null)
            {
                equipment.Shoulders = items.FirstOrDefault(a => a.Id == equipment.Shoulders.Id);
            }
            if (items.FirstOrDefault(a => a.Id == equipment.Bracers.Id) != null)
            {
                equipment.Bracers = items.FirstOrDefault(a => a.Id == equipment.Bracers.Id);
            }

            return equipment;
        }
    }
}
