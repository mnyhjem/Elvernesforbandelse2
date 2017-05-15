using System;
using Microsoft.Xna.Framework;

namespace Elvencurse2.Client.StateManager
{
    public abstract class Gamestate:IDisposable
    {

        public abstract void Initialize();
        public abstract void LoadContent();


        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);

        public virtual void Dispose()
        {
        }
    }

    public class Statemanager: Gamestate
    {
        public Gamestate ActiveGamestate { get; private set; }



        private bool _lockObj;
        private Gamestate _loadingState;

        public Statemanager()
        {
            
        }

        public void SetLoadingGamestate(Gamestate state)
        {
            _loadingState = state;
            _loadingState.Initialize();
            _loadingState.LoadContent();
        }

        public void ChangeState(Gamestate state)
        {
            _lockObj = true;
            
            ActiveGamestate?.Dispose();

            ActiveGamestate = state;
            ActiveGamestate.Initialize();
            ActiveGamestate.LoadContent();
            
            _lockObj = false;


        }

        public override void Initialize()
        {
            if (!_lockObj)
            {
                ActiveGamestate.Initialize();
            }
            
        }

        public override void LoadContent()
        {
            if (!_lockObj)
            {
                ActiveGamestate.LoadContent();
            }
            
        }

        public override void Update(GameTime gameTime)
        {
            if (_lockObj)
            {
                _loadingState.Update(gameTime);
                return;
            }
            ActiveGamestate.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_lockObj)
            {
                _loadingState.Draw(gameTime);
                return;
            }
            ActiveGamestate.Draw(gameTime);
        }
    }
}
