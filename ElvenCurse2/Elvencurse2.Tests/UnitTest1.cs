using System;
using Elvencurse2.Model;
using Elvencurse2.Model.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace Elvencurse2.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void DegreeBearing_Viser_Korrekt_Når_Det_Er_Syd()
        {
            var start = new Point {X = 37, Y = 25};
            var end = new Point(37, 29);
            var res = Model.Utilities.MathLib.AngleInDeg(start, end);

            Assert.AreEqual(180, res);
        }

        [TestMethod]
        public void BearingToDirection_Viser_Korrekt_Når_Det_Er_Syd()
        {
            var res = Model.Utilities.MathLib.BearingToDirection(180);
            Assert.AreEqual(Direction.South, res);
        }

        [TestMethod]
        public void DegreeBearing_Viser_Korrekt_Når_Det_Er_Nord()
        {
            var start = new Point { X = 37, Y = 29 };
            var end = new Point(37, 25);
            var res = Model.Utilities.MathLib.AngleInDeg(start, end);

            Assert.AreEqual(0, res);
        }

        [TestMethod]
        public void BearingToDirection_Viser_Korrekt_Når_Det_Er_Nord()
        {
            var res = Model.Utilities.MathLib.BearingToDirection(0);
            Assert.AreEqual(Direction.North, res);
        }

        [TestMethod]
        public void DegreeBearing_Viser_Korrekt_Når_Det_Er_Øst()
        {
            var start = new Point { X = 37, Y = 29 };
            var end = new Point(40, 29);
            var res = Model.Utilities.MathLib.AngleInDeg(start, end);

            Assert.AreEqual(90, res);
        }

        [TestMethod]
        public void BearingToDirection_Viser_Korrekt_Når_Det_Er_Øst()
        {
            var res = Model.Utilities.MathLib.BearingToDirection(90);
            Assert.AreEqual(Direction.East, res);
        }

        [TestMethod]
        public void DegreeBearing_Viser_Korrekt_Når_Det_Er_Vest()
        {
            
            var start = new Point { X = 37, Y = 29 };
            var end = new Point(30, 29);
            var res = Model.Utilities.MathLib.AngleInDeg(start, end);

            Assert.AreEqual(270, res);
        }

        [TestMethod]
        public void BearingToDirection_Viser_Korrekt_Når_Det_Er_Vest()
        {
            var res = Model.Utilities.MathLib.BearingToDirection(270);
            Assert.AreEqual(Direction.West, res);
        }

        [TestMethod]
        public void DegreeBearing_Viser_Korrekt_Når_Det_Er_NordVest()
        {

            var start = new Point { X = 83, Y = 85 };
            var end = new Point(82, 84);
            var res = Model.Utilities.MathLib.AngleInDeg(start, end);

            Assert.AreEqual(315, res);
        }

        [TestMethod]
        public void BearingToDirection_Viser_Korrekt_Når_Det_Er_NordVest()
        {
            var res = Model.Utilities.MathLib.BearingToDirection(315);
            Assert.AreEqual(Direction.NorthWest, res);
        }

        [TestMethod]
        public void DegreeBearing_Viser_Korrekt_Når_Det_Er_SydVest()
        {

            var start = new Point { X = 83, Y = 85 };
            var end = new Point(82, 86);
            var res = Model.Utilities.MathLib.AngleInDeg(start, end);

            Assert.AreEqual(225, res);
        }

        [TestMethod]
        public void BearingToDirection_Viser_Korrekt_Når_Det_Er_SydVest()
        {
            var res = Model.Utilities.MathLib.BearingToDirection(225);
            Assert.AreEqual(Direction.SouthWest, res);
        }

        [TestMethod]
        public void DegreeBearing_Viser_Korrekt_Når_Det_Er_SydØst()
        {

            var start = new Point { X = 83, Y = 85 };
            var end = new Point(84, 86);
            var res = Model.Utilities.MathLib.AngleInDeg(start, end);

            Assert.AreEqual(135, res);
        }

        [TestMethod]
        public void BearingToDirection_Viser_Korrekt_Når_Det_Er_SydØst()
        {
            var res = Model.Utilities.MathLib.BearingToDirection(135);
            Assert.AreEqual(Direction.SouthEast, res);
        }

        [TestMethod]
        public void DegreeBearing_Viser_Korrekt_Når_Det_Er_NordØst()
        {

            var start = new Point { X = 83, Y = 85 };
            var end = new Point(84, 84);
            var res = Model.Utilities.MathLib.AngleInDeg(start, end);

            Assert.AreEqual(45, res);
        }

        [TestMethod]
        public void BearingToDirection_Viser_Korrekt_Når_Det_Er_NordØst()
        {
            var res = Model.Utilities.MathLib.BearingToDirection(45);
            Assert.AreEqual(Direction.NorthEast, res);
        }
    }
}
