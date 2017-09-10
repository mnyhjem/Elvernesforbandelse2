using Elvencurse2.Model;
using ElvenCurse2.Client.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Elvencurse2.Client.Model
{
    public class Playerentity : Creatureentity
    {
        public Playerentity(ElvenCurse2 game, Payload payload) : base(game, payload)
        {
            CreateSprite();
        }

        public override bool Update(float deltaSeconds, KeyboardState keyboardState)
        {
            if (!IsLoaded)
            {
                return false;
            }

            //var moveDirection = Vector2.Zero;
            var isMoving = false;
            var moveDirection = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                moveDirection -= Vector2.UnitY;
                isMoving = true;
            }


            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                moveDirection -= Vector2.UnitX;
                isMoving = true;
            }


            if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                moveDirection += Vector2.UnitY;
                isMoving = true;
            }


            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                moveDirection += Vector2.UnitX;
                isMoving = true;
            }

            if (moveDirection != Vector2.Zero)
            {
                Direction = Vector2ToDirection(moveDirection);
            }

            //// need to normalize the direction vector incase moving diagonally, but can't normalize the zero vector
            //// however, the zero vector means we didn't want to move this frame anyways so all good
            //var isMoving = moveDirection != Vector2.Zero;
            //if (isMoving)
            //{
            //    moveDirection.Normalize();
            //    //_camera.Move(moveDirection * cameraSpeed * deltaSeconds);
            //    _playerSprite.Position = _playerSprite.Position + Vector2.Transform(moveDirection * MoveSpeed * deltaSeconds, Matrix.CreateRotationZ(-_playerSprite.Rotation));
            //    //_camera.LookAt(_playerSprite.Position);

            //    //_camera.Position = _playerSprite.Position;
            //    //_playerSprite.M
            //}

            if (!string.IsNullOrEmpty(Animation))
            {
                _sprite.Play(Animation);
                Animation = "";
            }

            _sprite.Position = _newPosition;
            _sprite.Update(deltaSeconds);

            return isMoving;
        }
    }
}
