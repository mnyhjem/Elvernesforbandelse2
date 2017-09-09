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
using GameTime = Elvencurse2.Model.Utilities.GameTime;

namespace Elvencurse2.Engine
{
    public class LongEventArgs : EventArgs
    {
        public long Value { get; }

        public LongEventArgs(long value)
        {
            Value = value;
        }
    }

    public class ElvenGame:IElvenGame
    {
        public event EventHandler<LongEventArgs> FpsUpdate;

        private void OnFpsUpdate()
        {
            FpsUpdate?.Invoke(this, new LongEventArgs(_actualFps));
        }

        public ConcurrentQueue<Payload> GameChanges { get; set; }

        public List<Gameobject> Gameobjects { get; set; }
        public Hub CurrentHub { get; set; }

        private HighFrequencyTimer _gameLoop;

        private const long DrawFps = 1000 / 40;
        private long _actualFps;
        private object _locker = new object();
        private int DRAW_AFTER = 40 / 20;
        private long _drawCount;
        private GameTime _gameTime;

        private Worldservice _worldservice;
        private Characterservice _characterservice;
        private ItemsService _itemsService;

        public ElvenGame()
        {
            Trace.WriteLine($"Server bootup at {DateTime.Now}");

            _itemsService = new ItemsService();
            _worldservice = new Worldservice(_itemsService);
            _characterservice = new Characterservice(this, _itemsService, _worldservice);
            

            GameChanges = new ConcurrentQueue<Payload>();
            Gameobjects = new List<Gameobject>();

            _gameLoop = new HighFrequencyTimer(1000 / (float)20, id => Update(id), () => { }, () => { }, (fps) =>
            {
                _actualFps = fps;
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

                    if (_actualFps <= DrawFps || (++_drawCount) % DRAW_AFTER == 0)
                    {
                        //Draw();
                        _drawCount = 0;
                        ProcessGamechanges();
                        //Trace.WriteLine(string.Format("Update {0}", DateTime.Now));
                        OnFpsUpdate();
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
                var p = gameobject.Update(gameTime);
                if (p != null)
                {
                    GameChanges.Enqueue(p);
                }
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
            var spiller = _characterservice.GetOnlineCharacterForUser(userId) as Creature;
            if (spiller == null)
            {
                return false;
            }
            // todo tjek om Location er udfyldt.. det skal den nemlig være...
            spiller.ConnectionId = contextConnectionId;
            var foundPlayer = Gameobjects.FirstOrDefault(a => a.Id == spiller.Id && !string.IsNullOrEmpty(a.ConnectionId)) as Creature;
            if (foundPlayer == null)
            {
                foundPlayer = spiller;
                Gameobjects.Add(foundPlayer);
                Trace.WriteLine($"{foundPlayer.Name} entered the world [lvl {foundPlayer.Level} health {foundPlayer.Health}]");
            }
            else
            {
                foundPlayer.ConnectionId = contextConnectionId;
                Trace.WriteLine($"{foundPlayer.Name} reconnected to the world [lvl {foundPlayer.Level} health {foundPlayer.Health}]");
            }
            

            //CurrentHub.Clients.All.Pong(DateTime.Now);//test
            // todo vi bør lave noget changemap halløj her, og så kun sende elementer fra den verdensdel spilleren er i..
            GameChanges.Enqueue(new Payload { Gameobject = spiller, Type = Payloadtype.AddPlayer });
            foreach (var o in Gameobjects.Where(a => a.ConnectionId != contextConnectionId && a.Location.Zone == spiller.Location.Zone))
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

#if DEBUGOUTPUT
                Trace.WriteLine(string.Format("Send packet {0} [{1} X{2},Y{3}] - Location [X:{4} Y:{5} Zone:{6}]",
                    cnt++,
                    o.Type,
                    (int)o.Gameobject.Position.X,
                    (int)o.Gameobject.Position.Y,
                    o.Gameobject.Location.X,
                    o.Gameobject.Location.Y,
                    o.Gameobject.Location.Zone));
#endif

                // hvis det er mapchange, så sender vi til alle at brugeren skal fjernes
                if (o.Type == Payloadtype.Mapchange)
                {
                    CurrentHub.Clients.All.PushPayload(o);
                }

                if (string.IsNullOrEmpty(o.Receiver))
                {
                    CurrentHub.Clients.Clients(Gameobjects.Where(a=>a.Location.Zone == o.Gameobject.Location.Zone).Select(a=>a.ConnectionId).ToList()).PushPayload(o);
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
