using System.Collections.Generic;
using System.Diagnostics;
using Elvencurse2.Model;
using ElvenCurse2.Client.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.TextureAtlases;

namespace Elvencurse2.Client.Model
{
    public class Wolf: Creatureentity
    {
        public Wolf(ElvenCurse2 game, Payload payload) : base(game, payload)
        {
            CreateSprite();
        }

        protected override void CreateEntitysprite(object o)
        {
            Texture2D texture = _game.Content.Load<Texture2D>("npcs/wolf/wolfsheet1_0");
            var regions = new Dictionary<string, Rectangle>();

            // idle
            AddRegion(regions, 0, 1, 32, 64);
            AddRegion(regions, 32, 1, 32, 64);
            AddRegion(regions, 64, 1, 32, 64);
            AddRegion(regions, 96, 1, 32, 64);
            AddRegion(regions, 128, 1, 32, 64);

            // liedownFront
            AddRegion(regions, 0, 64, 32, 64);
            AddRegion(regions, 32, 64, 32, 64);
            AddRegion(regions, 64, 64, 32, 64);
            AddRegion(regions, 96, 64, 32, 64);

            // liedownBack
            AddRegion(regions, 160, 64, 32, 64);
            AddRegion(regions, 192, 64, 32, 64);
            AddRegion(regions, 224, 64, 32, 64);
            AddRegion(regions, 256, 64, 32, 64);

            // walkFront
            AddRegion(regions, 0, 128, 32, 64);
            AddRegion(regions, 32, 128, 32, 64);
            AddRegion(regions, 64, 128, 32, 64);
            AddRegion(regions, 96, 128, 32, 64);

            // walkBack
            AddRegion(regions, 160, 128, 32, 64);
            AddRegion(regions, 192, 128, 32, 64);
            AddRegion(regions, 224, 128, 32, 64);
            AddRegion(regions, 256, 128, 32, 64);

            // attackFront
            AddRegion(regions, 0, 256, 32, 64);
            AddRegion(regions, 32, 256, 32, 64);
            AddRegion(regions, 64, 256, 32, 64);
            AddRegion(regions, 96, 256, 32, 64);
            AddRegion(regions, 128, 256, 32, 64);

            // attackBack
            AddRegion(regions, 160, 256, 32, 64);
            AddRegion(regions, 192, 256, 32, 64);
            AddRegion(regions, 224, 256, 32, 64);
            AddRegion(regions, 256, 256, 32, 64);
            AddRegion(regions, 288, 256, 32, 64);

            // walkRight
            AddRegion(regions, 320, 96, 64, 32);
            AddRegion(regions, 384, 96, 64, 32);
            AddRegion(regions, 448, 96, 64, 32);
            AddRegion(regions, 513, 96, 64, 32);
            AddRegion(regions, 577, 96, 64, 32);

            // attackRight
            AddRegion(regions, 320, 160, 64, 32);
            AddRegion(regions, 384, 160, 64, 32);
            AddRegion(regions, 448, 160, 64, 32);
            AddRegion(regions, 513, 160, 64, 32);
            AddRegion(regions, 577, 160, 64, 32);

            // lieLeft
            AddRegion(regions, 320, 192, 64, 32);
            AddRegion(regions, 384, 192, 64, 32);
            AddRegion(regions, 448, 192, 64, 32);
            AddRegion(regions, 513, 192, 64, 32);

            // walkLeft
            AddRegion(regions, 320, 288, 64, 32);
            AddRegion(regions, 384, 288, 64, 32);
            AddRegion(regions, 448, 288, 64, 32);
            AddRegion(regions, 513, 288, 64, 32);
            AddRegion(regions, 577, 288, 64, 32);

            // attackLeft
            AddRegion(regions, 320, 352, 64, 32);
            AddRegion(regions, 384, 352, 64, 32);
            AddRegion(regions, 448, 352, 64, 32);
            AddRegion(regions, 513, 352, 64, 32);
            AddRegion(regions, 577, 352, 64, 32);

            var atlas = new TextureAtlas("wolf", texture, regions);
            //var atlas = TextureAtlas.Create("characteratlas", texture, 32, 64);

            var animationfactory = new SpriteSheetAnimationFactory(atlas);

            animationfactory.Add("idle", new SpriteSheetAnimationData(new[] { 0, 1, 2, 3, 4}, isLooping: false));
            animationfactory.Add("liedownFront", new SpriteSheetAnimationData(new[] { 5,6,7,8 }, isLooping: false));
            animationfactory.Add("liedownBack", new SpriteSheetAnimationData(new[] { 9, 10, 11, 12 }, isLooping: false));
            animationfactory.Add("walkFront", new SpriteSheetAnimationData(new[] { 13, 14, 15, 16 }, isLooping: false));
            animationfactory.Add("walkBack", new SpriteSheetAnimationData(new[] { 17, 18, 19, 20 }, isLooping: false));
            animationfactory.Add("attackFront", new SpriteSheetAnimationData(new[] { 21, 22, 23, 24, 25 }, isLooping: false));
            animationfactory.Add("attackBack", new SpriteSheetAnimationData(new[] { 26, 27, 28, 29, 30 }, isLooping: false));
            animationfactory.Add("walkRight", new SpriteSheetAnimationData(new[] { 31, 32, 33, 34, 35 }, isLooping: false));
            animationfactory.Add("attackRight", new SpriteSheetAnimationData(new[] { 36, 37, 38, 39, 40 }, isLooping: false));
            animationfactory.Add("lieLeft", new SpriteSheetAnimationData(new[] { 41, 42, 43, 44 }, isLooping: false));
            animationfactory.Add("walkLeft", new SpriteSheetAnimationData(new[] { 45, 46, 47, 48, 49 }, isLooping: false));
            animationfactory.Add("attackLeft", new SpriteSheetAnimationData(new[] { 50, 51, 52, 53, 54 }, isLooping: false));


            //animationfactory.Add("walkBack", new SpriteSheetAnimationData(new[] { 104, 105, 106, 107, 108, 109, 110 }, isLooping: false));
            //animationfactory.Add("walkLeft", new SpriteSheetAnimationData(new[] { 117, 118, 119, 120, 121, 122, 123 }, isLooping: false));
            //animationfactory.Add("walkRight", new SpriteSheetAnimationData(new[] { 143, 144, 145, 146, 147, 148, 149 }, isLooping: false));

            _sprite = new AnimatedSprite(animationfactory);
            _sprite.Position = new Vector2(_payload.Gameobject.Position.X, _payload.Gameobject.Position.Y);

            //_sprite.Play("idle").IsLooping = true;

            IsLoaded = true;
        }

        private void AddRegion(Dictionary<string, Rectangle> regions, int x, int y, int width, int height)
        {
            var key = regions.Count.ToString();

            regions.Add(key, new Rectangle(x, y, width, height));
        }
    }
}
