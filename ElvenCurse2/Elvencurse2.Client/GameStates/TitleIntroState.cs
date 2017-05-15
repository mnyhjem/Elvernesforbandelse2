using System;
using Elvencurse2.Client.Components;
using Elvencurse2.Client.StateManager;
using ElvenCurse2.Client.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ElvenCurse2.Client.GameStates
{
    public class TitleIntroState : Gamestate
    {
        private readonly Elvencurse2.Client.ElvenCurse2 _game;
        private Texture2D _background;
        private SpriteFont _font;
        private TimeSpan _timeElapsed;
        private string _gameName = "Elvernes forbandelse 2";

        private Textbox _txtUsername;
        private Textbox _txtPassword;
        private Button _btnLogin;
        private Texture2D _loginTexture;
        
        public TitleIntroState(Elvencurse2.Client.ElvenCurse2 game)
        {
            _game = game;
        }

        
        public override void Initialize()
        {
            _timeElapsed = TimeSpan.Zero;
        }

        public override void LoadContent()
        {
            _font = _game.Content.Load<SpriteFont>("Fonts/BeyondWonderland");
            _background = _game.Content.Load<Texture2D>("LoadingBackgrounds/1");

            CreateLoginbox();

            //_game.SoundComponent.PlayMusic("improvisation 1_0");// todo slå music til igen
        }

        private void CreateLoginbox()
        {
            _loginTexture = _game.Content.Load<Texture2D>("UIComponents/uiloginboks");

            _txtUsername = new Textbox(new Rectangle(5, 5, 200, 20), _game);
            _txtUsername.Text = "";
            _txtUsername.FontColor = Color.Black;

            _txtPassword = new Textbox(new Rectangle(5, 25, 200, 20), _game);
            _txtPassword.Text = "";
            _txtPassword.MaskInput = true;
            _txtPassword.FontColor = Color.Black;

            _btnLogin = new Button(new Rectangle(0, 0, 110, 28), _game);
            _btnLogin.Clicked += BtnLoginOnClicked;

#if DEBUG
            _txtUsername.Text = "email@martinnyhjem.dk";
            _txtPassword.UnmaskedText = "123456";
#endif
        }

        private async void BtnLoginOnClicked(object sender, EventArgs eventArgs)
        {
            if (await Authentication.Authenticate(_txtUsername.Text, _txtPassword.UnmaskedText))
            {
                _game.Statemanager.ChangeState(new MainMenuState(_game));
                //manager.ChangeState((MainMenuState)GameRef.StartMenuState, null);
            }
            //System.Diagnostics.Debug.WriteLine("Clicked {0}", c++);
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            PlayerIndex? index = null;

            _timeElapsed += gameTime.ElapsedGameTime;

            if (Xin.CheckKeyReleased(Keys.Space) || Xin.CheckKeyReleased(Keys.Enter) || Xin.CheckMouseReleased(MouseButtons.Left))
            {
                //manager.ChangeState((MainMenuState)GameRef.StartMenuState, index);
            }

            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();
            _txtUsername.Update(gameTime, keyboardState, mouseState);
            _txtPassword.Update(gameTime, keyboardState, mouseState);

            _btnLogin.Update(gameTime, keyboardState, mouseState);
        }

        public override void Draw(GameTime gameTime)
        {
            _game.SpriteBatch.Begin();

            _game.SpriteBatch.Draw(_background, new Rectangle(0, 0, _game.Window.ClientBounds.Width, _game.Window.ClientBounds.Height), Color.White);

            

            Color color = new Color(1f, 1f, 1f) * (float)Math.Abs(Math.Sin(_timeElapsed.TotalSeconds * 2));
            var p = _font.MeasureString(_gameName) / 2;
            p.Y -= _game.Window.ClientBounds.Height / (float)2 - p.Y / 2;
            p.Y += 25;

            var center = new Vector2(_game.Window.ClientBounds.Width / (float)2, _game.Window.ClientBounds.Height / (float)2);
            _game.SpriteBatch.DrawString(_font, _gameName, center, color, 0, p, 1.0f, SpriteEffects.None, 0.5f);

            var loginPosition = new Vector2(center.X - _loginTexture.Width / 2, center.Y);
            _game.SpriteBatch.Draw(_loginTexture, loginPosition, Color.White);
            _game.SpriteBatch.End();

            _txtUsername.SetPosition(new Vector2(loginPosition.X + 55, loginPosition.Y + 85));
            _txtUsername.Draw(gameTime, _game.SpriteBatch);

            _txtPassword.SetPosition(new Vector2(loginPosition.X + 55, loginPosition.Y + 85 + 50));
            _txtPassword.Draw(gameTime, _game.SpriteBatch);

            _btnLogin.SetPosition(new Vector2(loginPosition.X + 115, loginPosition.Y + 180));
            _btnLogin.Draw(gameTime, _game.SpriteBatch);
        }
    }
}
