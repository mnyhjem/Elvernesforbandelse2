using Elvencurse2.Model.Creatures;
using Elvencurse2.Model.Creatures.Npcs;
using Elvencurse2.Model.Engine;
using Elvencurse2.Model.Enums;
using Microsoft.Xna.Framework;

namespace Elvencurse2.Model
{
    public class Creature : Gameobject
    {
        //private float MoveSpeed = 120f;
        private float _moveSpeed = 140f;

        private int _health;
        
        public CreatureRace Race { get; set; }

        public int Level { get; set; }
        public int Basehealth { get; set; }

        public virtual int Health
        {
            get { return _health < 0 ? 0 : _health; }
        }

        public Location DefaultLocation { get; set; }

        public CharacterAppearance Appearance { get; set; }

        public CharacterEquipment Equipment { get; set; }


        public Direction Direction { get; set; }
        public bool IsMoving { get; set; }

        public string Animation { get; set; }

        public Creature(IElvenGame elvenGame):base(elvenGame)
        {
            
        }

        public void SetHealth(int newHealth)
        {
            _health = newHealth;
        }

        public override void Update(Utilities.GameTime gameTime)
        {
            var moveDirection = Vector2.Zero;
            var animation = "";

            if (Direction == Direction.North || Direction == Direction.NorthWest || Direction == Direction.NorthEast)
            {
                animation = "walkBack";
                moveDirection -= Vector2.UnitY;
            }

            if (Direction == Direction.West || Direction == Direction.NorthWest || Direction == Direction.SouthWest)
            {
                animation = "walkLeft";
                moveDirection -= Vector2.UnitX;
            }

            if (Direction == Direction.South || Direction == Direction.SouthEast || Direction == Direction.SouthWest)
            {
                animation = "walkFront";
                moveDirection += Vector2.UnitY;
            }

            if (Direction == Direction.East || Direction == Direction.SouthEast || Direction == Direction.NorthEast)
            {
                animation = "walkRight";
                moveDirection += Vector2.UnitX;
            }

            if (Direction == Direction.SouthWest)
            {
                animation = "walkLeft";
            }

            // need to normalize the direction vector incase moving diagonally, but can't normalize the zero vector
            // however, the zero vector means we didn't want to move this frame anyways so all good
            var isMoving = moveDirection != Vector2.Zero;
            if (isMoving && IsMoving)
            {
                moveDirection.Normalize();
                Position = Position + Vector2.Transform(moveDirection * _moveSpeed * (float)gameTime.PercentOfSecond, Matrix.CreateRotationZ(0));

                // todo Vi bør nok returnere payload.. så vi ikke laver selve ændringen her i klassen, men i stedet gør det i update loopet..
                ElvenGame.GameChanges.Enqueue(
                    new Payload
                    {
                        Gameobject = this,
                        Type = Payloadtype.Move,
                        Animation = animation
                    });
            }
        }
    }
}
