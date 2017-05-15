using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Tiled;

namespace Elvencurse2.Client.Components
{
    public class MapComponent
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Game _game;
        private readonly Camera2D _camera;
        private TiledMap _map;
        private TiledMapRenderer _mapRenderer;

        private string _currentTestMap;

        public TiledMap CurrentMap
        {
            get { return _map; }
        }

        public MapComponent(GraphicsDevice graphicsDevice, Game game, Camera2D camera)
        {
            _graphicsDevice = graphicsDevice;
            _game = game;
            _camera = camera;
            _mapRenderer = new TiledMapRenderer(graphicsDevice);
        }

        public void LoadMap(string mapName)
        {
            _map = _game.Content.Load<TiledMap>(mapName);
            var collissionLayer = _map.Layers.FirstOrDefault(a => a.Name.ToLower() == "collision" || a.Name.ToLower() == "collisionlayer");
            if (collissionLayer != null)
            {
                collissionLayer.IsVisible = false;
            }

            _currentTestMap = mapName;
        }

        public void LookAtMapCenter()
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

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            _mapRenderer.Update(_map, gameTime);


            // Dette er demo halløj.. skal væk i den rigtige udgave..
            if (keyboardState.IsKeyDown(Keys.Tab))
            {
                if (_currentTestMap == "Maps/01")
                {
                    _currentTestMap = "Maps/02";
                }
                else
                {
                    _currentTestMap = "Maps/01";
                }
                LoadMap(_currentTestMap);
            }
        }

        public void Draw(GameTime gameTime)
        {
            var viewMatrix = _camera.GetViewMatrix();
            var projectionMatrix = Matrix.CreateOrthographicOffCenter(0, _graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height, 0, 0f, -1f);
            _mapRenderer.Draw(_map, ref viewMatrix, ref projectionMatrix);
        }
    }
}
