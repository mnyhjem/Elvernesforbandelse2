using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Elvencurse2.Client.Components
{
    public class EnvironmentComponent
    {
        private readonly ElvenCurse2 _game;
        private readonly Camera2D _camera;
        private Texture2D _lightMask;
        private Effect _lightEffect;
        private static RenderTarget2D _lightsTarget2D;
        private static RenderTarget2D _mainTarget2D;
        private Color _ambientColor;

        private float _timer = 0;

        public EnvironmentComponent(ElvenCurse2 game, Camera2D camera)
        {
            _game = game;
            _camera = camera;

            _ambientColor = new Color(1f, 1f, 1f, 1.0f);

            LoadContent();
        }

        public void CalculateAmbientColor(DateTime time, GameTime gameTime)
        {
            //time = new DateTime(2017,1,1,21,0,0);

            var elapsed = (float) gameTime.ElapsedGameTime.TotalSeconds;
            _timer -= elapsed;
            if (_timer > 0)
            {
                return;
            }

            _timer = 60;
            //new Color(0.1f, 0.1f, 0.2f, 1.0f)
            var lys = Lysintensitet(TidTilMinutter(time));
            _ambientColor = new Color(lys, lys, lys + 0.1f, 1f);
        }

        private static float Lysintensitet(int tid)
        {
            var res = -2.05 * Math.Pow(10, -6) * Math.Pow(tid, 2) + 0.003 * tid + -0.114;
            return (float)res;
        }

        private static int TidTilMinutter(DateTime time)
        {
            var res = (time.Hour * 60) + time.Minute;
            return res;
        }

        public void DrawLights()
        {
            _game.SpriteBatch.Begin(
                SpriteSortMode.Immediate,
                samplerState: SamplerState.PointClamp,
                transformMatrix: _camera.GetViewMatrix(),
                blendState: BlendState.AlphaBlend);

            // vores lyskilder i scenen
            // todo disse skal ligge i en liste af en art og så tegnes her en af gangen.. vi laver en ILightsource som alle lyskilder nedarver fra,
            // som har position og farve..
            _game.SpriteBatch.Draw(_lightMask, new Vector2(100, 100), Color.White);

            //_game.SpriteBatch.Draw(_lightMask, new Vector2(500, 500), Color.Red);

            _game.SpriteBatch.End();
        }

        public void SetRendertarget(Rendertarget target, Color color)
        {
            switch (target)
            {
                case Rendertarget.Lightmask:
                    _game.GraphicsDevice.SetRenderTarget(_lightsTarget2D);
                    //_game.GraphicsDevice.Clear(color);
                    _game.GraphicsDevice.Clear(_ambientColor);
                    break;

                case Rendertarget.MainScene:
                    _game.GraphicsDevice.SetRenderTarget(_mainTarget2D);
                    _game.GraphicsDevice.Clear(color);
                    break;

                case Rendertarget.None:
                    _game.GraphicsDevice.SetRenderTarget(null);
                    _game.GraphicsDevice.Clear(color);
                    break;
            }
        }

        public void SpliceScene()
        {
            SetRendertarget(Rendertarget.None, Color.Black);

            _game.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            _lightEffect.Parameters["lightMask"].SetValue(_lightsTarget2D);
            _lightEffect.CurrentTechnique.Passes[0].Apply();
            _game.SpriteBatch.Draw(_mainTarget2D, Vector2.Zero, Color.White);
            _game.SpriteBatch.End();
        }

        private void LoadContent()
        {
            _lightMask = _game.Content.Load<Texture2D>("Shaders/lightMask1");
            _lightEffect = _game.Content.Load<Effect>("Shaders/lightEffect");
            
            SetSceneSize();
        }

        public enum Rendertarget
        {
            Lightmask,
            MainScene,
            None
        }

        public void SetSceneSize()
        {
            if (_lightsTarget2D == null || _lightsTarget2D.Width != _game.GraphicsDevice.Viewport.Width || _lightsTarget2D.Height != _game.GraphicsDevice.Viewport.Height)
            {
                _lightsTarget2D?.Dispose();
                _lightsTarget2D = new RenderTarget2D(
                    _game.GraphicsDevice,
                    _game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                    _game.GraphicsDevice.PresentationParameters.BackBufferHeight);
            }

            if (_mainTarget2D == null || _mainTarget2D.Width != _game.GraphicsDevice.Viewport.Width || _mainTarget2D.Height != _game.GraphicsDevice.Viewport.Height)
            {
                _mainTarget2D?.Dispose();
                _mainTarget2D = new RenderTarget2D(
                    _game.GraphicsDevice,
                    _game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                    _game.GraphicsDevice.PresentationParameters.BackBufferHeight);
            }
        }
    }
}
