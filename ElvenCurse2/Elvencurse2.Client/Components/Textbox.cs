using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Elvencurse2.Client.Components
{
    public class Textbox
    {
        private Rectangle _rectangle;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _lastKeyboardState;
        private SpriteFont _font;
        private Texture2D _texture;
        private Vector2 _position;
        private bool _blink;
        private float _lastBlink;

        private bool _hasFocus;

        public string Text { get; set; }
        public Color FontColor { get; set; }
        public bool MaskInput { get; set; }
        public string UnmaskedText { get; set; }


        public void SetPosition(Vector2 position)
        {
            _position = position;
            _rectangle.X = (int)_position.X;
            _rectangle.Y = (int)_position.Y;
        }

        public Textbox(Rectangle position, Game game)
        {
            _font = game.Content.Load<SpriteFont>("Fonts/StandardFont");
            _rectangle = position;
            _position = new Vector2(_rectangle.X, _rectangle.Y);

            Text = "";
            FontColor = Color.White;

            _texture = new Texture2D(game.GraphicsDevice, 10, _rectangle.Height);
            var colorData = new Color[_texture.Width * _texture.Height];
            for (var i = 0; i < colorData.Length; i++)
            {
                colorData[i] = Color.DarkGray;
            }
            _texture.SetData(colorData);
        }
        
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (_blink && _hasFocus)
            {
                spriteBatch.Draw(_texture, new Vector2(_position.X + _font.MeasureString(Text).X, _position.Y), Color.White);
            }



            spriteBatch.DrawString(_font, Text, _position, FontColor);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            UpdateBlinkingCursor(gameTime);
            _lastKeyboardState = _currentKeyboardState;
            _currentKeyboardState = keyboardState;

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                _hasFocus = _rectangle.Contains(new Point(mouseState.X, mouseState.Y));
            }

            if (!_hasFocus)
            {
                return;
            }

            var keysPressed = _currentKeyboardState.GetPressedKeys();
            foreach (var key in keysPressed)
            {
                if (!_lastKeyboardState.IsKeyUp(key))
                {
                    continue;
                }

                if ((int)key >= 160 && (int)key <= 163)
                {
                    //LeftShift = 160,
                    //RightShift = 161,
                    //LeftControl = 162,
                    //RightControl = 163,
                    continue;
                }
                AddKeyToText(key);
            }
        }

        private void UpdateBlinkingCursor(GameTime gameTime)
        {
            if (_blink)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - _lastBlink >= 600)
                {
                    _lastBlink = (float)gameTime.TotalGameTime.TotalMilliseconds;
                    _blink = !_blink;
                }
            }
            else if (!_blink)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds - _lastBlink >= 425)
                {
                    _lastBlink = (float)gameTime.TotalGameTime.TotalMilliseconds;
                    _blink = !_blink;
                }
            }
        }

        private void AddKeyToText(Keys key)
        {
            var newchar = "";

            if (key == Keys.Back && UnmaskedText.Length > 0)
            {
                UnmaskedText = UnmaskedText.Substring(0, Text.Length - 1);
                UpdateText();
                return;
            }

            if (key == Keys.Space)
            {
                newchar = " ";
            }
            else if (key == Keys.OemPeriod)
            {
                newchar = ".";
            }
            else if ((int)key >= 65 && (int)key <= 90)
            {
                newchar = ((char)key).ToString();
                if (!_currentKeyboardState.IsKeyDown(Keys.RightShift) && !_currentKeyboardState.IsKeyDown(Keys.LeftShift))
                {
                    newchar = newchar.ToLower();
                }
            }
            else if ((int)key >= 48 && (int)key <= 57)
            {
                if (_currentKeyboardState.IsKeyDown(Keys.LeftControl) && _currentKeyboardState.IsKeyDown(Keys.RightAlt))
                {
                    key += 14;
                }
                newchar = ((char)((int)key)).ToString();
            }

            UnmaskedText += newchar;
            UpdateText();
        }

        public void UpdateText()
        {
            if (!MaskInput)
            {
                Text = UnmaskedText;
            }
            else
            {
                Text = "";
                for (var i = 0; i < UnmaskedText.Length; i++)
                {
                    Text += "*";
                }
            }
        }
    }
}
