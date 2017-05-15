using Elvencurse2.Client.StateManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Elvencurse2.Client.GameStates
{
    public class LoadingGamestate : Gamestate
    {
        private readonly ElvenCurse2 _game;
        private Texture2D _background;
        private SpriteFont _font;

        public LoadingGamestate(ElvenCurse2 game)
        {
            _game = game;
        }

        public override void Initialize()
        {
            //throw new System.NotImplementedException();
        }

        public override void LoadContent()
        {
            _background = _game.Content.Load<Texture2D>("LoadingBackgrounds/3");
            _font = _game.Content.Load<SpriteFont>("Fonts/BeyondWonderland");
        }

        public override void Update(GameTime gameTime)
        {
            //throw new System.NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            //throw new System.NotImplementedException();
            _game.SpriteBatch.Begin();

            _game.SpriteBatch.Draw(_background, new Rectangle(0, 0, _game.Window.ClientBounds.Width, _game.Window.ClientBounds.Height), Color.White);

            _game.SpriteBatch.End();
        }
    }
}
