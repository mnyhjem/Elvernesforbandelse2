using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Elvencurse2.Client.Components
{
    public class Button
    {
        private Rectangle _rectangle;
        private readonly Game _game;
        private Vector2 _position;

        private Texture2D _texture;
        private bool _isClicked;

        public event EventHandler Clicked;

        public Button(Rectangle rectangle, Game game)
        {
            _rectangle = rectangle;
            _position = new Vector2(_rectangle.X, _rectangle.Y);
            _game = game;

            CreateTexture();
        }

        private void OnClicked()
        {
            Clicked?.Invoke(this, new EventArgs());
        }

        private void CreateTexture()
        {
            _texture = new Texture2D(_game.GraphicsDevice, _rectangle.Width, _rectangle.Height);
            var colorData = new Color[_texture.Width * _texture.Height];
            for (var i = 0; i < colorData.Length; i++)
            {
                colorData[i] = Color.DarkGray;
            }
            _texture.SetData(colorData);
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
            _rectangle.X = (int)_position.X;
            _rectangle.Y = (int)_position.Y;
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            var last = _isClicked;
            _isClicked = mouseState.LeftButton == ButtonState.Pressed && _rectangle.Contains(new Point(mouseState.X, mouseState.Y));

            if (_isClicked && !last)
            {
                OnClicked();
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_isClicked)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(_texture, _rectangle, Color.White * 0.5f);
                spriteBatch.End();
            }
        }
    }
}
