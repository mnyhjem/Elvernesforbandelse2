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
        private float _moveSpeed = 60f;

        private int _health;
        
        public CreatureRace Race { get; set; }

        

        public int Level { get; set; }
        public int Basehealth
        {
            get
            {
                return _baseHealth;
            }
            set
            {
                _baseHealth = value;
                ResetHealth();
            }
        }
        private int _baseHealth;

        public virtual int Health
        {
            get { return _health < 0 ? 0 : _health; }
        }
        public int MaxHealth
        {
            get
            {
                return GetMaxHealth();
            }
        }

        public Location DefaultLocation { get; set; }

        public CharacterAppearance Appearance { get; set; }

        public CharacterEquipment Equipment { get; set; }


        public Direction Direction { get; set; }
        public bool IsMoving { get; set; }

        public string Animation { get; set; }

        public Creature(IElvenGame elvenGame, IWorldservice worldservice, Creaturetype type) :base(elvenGame, worldservice, type)
        {
            
        }

        public void SetHealth(int newHealth)
        {
            _health = newHealth;
        }

        protected int GetMaxHealth()
        {
            return _baseHealth * Level;
            //return _baseHealth + (15 * Level) - 15;
        }

        public void ResetHealth()
        {
            _health = GetMaxHealth();
        }

        public override Payload Update(Utilities.GameTime gameTime)
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
                var newPosition = Position + Vector2.Transform(moveDirection * _moveSpeed * (float)gameTime.PercentOfSecond, Matrix.CreateRotationZ(0));

                var newLocation = _worldservice.ValidatePosition(newPosition, Location);
                if (newLocation == null)
                {
                    InvalidMoveCounter++;
                    return null;
                }
                
                Position = newPosition;
                Location = newLocation;
                InvalidMoveCounter = 0;


                return new Payload
                {
                    Gameobject = this,
                    Type = Payloadtype.Move,
                    Animation = animation
                };
            }
            return null;
        }

        protected int InvalidMoveCounter { get; set; }
    }
}
