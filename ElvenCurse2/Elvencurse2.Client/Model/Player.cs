using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.Mime;
using System.Threading;
using Elvencurse2.Model;
using Elvencurse2.Model.Creatures;
using Elvencurse2.Model.Engine;
using Elvencurse2.Model.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using GameTime = Elvencurse2.Model.Utilities.GameTime;

namespace ElvenCurse2.Client.Model
{
    public class Player : Creature
    {
        private readonly Elvencurse2.Client.ElvenCurse2 _game;
        private AnimatedSprite _playerSprite;
        private Vector2 _newPosition;

        private Payload _payload;

        public bool IsLoaded { get; set; }


        public override Vector2 Position
        {
            get
            {
                if (_playerSprite == null)
                {
                    return new Vector2(0,0);
                }
                return _playerSprite.Position;
            }
        }

        public bool UpdateCameraposition { get; set; }

        public Player(IElvenGame elvenGame, IWorldservice worldservice) : base(elvenGame, worldservice)
        {
        }

        public Player(Elvencurse2.Client.ElvenCurse2 game, Payload payload) : base(null, null)
        {
            _game = game;
            _payload = payload;

            Location = _payload.Gameobject.Location;

            ThreadPool.QueueUserWorkItem(CreatePlayersprite);
        }

        public override Payload Update(GameTime gameTime)
        {
            //base.Update(gameTime);
            return null;
        }

        public bool Update(float deltaSeconds, KeyboardState keyboardState)
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
                _playerSprite.Play(Animation);
                Animation = "";
            }

            _playerSprite.Position = _newPosition;
            _playerSprite.Update(deltaSeconds);

            return isMoving;
        }

        private Direction Vector2ToDirection(Vector2 vector)
        {
            if (vector.X == 1 && vector.Y == 1)
            {
                return Direction.SouthEast;
            }

            if (vector.X == -1 && vector.Y == 1)
            {
                return Direction.SouthWest;
            }

            if (vector.X == 1 && vector.Y == -1)
            {
                return Direction.NorthEast;
            }

            if (vector.X == -1 && vector.Y == -1)
            {
                return Direction.NorthWest;
            }

            if (vector.Y == -1)
            {
                return Direction.North;
            }

            if (vector.Y == 1)
            {
                return Direction.South;
            }

            if (vector.X == -1)
            {
                return Direction.West;
            }

            if (vector.X == 1)
            {
                return Direction.East;
            }

            return Direction.East;
        }

        public void Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            if (!IsLoaded)
            {
                return;
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, samplerState: SamplerState.PointClamp, transformMatrix: camera.GetViewMatrix(), blendState: BlendState.AlphaBlend);
            spriteBatch.Draw(_playerSprite);
            spriteBatch.End();
        }

        private void CreatePlayersprite(object o)
        {
            Texture2D texture;
            if (_payload == null || ((Creature)_payload.Gameobject).Appearance == null)
            {
                texture = _game.Content.Load<Texture2D>("charactersprite");
            }
            else
            {
                var sw = new Stopwatch();
                sw.Start();
                texture = CreateSpritetextureFromPayload(_payload);
                sw.Stop();
                Trace.WriteLine(string.Format("Create texture {0}", sw.Elapsed));
            }


            

            var atlas = TextureAtlas.Create("characteratlas", texture, 64, 64);

            var animationfactory = new SpriteSheetAnimationFactory(atlas);
            animationfactory.Add("spellcastBack", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4, 5, 6 }, isLooping: false));
            animationfactory.Add("spellcastLeft", new SpriteSheetAnimationData(new[] { 13, 14, 15, 16, 17, 18, 19 }, isLooping: false));
            animationfactory.Add("spellcastFront", new SpriteSheetAnimationData(new[] { 26, 27, 28, 29, 30, 31, 32 }, isLooping: false));
            animationfactory.Add("spellcastRight", new SpriteSheetAnimationData(new[] { 39, 40, 41, 42, 43, 44, 45 }, isLooping: false));

            animationfactory.Add("thrustBack", new SpriteSheetAnimationData(new[] { 52, 53, 54, 55, 56, 57, 58 }, isLooping: false));
            animationfactory.Add("thrustLeft", new SpriteSheetAnimationData(new[] { 65, 66, 67, 68, 69, 70, 71 }, isLooping: false));
            animationfactory.Add("thrustFront", new SpriteSheetAnimationData(new[] { 78, 79, 80, 81, 82, 83, 84 }, isLooping: false));
            animationfactory.Add("thrustRight", new SpriteSheetAnimationData(new[] { 91, 92, 93, 94, 95, 96, 97 }, isLooping: false));

            animationfactory.Add("walkBack", new SpriteSheetAnimationData(new[] { 104, 105, 106, 107, 108, 109, 110 }, isLooping: false));
            animationfactory.Add("walkLeft", new SpriteSheetAnimationData(new[] { 117, 118, 119, 120, 121, 122, 123 }, isLooping: false));
            animationfactory.Add("walkFront", new SpriteSheetAnimationData(new[] { 130, 131, 132, 133, 134, 135, 136 }, isLooping: false));
            animationfactory.Add("walkRight", new SpriteSheetAnimationData(new[] { 143, 144, 145, 146, 147, 148, 149 }, isLooping: false));

            animationfactory.Add("slashBack", new SpriteSheetAnimationData(new[] { 156, 157, 158, 159, 160, 161, 162 }, isLooping: false));
            animationfactory.Add("slashLeft", new SpriteSheetAnimationData(new[] { 169, 170, 171, 172, 173, 174, 175 }, isLooping: false));
            animationfactory.Add("slashFront", new SpriteSheetAnimationData(new[] { 182, 183, 184, 185, 186, 187, 188 }, isLooping: false));
            animationfactory.Add("slashRight", new SpriteSheetAnimationData(new[] { 195, 196, 197, 198, 199, 200, 201 }, isLooping: false));

            animationfactory.Add("shootBack", new SpriteSheetAnimationData(new[] { 208, 209, 210, 211, 212, 213, 214 }, isLooping: false));
            animationfactory.Add("shootLeft", new SpriteSheetAnimationData(new[] { 221, 222, 223, 224, 225, 226, 227 }, isLooping: false));
            animationfactory.Add("shootFront", new SpriteSheetAnimationData(new[] { 234, 235, 236, 237, 238, 239, 240 }, isLooping: false));
            animationfactory.Add("shootRight", new SpriteSheetAnimationData(new[] { 247, 248, 249, 250, 251, 252, 253 }, isLooping: false));

            animationfactory.Add("hurtBack", new SpriteSheetAnimationData(new[] { 260, 261, 262, 263, 264, 265 }, isLooping: false));
            animationfactory.Add("hurtLeft", new SpriteSheetAnimationData(new[] { 273, 274, 275, 276, 277, 278 }, isLooping: false));
            animationfactory.Add("hurtFront", new SpriteSheetAnimationData(new[] { 286, 287, 288, 289, 290, 291 }, isLooping: false));
            animationfactory.Add("hurtRight", new SpriteSheetAnimationData(new[] { 299, 300, 301, 302, 303, 304 }, isLooping: false));

            _playerSprite = new AnimatedSprite(animationfactory);
            _playerSprite.Position = new Vector2(_payload.Gameobject.Position.X, _payload.Gameobject.Position.Y);
            //_playerSprite.Play("spellcastBack").IsLooping = true;

            IsLoaded = true;
        }

        

        private Texture2D CreateSpritetextureFromPayload(Payload payload)
        {
            var sprite = GetBody(((Creature)payload.Gameobject).Appearance);
            sprite = GetEquipment(((Creature)payload.Gameobject).Equipment, sprite);


            sprite = Merge(sprite, GetImage("weapons/right hand/either/bow"));
            sprite = Merge(sprite, GetImage("weapons/left hand/either/arrow"));

            Texture2D texture;
            using (var ms = new MemoryStream(sprite))
            {
                texture = Texture2D.FromStream(_game.GraphicsDevice, ms);
            }
            
            return texture;
        }

        private byte[] GetBody(CharacterAppearance ca)
        {
            var image = GetImage($"body/{ca.Sex}/{ca.Body}");
            image = Merge(image, GetImage($"body/{ca.Sex}/eyes/{ca.Eyecolor}"));

            if (ca.Nose != Nose.Default)
            {
                image = Merge(image, GetImage($"body/{ca.Sex}/nose/{ca.Nose}_{ca.Body}"));
            }

            if (ca.Ears != Ears.Default)
            {
                image = Merge(image, GetImage($"body/{ca.Sex}/Ears/{ca.Ears}_{ca.Body}"));
            }

            if (ca.Hair.Type != Hair.HairType.None)
            {
                image = Merge(image, GetImage($"hair/{ca.Sex}/{ca.Hair.Type}/{ca.Hair.Color.ToString().Replace("_", "-")}"));
            }

            if (ca.Facial.Type != Facial.FacialType.None)
            {
                image = Merge(image, GetImage($"facial/{ca.Sex}/{ca.Facial.Type}/{ca.Facial.Color.ToString().Replace("_", "-")}"));
            }

            return image;
        }

        private byte[] GetEquipment(CharacterEquipment ce, byte[] sprite)
        {
            if (ce == null)
            {
                return sprite;
            }

            if (ce.Head != null)
            {
                sprite = Merge(sprite, GetImage(ce.Head.Imagepath));
            }

            if (ce.Chest != null)
            {
                sprite = Merge(sprite, GetImage(ce.Chest.Imagepath));
            }

            if (ce.Arms != null)
            {
                sprite = Merge(sprite, GetImage(ce.Arms.Imagepath));
            }

            if (ce.Shoulders != null)
            {
                sprite = Merge(sprite, GetImage(ce.Shoulders.Imagepath));
            }

            if (ce.Bracers != null)
            {
                sprite = Merge(sprite, GetImage(ce.Bracers.Imagepath));
            }

            if (ce.Hands != null)
            {
                sprite = Merge(sprite, GetImage(ce.Hands.Imagepath));
            }

            if (ce.Legs != null)
            {
                sprite = Merge(sprite, GetImage(ce.Legs.Imagepath));
            }

            if (ce.Feet != null)
            {
                sprite = Merge(sprite, GetImage(ce.Feet.Imagepath));
            }

            if (ce.Belt != null)
            {
                sprite = Merge(sprite, GetImage(ce.Belt.Imagepath));
            }

            if (ce.Weapon != null)
            {
                sprite = Merge(sprite, GetImage(ce.Weapon.Imagepath));
                // if bow, sæt også weapons\left hand\either\arrow på, så der er en pil på buen..
                //sprite = Merge(sprite, GetImage(ce.Torso.Imagepath));
            }

            return sprite;
            //var image = GetImage($"body/{ca.Sex}/{ca.Body}");
            //image = Merge(image, GetImage($"body/{ca.Sex}/eyes/{ca.Eyecolor}"));

            //if (ca.Nose != Nose.Default)
            //{
            //    image = Merge(image, GetImage($"body/{ca.Sex}/nose/{ca.Nose}_{ca.Body}"));
            //}

            //if (ca.Ears != Ears.Default)
            //{
            //    image = Merge(image, GetImage($"body/{ca.Sex}/Ears/{ca.Ears}_{ca.Body}"));
            //}

            //if (ca.Hair.Type != Hair.HairType.None)
            //{
            //    image = Merge(image, GetImage($"hair/{ca.Sex}/{ca.Hair.Type}/{ca.Hair.Color.ToString().Replace("_", "-")}"));
            //}

            //if (ca.Facial.Type != Facial.FacialType.None)
            //{
            //    image = Merge(image, GetImage($"facial/{ca.Sex}/{ca.Facial.Type}/{ca.Facial.Color.ToString().Replace("_", "-")}"));
            //}

            //return image;
        }

        private byte[] Merge(byte[] bytearray1, byte[] bytearray2)
        {
            using (var ms1 = new MemoryStream(bytearray1))
            using (var ms2 = new MemoryStream(bytearray2))
            {
                using (var image1 = Image.FromStream(ms1))
                using (var image2 = Image.FromStream(ms2))
                {
                    using (var target = new Bitmap(image1.Width, image1.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                    {
                        using (var graphics = Graphics.FromImage(target))
                        {
                            graphics.CompositingMode = CompositingMode.SourceOver; // this is the default, but just to be clear

                            graphics.DrawImage(image1, 0, 0);
                            graphics.DrawImage(image2, 0, 0);

                            return ImageToByteArray(target);
                        }
                    }
                }
            }
        }

        private byte[] GetImage(string path)
        {
            var sw = new Stopwatch();
            sw.Start();

            var rootPath = System.Environment.CurrentDirectory + "/Content/CS/U/";
            using (var img = Image.FromFile(rootPath + path + ".png"))
            using (var bmp = new Bitmap(img))
            {
                var bytes = ImageToByteArray(bmp);

                sw.Stop();
                Debug.WriteLine($"GetImage {sw.Elapsed}");
                return bytes;
            }
        }

        private static byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private static byte[] ImageToByteArray(Image img)
        {
            //var sw = new Stopwatch();
            //sw.Start();
            ImageConverter converter = new ImageConverter();
            var bytes = (byte[])converter.ConvertTo(img, typeof(byte[]));
            //sw.Stop();
            //Debug.WriteLine($"ImageToBytes {sw.Elapsed}");

            return bytes;
        }

        public void SetPosition(Vector2 position)
        {
            _newPosition = position;
        }
    }
}
