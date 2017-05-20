using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Elvencurse2.Engine.Services;
using Elvencurse2.Model;
using Elvencurse2.Model.Engine;
using Elvencurse2.Model.Enums;
using Microsoft.AspNet.SignalR;
using Microsoft.Xna.Framework;
using GameTime = Elvencurse2.Model.Utilities.GameTime;

namespace Elvencurse2.Engine
{
    public class ElvenGame:IElvenGame
    {
        public ConcurrentQueue<Payload> GameChanges { get; set; }

        public List<Gameobject> Gameobjects { get; set; }
        public Hub CurrentHub { get; set; }

        private HighFrequencyTimer _gameLoop;

        private const long DrawFPS = 1000 / 40;
        long _actualFPS = 0;
        private object _locker = new object();
        private int DRAW_AFTER = 40 / 20;
        private long _drawCount = 0;
        private Model.Utilities.GameTime _gameTime;

        private Worldservice _worldservice;
        private Characterservice _characterservice;

        public ElvenGame()
        {
            Trace.WriteLine($"Server bootup at {DateTime.Now}");

            _worldservice = new Worldservice();
            _characterservice = new Characterservice(this);


            GameChanges = new ConcurrentQueue<Payload>();
            Gameobjects = new List<Gameobject>();

            _gameLoop = new HighFrequencyTimer(1000 / (float)20, id => Update(id), () => { }, () => { }, (fps) =>
            {
                _actualFPS = fps;
            });

            _gameTime = new Model.Utilities.GameTime();

            Gameobjects.AddRange(_worldservice.GetAllNpcs());

            _gameLoop.Start();
        }

        private long Update(long id)
        {
            lock (_locker)
            {
                DateTime utcNow = DateTime.UtcNow;

                try
                {
                    //if ((utcNow - _lastSpawn).TotalSeconds >= 1 && _spawned < AIShipsToSpawn)
                    //{
                    //    _spawned += SpawnsPerInterval;
                    //    SpawnAIShips(SpawnsPerInterval);
                    //    _lastSpawn = utcNow;
                    //}

                    _gameTime.Update(utcNow);

                    //GameHandler.Update(_gameTime);

                    //_space.Update();

                    UpdateGameobjects(_gameTime);

                    if (_actualFPS <= DrawFPS || (++_drawCount) % DRAW_AFTER == 0)
                    {
                        //Draw();
                        _drawCount = 0;
                        ProcessGamechanges();
                        //Trace.WriteLine(string.Format("Update {0}", DateTime.Now));
                    }
                }
                catch (Exception e)
                {
                    //ErrorLog.Instance.Log(e);
                }

                return id;
            }
        }

        private void UpdateGameobjects(GameTime gameTime)
        {
            foreach (var gameobject in Gameobjects)
            {
                gameobject.Update(gameTime);
            }
        }

        public bool EnterWorld(string userId, string contextConnectionId)
        {
            //var spiller = new Player(this)
            //{
            //    ConnectionId = contextConnectionId,
            //    Position = new Vector2
            //    {
            //        X = 200,
            //        Y = 200
            //    }
            //};
            var spiller = _characterservice.GetOnlineCharacterForUser(userId);
            if (spiller == null)
            {
                return false;
            }
            spiller.ConnectionId = contextConnectionId;
            Gameobjects.Add(spiller);

            //CurrentHub.Clients.All.Pong(DateTime.Now);//test

            GameChanges.Enqueue(new Payload { Gameobject = spiller, Type = Payloadtype.AddPlayer });
            foreach (var o in Gameobjects.Where(a => a.ConnectionId != contextConnectionId))
            {
                GameChanges.Enqueue(new Payload
                {
                    Gameobject = o,
                    Receiver = contextConnectionId,
                    Type = Payloadtype.AddPlayer
                });
            }
            return true;
        }

        private long cnt = 1;
        private void ProcessGamechanges()
        {
            while (GameChanges.Count > 0)
            {
                Payload o = null;
                if (!GameChanges.TryDequeue(out o))
                {
                    break;
                }

                Trace.WriteLine(string.Format("Send packet {0} [{1} X{2},Y{3}]", cnt++, o.Type, o.Gameobject.Position.X, o.Gameobject.Position.Y));

                if (string.IsNullOrEmpty(o.Receiver))
                {
                    CurrentHub.Clients.All.PushPayload(o);
                }
                else
                {
                    CurrentHub.Clients.Client(o.Receiver).PushPayload(o);
                }
            }
        }

        public void MovePlayer(string contextConnectionId, Direction direction)
        {
            var p = Gameobjects.FirstOrDefault(a => a.ConnectionId == contextConnectionId) as Player;
            if (p == null)
            {
                return;
            }

            p.Direction = direction;
            p.IsMoving = true;
        }

        public void StopMovePlayer(string contextConnectionId)
        {
            var p = Gameobjects.FirstOrDefault(a => a.ConnectionId == contextConnectionId) as Player;
            if (p == null)
            {
                return;
            }
            p.IsMoving = false;
        }
    }
}
