using System;
using Elvencurse2.Client.Components;
using Elvencurse2.Client.GameStates;
using Elvencurse2.Client.StateManager;
using ElvenCurse2.Client.Components;
using ElvenCurse2.Client.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Elvencurse2.Client
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ElvenCurse2 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Statemanager _statemanager;
        public Statemanager Statemanager {get { return _statemanager; }}

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public static Rectangle ScreenRectangle
        {
            get { return screenRectangle; }
        }
        static Rectangle screenRectangle;


        public SoundComponent SoundComponent { get; set; }

        public ElvenCurse2()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = false,
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600
            };
            Content.RootDirectory = "Content";

            SoundComponent = new SoundComponent(this);
            SoundComponent.Load();

            screenRectangle = new Rectangle(0, 0, 1280, 720);

            graphics.PreferredBackBufferWidth = ScreenRectangle.Width;
            graphics.PreferredBackBufferHeight = ScreenRectangle.Height;

            
            IsMouseVisible = true;

            _statemanager = new Statemanager();
            

            //titleIntroState = new TitleIntroState(this);
            //startMenuState = new MainMenuState(this);
            //gamePlayState = new GameplayState(this);

            //_gameStateManager.ChangeState((TitleIntroState)titleIntroState, PlayerIndex.One);

            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Components.Add(new Xin(this));

            

            base.Initialize();
        }

        private bool _windowSizeIsBeingChanged = false;
        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            _windowSizeIsBeingChanged = !_windowSizeIsBeingChanged;
            if (_windowSizeIsBeingChanged)
            {
                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                graphics.ApplyChanges();
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _statemanager.SetLoadingGamestate(new LoadingGamestate(this));
            _statemanager.ChangeState(new TitleIntroState(this));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _statemanager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _statemanager.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
