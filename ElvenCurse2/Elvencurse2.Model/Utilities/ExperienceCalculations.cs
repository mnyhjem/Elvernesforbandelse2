using System.Collections.Generic;
using System.Linq;

namespace Elvencurse2.Model.Utilities
{
    public static class ExperienceCalculations
    {
        public static int LevelCap { get { return 20; } }

        private static List<XpLevel> XpLevels
        {
            get
            {
                if (_xpLevels == null)
                {
                    CreateXpLevels();
                }
                return _xpLevels;
            }
        }

        private static List<XpLevel> _xpLevels;

        public static int XpRequiredToAdvanceToNextLevel(int currentLevel)
        {
            var r = XpLevels.FirstOrDefault(a => a.Level == currentLevel);
            if (r == null)
            {
                return 0;
            }

            return r.XpToNextLevel;
        }

        public static int CurrentlevelFromAccumulatedXp(int xp)
        {
            var r = XpLevels.OrderByDescending(a => a.AccXpToThisLevel).FirstOrDefault(a => a.AccXpToThisLevel <= xp);
            if (r == null)
            {
                return 0;
            }

            return r.Level;
        }

        public static int XpEarnedOnCurrentLevel(int currentLevel, int xp)
        {
            var lvl = XpLevels.FirstOrDefault(a => a.Level == currentLevel);
            if (lvl == null)
            {
                return 0;
            }
            var r = lvl.AccXpToThisLevel;

            return xp - r;
        }

        private static void CreateXpLevels()
        {
            _xpLevels = new List<XpLevel>();
            _xpLevels.Add(new XpLevel { Level = 1, XpToNextLevel = 400, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 2, XpToNextLevel = 900, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 3, XpToNextLevel = 1400, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 4, XpToNextLevel = 2100, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 5, XpToNextLevel = 2800, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 6, XpToNextLevel = 3600, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 7, XpToNextLevel = 4500, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 8, XpToNextLevel = 5400, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 9, XpToNextLevel = 6500, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 10, XpToNextLevel = 7600, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });

            _xpLevels.Add(new XpLevel { Level = 11, XpToNextLevel = 8700, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 12, XpToNextLevel = 9800, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 13, XpToNextLevel = 11000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 14, XpToNextLevel = 12300, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 15, XpToNextLevel = 13600, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 16, XpToNextLevel = 15000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 17, XpToNextLevel = 16400, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 18, XpToNextLevel = 17800, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 19, XpToNextLevel = 19300, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 20, XpToNextLevel = 20800, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });

            _xpLevels.Add(new XpLevel { Level = 21, XpToNextLevel = 22400, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 22, XpToNextLevel = 24000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 23, XpToNextLevel = 25500, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 24, XpToNextLevel = 27200, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 25, XpToNextLevel = 28900, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 26, XpToNextLevel = 30500, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 27, XpToNextLevel = 32200, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 28, XpToNextLevel = 33900, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 29, XpToNextLevel = 36300, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 30, XpToNextLevel = 38800, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });

            _xpLevels.Add(new XpLevel { Level = 31, XpToNextLevel = 41600, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 32, XpToNextLevel = 44600, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 33, XpToNextLevel = 48000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 34, XpToNextLevel = 51400, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 35, XpToNextLevel = 55000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 36, XpToNextLevel = 58700, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 37, XpToNextLevel = 62400, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 38, XpToNextLevel = 66200, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 39, XpToNextLevel = 70200, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 40, XpToNextLevel = 74300, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });

            _xpLevels.Add(new XpLevel { Level = 41, XpToNextLevel = 78500, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 42, XpToNextLevel = 82800, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 43, XpToNextLevel = 87100, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 44, XpToNextLevel = 91600, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 45, XpToNextLevel = 96300, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 46, XpToNextLevel = 101000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 47, XpToNextLevel = 105800, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 48, XpToNextLevel = 110700, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 49, XpToNextLevel = 115700, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 50, XpToNextLevel = 120900, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });

            _xpLevels.Add(new XpLevel { Level = 51, XpToNextLevel = 126100, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 52, XpToNextLevel = 131500, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 53, XpToNextLevel = 137000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 54, XpToNextLevel = 142500, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 55, XpToNextLevel = 148200, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 56, XpToNextLevel = 154000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 57, XpToNextLevel = 159900, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 58, XpToNextLevel = 165800, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 59, XpToNextLevel = 172000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 60, XpToNextLevel = 290000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });

            _xpLevels.Add(new XpLevel { Level = 61, XpToNextLevel = 317000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 62, XpToNextLevel = 349000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 63, XpToNextLevel = 386000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 64, XpToNextLevel = 428000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 65, XpToNextLevel = 475000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 66, XpToNextLevel = 527000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 67, XpToNextLevel = 585000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 68, XpToNextLevel = 648000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 69, XpToNextLevel = 717000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 70, XpToNextLevel = 1523800, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });

            _xpLevels.Add(new XpLevel { Level = 71, XpToNextLevel = 1539000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 72, XpToNextLevel = 1555700, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 73, XpToNextLevel = 1571800, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 74, XpToNextLevel = 1587900, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 75, XpToNextLevel = 1604200, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 76, XpToNextLevel = 1620700, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 77, XpToNextLevel = 1637400, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 78, XpToNextLevel = 1653900, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 79, XpToNextLevel = 1670800, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 80, XpToNextLevel = 1686300, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });

            _xpLevels.Add(new XpLevel { Level = 81, XpToNextLevel = 2121500, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 82, XpToNextLevel = 2669000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 83, XpToNextLevel = 3469000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 84, XpToNextLevel = 4583000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 85, XpToNextLevel = 13000000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 86, XpToNextLevel = 15080000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 87, XpToNextLevel = 22600000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 88, XpToNextLevel = 27300000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 89, XpToNextLevel = 32800000, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
            _xpLevels.Add(new XpLevel { Level = 90, XpToNextLevel = 0, AccXpToThisLevel = _xpLevels.Sum(a => a.XpToNextLevel) });
        }

        private class XpLevel
        {
            public int Level { get; set; }
            public int XpToNextLevel { get; set; }
            public int AccXpToThisLevel { get; set; }
        }
    }
}
