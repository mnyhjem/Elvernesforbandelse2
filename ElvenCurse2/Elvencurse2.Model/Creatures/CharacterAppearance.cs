namespace Elvencurse2.Model.Creatures
{
    public class CharacterAppearance
    {
        public Sex Sex { get; set; }
        public Body Body { get; set; }
        public Eyecolor Eyecolor { get; set; }
        public Nose Nose { get; set; }
        public Ears Ears { get; set; }

        public Facial Facial { get; set; }
        public Hair Hair { get; set; }

        public CharacterAppearance()
        {
            Facial = new Facial();
            Hair = new Hair();
        }
    }

    public class Hair
    {
        public HairType Type { get; set; }
        public HairColor Color { get; set; }

        public enum HairType
        {
            None = 0,
            Bangs = 1,
            Bangslong = 2,
            Bangslong2 = 3,
            Bangsshort = 4,
            Bedhead = 5,
            Bunches = 6,
            Jewfro = 7,
            Long = 8,
            Longhawk = 9,
            Longknot = 10,
            Loose = 11,
            Messy1 = 12,
            Messy2 = 13,
            Mohawk = 14,
            Page = 15,
            Page2 = 16,
            Parted = 17,
            Pixie = 18,
            Plain = 19,
            Ponytail = 20,
            Ponytail2 = 21,
            Princess = 22,
            ShortHawk = 23,
            ShortKnot = 24,
            Shoulderl = 25,
            Shoulderr = 26,
            Swoop = 27,
            Unkempt = 28,
            Xlong = 29,
            Xlongknot = 30
        }
    }

    public class Facial
    {
        public FacialType Type { get; set; }
        public HairColor Color { get; set; }

        public enum FacialType
        {
            None = 0,
            Beard = 1,
            Bigstache = 2,
            Fiveoclock = 3,
            Frenchstache = 4,
            Mustache = 5
        }
    }

    public enum HairColor
    {
        Black = 0,
        Blonde = 1,
        Blonde2 = 2,
        Blue = 3,
        Blue2 = 4,
        Brown = 5,
        Brown2 = 6,
        Brunette = 7,
        Brunette2 = 8,
        Dark_blonde = 9,
        Gold = 10,
        Gray = 11,
        Gray2 = 12,
        Green = 13,
        Green2 = 14,
        Light_blonde = 15,
        Light_blonde2 = 16,
        Pink = 17,
        Pink2 = 18,
        Purple = 19,
        Raven = 20,
        Raven2 = 21,
        Redhead = 22,
        Redhead2 = 23,
        Ruby_red = 24,
        White = 25,
        White_blonde = 26,
        White_blonde2 = 27,
        White_Cyan = 28
    }

    public enum Ears
    {
        Default = 0,
        Bigears = 1,
        Elvenears = 2
    }

    public enum Nose
    {
        Default = 0,
        Bignose = 1,
        Buttonnose = 2,
        Straightnose = 3
    }

    public enum Eyecolor
    {
        Blue = 0,
        Brown = 1,
        Gray = 2,
        Green = 3,
        Orange = 4,
        Purple = 5,
        Red = 6,
        Yellow = 7
    }

    public enum Sex
    {
        Male = 0,
        Female = 1
    }

    public enum Body
    {
        Dark = 0,
        Dark2 = 1,
        Darkelf = 2,
        Darkelf2 = 3,
        Light = 4,
        Orc = 5,
        Red_orc = 6,
        Tanned = 7,
        tanned2 = 8
    }
}
