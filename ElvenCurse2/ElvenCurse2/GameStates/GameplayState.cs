using System;
using System.Collections.Generic;
using System.Configuration;
using Elvencurse2.Model;
using Elvencurse2.Model.Enums;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.ViewportAdapters;

namespace ElvenCurse2.GameStates
{
    public interface IGamePlayState
    {
        void SetUpNewGame();
        void LoadExistingGame();
        void StartGame();
    }

    public class GameplayState : BaseGameState, IGamePlayState
    {
        private FramesPerSecondCounter _fpsCounter;
        private DefaultViewportAdapter _viewportAdapter;
        private Camera2D _camera;
        private TiledMapRenderer _mapRenderer;

        private List<Client.Model.Player> _players;
        private Client.Model.Player _player;

        private IHubProxy _hubProxy;
        private string _connectionId;

        private BitmapFont _bitmapFont;
        private TiledMap _map;

        private KeyboardState _previousKeyboardState;

        public GameplayState(Game game) : base(game)
        {
            game.Services.AddService(typeof(IGamePlayState), this);
        }

        public override void Initialize()
        {
            _viewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
            _camera = new Camera2D(_viewportAdapter);
            _mapRenderer = new TiledMapRenderer(GraphicsDevice);

            _fpsCounter = new FramesPerSecondCounter();

            base.Initialize();

            _players = new List<Client.Model.Player>();
            SignalR();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            // TODO: use this.Content to load your game content here
            //_player = Content.Load<Texture2D>("orc");
            _bitmapFont = Game.Content.Load<BitmapFont>("montserrat-32");

            _map = Game.Content.Load<TiledMap>("01");

            _player = new Client.Model.Player(Game.Content);

            SetCameraPosition(_player.Position);
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            _mapRenderer.Update(_map, gameTime);

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Game.Exit();
            }
                

            
            if (_player.Update(deltaSeconds, keyboardState))
            {
                if (keyboardState != _previousKeyboardState)
                {
                    _hubProxy.Invoke("MovePlayer", _player.Direction);
                }
            }
            else
            {
                if (keyboardState != _previousKeyboardState)
                {
                    _hubProxy.Invoke("StopMovePlayer");
                }
            }

            if (_player.UpdateCameraposition)
            {
                _player.UpdateCameraposition = false;
                SetCameraPosition(_player.Position);
            }

            foreach (var p in _players)
            {
                p.Update(deltaSeconds, keyboardState);
            }


            const float zoomSpeed = 0.3f;
            if (keyboardState.IsKeyDown(Keys.R))
                _camera.ZoomIn(zoomSpeed * deltaSeconds);

            if (keyboardState.IsKeyDown(Keys.F))
                _camera.ZoomOut(zoomSpeed * deltaSeconds);

            //if (_previousKeyboardState.IsKeyDown(Keys.Tab) && keyboardState.IsKeyUp(Keys.Tab))
            //{
            //    //_map = LoadNextMap();
            //    LookAtMapCenter();
            //}

            //if (_previousKeyboardState.IsKeyDown(Keys.H) && keyboardState.IsKeyUp(Keys.H))
            //    _showHelp = !_showHelp;

            //if (keyboardState.IsKeyDown(Keys.Z))
            //    _camera.Position = Vector2.Zero;

            //if (keyboardState.IsKeyDown(Keys.X))
            //    _camera.LookAt(Vector2.Zero);

            //if (keyboardState.IsKeyDown(Keys.C))
            //    LookAtMapCenter();
            
            _previousKeyboardState = keyboardState;

            _fpsCounter.Update(gameTime);


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            var viewMatrix = _camera.GetViewMatrix();
            var projectionMatrix = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0f, -1f);

            _mapRenderer.Draw(_map, ref viewMatrix, ref projectionMatrix);

            DrawText();

            _fpsCounter.Draw(gameTime);

            _player.Draw(GameRef.SpriteBatch, _camera);

            foreach (var p in _players)
            {
                p.Draw(GameRef.SpriteBatch, _camera);
            }


            base.Draw(gameTime);
        }

        private void DrawText()
        {
            var textColor = Color.Black;
            GameRef.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);

            var baseTextPosition = new Point2(5, 0);

            // textPosition = base position (point) + offset (vector2)
            var textPosition = baseTextPosition + new Vector2(0, _bitmapFont.LineHeight * 0);
            GameRef.SpriteBatch.DrawString(_bitmapFont,
                $"Map: {_map.Name}; {_map.TileLayers.Count} tile layer(s) @ {_map.Width}x{_map.Height} tiles, {_map.ImageLayers.Count} image layer(s)",
                textPosition, textColor);
            textPosition = baseTextPosition + new Vector2(0, _bitmapFont.LineHeight * 1);

            // we can safely get the metrics without worrying about spritebatch interfering because spritebatch submits on End()
            GameRef.SpriteBatch.DrawString(_bitmapFont,
                $"FPS: {_fpsCounter.FramesPerSecond:0}, Draw Calls: {GraphicsDevice.Metrics.DrawCount}, Texture Count: {GraphicsDevice.Metrics.TextureCount}, Triangle Count: {GraphicsDevice.Metrics.PrimitiveCount}",
                textPosition, textColor);
            textPosition = baseTextPosition + new Vector2(0, _bitmapFont.LineHeight * 2);
            GameRef.SpriteBatch.DrawString(_bitmapFont, $"Camera Position: (x={_camera.Position.X}, y={_camera.Position.Y})",
                textPosition, textColor);

            //if (!_showHelp)
            //{
            //    GameRef.SpriteBatch.DrawString(_bitmapFont, "H: Show help", new Vector2(5, _bitmapFont.LineHeight * 3),
            //        textColor);
            //}
            //else
            //{
            //    textPosition = baseTextPosition + new Vector2(0, _bitmapFont.LineHeight * 3);
            //    _spriteBatch.DrawString(_bitmapFont, "H: Hide help", textPosition, textColor);
            //    textPosition = baseTextPosition + new Vector2(0, _bitmapFont.LineHeight * 4);
            //    _spriteBatch.DrawString(_bitmapFont, "WASD/Arrows: Pan camera", textPosition, textColor);
            //    textPosition = baseTextPosition + new Vector2(0, _bitmapFont.LineHeight * 5);
            //    _spriteBatch.DrawString(_bitmapFont, "RF: Zoom camera in / out", textPosition, textColor);
            //    textPosition = baseTextPosition + new Vector2(0, _bitmapFont.LineHeight * 6);
            //    _spriteBatch.DrawString(_bitmapFont, "Z: Move camera to the origin", textPosition, textColor);
            //    textPosition = baseTextPosition + new Vector2(0, _bitmapFont.LineHeight * 7);
            //    _spriteBatch.DrawString(_bitmapFont, "X: Move camera to look at the origin", textPosition, textColor);
            //    textPosition = baseTextPosition + new Vector2(0, _bitmapFont.LineHeight * 8);
            //    _spriteBatch.DrawString(_bitmapFont, "C: Move camera to look at center of the map", textPosition,
            //        textColor);
            //    textPosition = baseTextPosition + new Vector2(0, _bitmapFont.LineHeight * 9);
            //    _spriteBatch.DrawString(_bitmapFont, "Tab: Cycle through maps", textPosition, textColor);
            //}

            GameRef.SpriteBatch.End();
        }

        private void LookAtMapCenter()
        {
            switch (_map.Orientation)
            {
                case TiledMapOrientation.Orthogonal:
                    _camera.LookAt(new Vector2(_map.WidthInPixels, _map.HeightInPixels) * 0.5f);
                    break;
                case TiledMapOrientation.Isometric:
                    _camera.LookAt(new Vector2(0, _map.HeightInPixels + _map.TileHeight) * 0.5f);
                    break;
                case TiledMapOrientation.Staggered:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetCameraPosition(Vector2 position)
        {
            position.X -= _viewportAdapter.ViewportWidth / 2f;
            position.Y -= _viewportAdapter.ViewportHeight / 2f;

            if (position.X <= 0)
            {
                position.X = 0;
            }

            if (position.Y <= 0)
            {
                position.Y = 0;
            }

            if (position.X >= _map.WidthInPixels - _viewportAdapter.ViewportWidth)
            {
                position.X = _map.WidthInPixels - _viewportAdapter.ViewportWidth;
            }

            if (position.Y >= _map.HeightInPixels - _viewportAdapter.ViewportHeight)
            {
                position.Y = _map.HeightInPixels - _viewportAdapter.ViewportHeight;
            }


            _camera.Position = position;
        }

        public void SetUpNewGame()
        {
        }

        public void LoadExistingGame()
        {
        }

        public void StartGame()
        {
        }

        private void SignalR()
        {
            var con = new HubConnection(ConfigurationManager.AppSettings["realm"]);
            _hubProxy = con.CreateHubProxy("ElvenHub");
            _hubProxy.On<DateTime>("Pong", (time) =>
            {
                var i = 0;
            });

            _hubProxy.On<Payload>("PushPayload", (payload) =>
            {
                switch (payload.Type)
                {
                    case Payloadtype.AddEntity:// add player
                        if (payload.Gameobject.ConnectionId != _connectionId)
                        {
                            var newPlayer = new Client.Model.Player(Game.Content)
                            {
                                ConnectionId = payload.Gameobject.ConnectionId
                            };
                            newPlayer.SetPosition(new Vector2(
                                payload.Gameobject.Position.X,
                                payload.Gameobject.Position.Y));
                            _players.Add(newPlayer);
                        }
                        else
                        {
                            _player.SetPosition(new Vector2(
                                payload.Gameobject.Position.X,
                                payload.Gameobject.Position.Y));
                        }
                        break;
                    case Payloadtype.Move:// move
                        var p = _players.Find(a => a.ConnectionId == payload.Gameobject.ConnectionId);
                        var isThisPlayer = false;
                        if (p == null)
                        {
                            if (_connectionId == payload.Gameobject.ConnectionId)
                            {
                                isThisPlayer = true;
                                p = _player;
                            }
                            else
                            {
                                return;
                            }
                        }
                        p.SetPosition(new Vector2(
                            payload.Gameobject.Position.X,
                            payload.Gameobject.Position.Y));

                        p.Animation = string.IsNullOrEmpty(payload.Animation) ? "" : payload.Animation;

                        if (isThisPlayer)
                        {
                            p.UpdateCameraposition = true;
                        }

                        break;
                }
            });

            con.Start().Wait();
            _connectionId = con.ConnectionId;

            _hubProxy.Invoke("EnterWorld").Wait();
        }
    }
}
