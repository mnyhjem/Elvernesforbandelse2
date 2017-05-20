using System;
using System.Collections.Generic;
using System.Configuration;
using Elvencurse2.Client.Components;
using Elvencurse2.Client.StateManager;
using Elvencurse2.Model;
using Elvencurse2.Model.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.ViewportAdapters;

namespace ElvenCurse2.Client.GameStates
{
    public class GameplayState : Gamestate
    {
        private readonly Elvencurse2.Client.ElvenCurse2 _game;
        private FramesPerSecondCounter _fpsCounter;
        private DefaultViewportAdapter _viewportAdapter;
        private Camera2D _camera;
        

        private List<Client.Model.Player> _players;
        private Client.Model.Player _player;

        private SignalRComponent _signalRComponent;

        private BitmapFont _bitmapFont;
        

        private KeyboardState _previousKeyboardState;

        private MapComponent _mapComponent;

        private Texture2D _background;

        private DrawState _drawState = DrawState.Loading;

        private EnvironmentComponent _environment;
        
        enum DrawState
        {
            Loading = 0,
            Ready = 1
        }

        public GameplayState(Elvencurse2.Client.ElvenCurse2 game)
        {
            _game = game;
        }

        public override void Initialize()
        {
            _drawState = DrawState.Loading;

            _viewportAdapter = new DefaultViewportAdapter(_game.GraphicsDevice);
            _camera = new Camera2D(_viewportAdapter);

            _mapComponent = new MapComponent(_game.GraphicsDevice, _game, _camera);

            _fpsCounter = new FramesPerSecondCounter();
            
            _players = new List<Client.Model.Player>();
            
            _signalRComponent = new SignalRComponent(ConfigurationManager.AppSettings["realm"]);
            _signalRComponent.LostConnection += _signalRComponent_LostConnection;
            _signalRComponent.Connect();

            _drawState = DrawState.Ready;
        }

        private void _signalRComponent_LostConnection(object sender, EventArgs e)
        {
            _signalRComponent.LostConnection -= _signalRComponent_LostConnection;
            _signalRComponent.Dispose();

            _game.Statemanager.ChangeState(new MainMenuState(_game));
            //manager.ChangeState((MainMenuState)_game.StartMenuState, null);
        }
        

        public override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            _bitmapFont = _game.Content.Load<BitmapFont>("Fonts/montserrat-32");

            _background = _game.Content.Load<Texture2D>("LoadingBackgrounds/1");

            _mapComponent.LoadMap("Maps/01");

            

            _player = new Client.Model.Player(_game.Content);

            SetCameraPosition(_player.Position);

            _environment = new EnvironmentComponent(_game, _camera);
        }

        private void ProcessSignalrQueue()
        {
            Payload payload = null;
            while (_signalRComponent.UnhandledPayload.TryDequeue(out payload))
            {
                switch (payload.Type)
                {
                    case Payloadtype.AddPlayer:// add player
                        if (payload.Gameobject.ConnectionId != _signalRComponent.ConnectionId)
                        {
                            var newPlayer = new ElvenCurse2.Client.Model.Player(_game.Content)
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
                            _player.UpdateCameraposition = true;
                        }
                        break;
                    case Payloadtype.Move:// move
                        var p = _players.Find(a => a.ConnectionId == payload.Gameobject.ConnectionId);
                        var isThisPlayer = false;
                        if (p == null)
                        {
                            if (_signalRComponent.ConnectionId == payload.Gameobject.ConnectionId)
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
            }
        }

        public override void Update(GameTime gameTime)
        {
            var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            _mapComponent.Update(gameTime, keyboardState);

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                _game.Exit();
            }

            ProcessSignalrQueue();

            _environment.CalculateAmbientColor(DateTime.Now, gameTime);//<-- kunne være tid fra serveren..

            if (_player.Update(deltaSeconds, keyboardState))
            {
                if (keyboardState != _previousKeyboardState)
                {
                    _signalRComponent.HubProxy.Invoke("MovePlayer", _player.Direction);
                }
            }
            else
            {
                if (keyboardState != _previousKeyboardState)
                {
                    _signalRComponent.HubProxy.Invoke("StopMovePlayer");
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
        }

        public override void Draw(GameTime gameTime)
        {
            //_game.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            //_game.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            //_game.GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            switch (_drawState)
            {
                case DrawState.Loading:
                    _game.SpriteBatch.Begin();
                    _game.SpriteBatch.Draw(_background, new Rectangle(0, 0, _game.GraphicsDevice.Viewport.Width, _game.GraphicsDevice.Viewport.Height), Color.White);
                    _game.SpriteBatch.End();
                    break;

                case DrawState.Ready:
                    _environment.SetSceneSize();

                    // Scene lys
                    _environment.SetRendertarget(EnvironmentComponent.Rendertarget.Lightmask, Color.White);// farve bruges ikke her..
                    //_game.GraphicsDevice.Clear(new Color(0.1f, 0.1f, 0.2f, 1.0f));

                    // Andre lyskilder
                    _environment.DrawLights();

                    // spil
                    _environment.SetRendertarget(EnvironmentComponent.Rendertarget.MainScene, Color.Transparent);
                    _mapComponent.Draw(gameTime);
                    DrawText();
                    _fpsCounter.Draw(gameTime);
                    _player.Draw(_game.SpriteBatch, _camera);
                    foreach (var p in _players)
                    {
                        p.Draw(_game.SpriteBatch, _camera);
                    }
                    
                    // Splice render targets..
                    _environment.SpliceScene();
                    

                    break;
            }
        }

        private void DrawText()
        {
            var textColor = Color.Black;
            _game.SpriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);

            var baseTextPosition = new Point2(5, 0);

            // textPosition = base position (point) + offset (vector2)
            var textPosition = baseTextPosition + new Vector2(0, _bitmapFont.LineHeight * 0);
            _game.SpriteBatch.DrawString(_bitmapFont,
                $"Map: {_mapComponent.CurrentMap.Name}; {_mapComponent.CurrentMap.TileLayers.Count} tile layer(s) @ {_mapComponent.CurrentMap.Width}x{_mapComponent.CurrentMap.Height} tiles, {_mapComponent.CurrentMap.ImageLayers.Count} image layer(s)",
                textPosition, textColor);
            textPosition = baseTextPosition + new Vector2(0, _bitmapFont.LineHeight * 1);

            // we can safely get the metrics without worrying about spritebatch interfering because spritebatch submits on End()
            _game.SpriteBatch.DrawString(_bitmapFont,
                $"FPS: {_fpsCounter.FramesPerSecond:0}, Draw Calls: {_game.GraphicsDevice.Metrics.DrawCount}, Texture Count: {_game.GraphicsDevice.Metrics.TextureCount}, Triangle Count: {_game.GraphicsDevice.Metrics.PrimitiveCount}",
                textPosition, textColor);
            textPosition = baseTextPosition + new Vector2(0, _bitmapFont.LineHeight * 2);
            _game.SpriteBatch.DrawString(_bitmapFont, $"Camera Position: (x={_camera.Position.X}, y={_camera.Position.Y}) Ambient={_environment.AmbientLevel}", textPosition, textColor);

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

            _game.SpriteBatch.End();
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

            if (position.X >= _mapComponent.CurrentMap.WidthInPixels - _viewportAdapter.ViewportWidth)
            {
                position.X = _mapComponent.CurrentMap.WidthInPixels - _viewportAdapter.ViewportWidth;
            }

            if (position.Y >= _mapComponent.CurrentMap.HeightInPixels - _viewportAdapter.ViewportHeight)
            {
                position.Y = _mapComponent.CurrentMap.HeightInPixels - _viewportAdapter.ViewportHeight;
            }
            
            _camera.Position = position;
        }

        public override void Dispose()
        {
            //_game?.Dispose();//<-- vi skal ikke dispose game......
            _signalRComponent?.Dispose();
            _background?.Dispose();
        }
    }
}
