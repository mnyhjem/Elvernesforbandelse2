using System.Linq;
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

        public Creature(IElvenGame elvenGame, IWorldservice worldservice) :base(elvenGame, worldservice)
        {
            
        }

        public void SetHealth(int newHealth)
        {
            _health = newHealth;
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

                // todo Opdater location objektet
                // todo Det er også det som gør at kortet skifter.. når Zone bliver ændret.
                // todo Samtidigt skal vi også her tjekke om det overhovedet er muligt at gå derhen hvor vi vil.
                // todo Vi kan eventuelt ligge disse tjek i worldservice, da denne bør kende verdenen.
                var map = _worldservice.Worldsections.FirstOrDefault(a => a.Id == Location.Zone);
                if (newPosition.X < 0 || newPosition.X > map.Tilemap.Width * map.Tilemap.Tilewidth)
                {
                    return null;
                }
                if (newPosition.Y < 0 || newPosition.Y > map.Tilemap.Height * map.Tilemap.Tileheight)
                {
                    return null;
                }

                var newLocation = _worldservice.PositionToLocation(newPosition, map);
                // tjek for kollision
                if (map.Tilemap.Layers[6].Data[(int)newLocation.Y * 100 + (int)newLocation.X] > 0)
                {
                    return null;
                }

                Position = newPosition;
                Location = newLocation;
                
                return new Payload
                {
                    Gameobject = this,
                    Type = Payloadtype.Move,
                    Animation = animation
                };
            }
            return null;
        }
    }
}
