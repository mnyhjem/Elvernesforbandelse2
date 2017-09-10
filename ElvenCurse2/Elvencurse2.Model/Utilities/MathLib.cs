using System;
using Elvencurse2.Model.Enums;
using Microsoft.Xna.Framework;

namespace Elvencurse2.Model.Utilities
{
    public static class MathLib
    {
        //This returns the angle in radians
        public static double AngleInRad(Point vec1, Point vec2)
        {
            return Math.Atan2(vec2.Y - (double)vec1.Y, (double)vec2.X - vec1.X);
        }

        //This returns the angle in degrees
        public static double AngleInDeg(Point vec1, Point vec2)
        {
            var res = AngleInRad(vec1, vec2) * 180 / Math.PI;
            res += 90;

            if (res < 0)
            {
                res += 360;
            }

            return res;
        }

        public static double AngleInDeg(Location vec1, Point vec2)
        {
            return AngleInDeg(new Point {X = (int) vec1.X, Y = (int) vec1.Y}, vec2);
        }

        public static Direction BearingToDirection(double bearing)
        {
            return (Direction) Math.Round(bearing / 45);
        }
    }
}
