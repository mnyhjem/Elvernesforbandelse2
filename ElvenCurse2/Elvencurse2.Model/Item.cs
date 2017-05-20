namespace Elvencurse2.Model
{
    public class Item
    {
        public int Id { get; set; }
        public Itemcategory Category { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Imagepath { get; set; }
    }

    public enum Itemcategory
    {
        Unknown = 0,
        Wearable = 1,
        Junk = 2
    }

    public enum WearableType
    {
        Neck = 0,
        Belt = 1,
        Feet = 2,
        Hands = 3,
        Head = 4,
        Legs = 5,
        Chest = 6,
        Weapon = 7,
        Arms = 8,
        Shoulders = 9,
        Bracers = 10
    }
}
