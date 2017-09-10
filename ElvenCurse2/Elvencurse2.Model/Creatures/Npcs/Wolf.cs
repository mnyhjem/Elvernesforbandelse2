using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Elvencurse2.Model.Engine;
using Elvencurse2.Model.Enums;
using Microsoft.Xna.Framework;
using GameTime = Elvencurse2.Model.Utilities.GameTime;

namespace Elvencurse2.Model.Creatures.Npcs
{
    public class Wolf : Creature
    {
        public Wolf(IElvenGame elvenGame, IWorldservice worldservice) : base(elvenGame, worldservice, Creaturetype.Wolf)
        {
            FoundPath = new Queue<Point>();
        }

        public override Payload Update(GameTime gameTime)
        {
            //if (Id != 7)
            //{
            //    return null;
            //}

            var map = _worldservice.Worldsections.FirstOrDefault(a => a.Id == Location.Zone);
            if (map == null)
            {
                return null;
            }

            if (InvalidMoveCounter > 10)
            {
                // Vi sidder fast et sted, så vi resetter..
                if (Id == 7)
                {
                    Trace.WriteLine(string.Format("{0} sidder fast.. Nulstiller", Name));
                }
                
                FoundPath.Clear();
            }

            if (FoundPath.Count == 0)
            {
                var collisionLayer = map.Tilemap.Layers.FirstOrDefault(a => a.Name.ToLower() == "collisionlayer" || a.Name.ToLower() == "collision" || a.Name.ToLower() == "blocking");
                if (collisionLayer != null)
                {
                    var pathfindingMap = new int[collisionLayer.Width, collisionLayer.Height];
                    for (int y = 0; y < collisionLayer.Height; y++)
                    {
                        for (int x = 0; x < collisionLayer.Width; x++)
                        {
                            var data = collisionLayer.Data[y * collisionLayer.Width + x];
                            pathfindingMap[x, y] = data > 0 ? 0 : 1;
                        }
                    }
                    
                    var start = new Point((int)Location.X, (int)Location.Y);
                    var end = _worldservice.GetRandomPoint(Location, 20);
                    if (InvalidMoveCounter > 10)
                    {
                        _headingTo = null;
                        end = new Point((int)DefaultLocation.X, (int)DefaultLocation.Y);
                    }
                    if (Id == 7)
                    {
                        Trace.WriteLine(string.Format("{0} New target {1},{2}", Name, end.X, end.Y));
                    }
                    var path = AI.Pathfinders.Pathfinder.Run(pathfindingMap, collisionLayer.Width, collisionLayer.Height, start, end);
                    if (path != null)
                    {
                        path.Reverse();
                        foreach (var p in path)
                        {
                            FoundPath.Enqueue(p);
                        }
                    }
                    else
                    {
                        Trace.WriteLine(string.Format("{0} No solution for target {1},{2}", Name, end.X, end.Y));
                    }
                }
                
            }

            if (FoundPath.Count > 0)
            {
                if (_headingTo == null || _headingTo.Value.X == (int)Location.X && _headingTo.Value.Y == (int)Location.Y)
                {
                    _headingTo = FoundPath.Dequeue();
                    if (_headingTo.Value.X == (int) Location.X && _headingTo.Value.Y == (int) Location.Y && FoundPath.Count > 0)
                    {
                        _headingTo = FoundPath.Dequeue();
                    }
                }
                
                var bearing = Utilities.MathLib.AngleInDeg(Location, _headingTo.Value);
                Direction = Utilities.MathLib.BearingToDirection(bearing);
            }

            IsMoving = AreWeMoving();

            if (InvalidMoveCounter > 25)
            {
                // Vi resetter helt... den sidder godt fast...
                InvalidMoveCounter = 0;
                Location = DefaultLocation;
                Position = new Vector2(Location.X * 32, Location.Y * 32);
                _headingTo = null;
                return new Payload
                {
                    Gameobject = this,
                    Type = Payloadtype.Move,
                    Animation = ""
                };
            }

            return base.Update(gameTime);
        }

        private bool AreWeMoving()
        {
            if (FoundPath.Count > 0)
            {
                return true;
            }

            if (_headingTo == null)
            {
                return false;
            }

            if (_headingTo.Value.X != (int) Location.X || _headingTo.Value.Y != (int) Location.Y)
            {
                return true;
            }

            return false;
        }
        
        public Queue<Point> FoundPath { get; set; }
        private Point? _headingTo;
    }
}
