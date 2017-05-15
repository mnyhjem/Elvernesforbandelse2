using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Elvencurse2.Client.Components;
using Elvencurse2.Client.StateManager;
using ElvenCurse2.Client.Components;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ElvenCurse2.Client.GameStates
{
    public class MainMenuState : Gamestate
    {
        private readonly Elvencurse2.Client.ElvenCurse2 _game;

        private Texture2D background;
        private SpriteFont spriteFont;
        private MenuComponent menuComponent;
        private IHubProxy _hubProxy;

        
        public MainMenuState(Elvencurse2.Client.ElvenCurse2 game)
        {
            _game = game;
        }

        public override void Initialize()
        {
            //throw new NotImplementedException();
        }

        public override void LoadContent()
        {
            spriteFont = _game.Content.Load<SpriteFont>(@"Fonts\InterfaceFont");
            background = _game.Content.Load<Texture2D>(@"GameScreens\menuscreen");

            Texture2D texture = _game.Content.Load<Texture2D>(@"Misc\wooden-button");

            string[] menuItems = { "NEW GAME", "CONTINUE", "OPTIONS", "EXIT" };

            menuComponent = new MenuComponent(spriteFont, texture, menuItems);

            Vector2 position = new Vector2();

            position.Y = 90;
            position.X = 1200 - menuComponent.Width;

            menuComponent.Postion = position;
            

            //// auth test
            //// todo dette herunder skal væk, da det kun er test af auth..
            //var connection = new HubConnection(ConfigurationManager.AppSettings["realm"]);
            //connection.CookieContainer = new CookieContainer();
            //connection.CookieContainer.Add(Authentication.AuthCookie);

            //_hubProxy = connection.CreateHubProxy("TestHub");
            //_hubProxy.On<string>("addNewMessage", (message) =>
            //{
            //    System.Diagnostics.Debug.WriteLine(message);
            //});

            //try
            //{
            //    connection.Start().Wait();
            //    _hubProxy.Invoke("Send", "hejsa").Wait();
            //}
            //catch (AggregateException)
            //{
                
            //}
            
            
        }

        public override void Update(GameTime gameTime)
        {
            menuComponent.Update(gameTime);

            if (Xin.CheckKeyReleased(Keys.Space) || Xin.CheckKeyReleased(Keys.Enter) || (menuComponent.MouseOver && Xin.CheckMouseReleased(MouseButtons.Left)))
            {
                if (menuComponent.SelectedIndex == 0)
                {
                    Xin.FlushInput();
                    
                    _game.Statemanager.ChangeState(new GameplayState(_game));
                    //_game.PushState((GameplayState)GameRef.GamePlayState, PlayerIndexInControl);
                }
                else if (menuComponent.SelectedIndex == 1)
                {
                    Xin.FlushInput();
                }
                else if (menuComponent.SelectedIndex == 2)
                {
                    Xin.FlushInput();
                }
                else if (menuComponent.SelectedIndex == 3)
                {
                    _game.Exit();
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _game.SpriteBatch.Begin();

            _game.SpriteBatch.Draw(background, Vector2.Zero, Color.White);

            _game.SpriteBatch.End();


            _game.SpriteBatch.Begin();

            menuComponent.Draw(gameTime, _game.SpriteBatch);

            _game.SpriteBatch.End();

        }
    }
}
