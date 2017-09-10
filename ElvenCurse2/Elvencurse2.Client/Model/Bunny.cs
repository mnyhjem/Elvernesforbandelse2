using System.Collections.Generic;
using Elvencurse2.Model;
using ElvenCurse2.Client.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using MonoGame.Extended.TextureAtlases;

namespace Elvencurse2.Client.Model
{
    public class Bunny:Creatureentity
    {
        public Bunny(ElvenCurse2 game, Payload payload) : base(game, payload)
        {
            CreateSprite();
        }

        protected override void CreateEntitysprite(object o)
        {
            Texture2D texture = _game.Content.Load<Texture2D>("npcs/whtdragons_MVanimals_ALL/animals/bunny2");

            var regions = new Dictionary<string, Rectangle>();

            // idle
            AddRegion(regions, 0, 0, 32, 32);

            // walkFront
            AddRegion(regions, 0, 0, 32, 32);
            AddRegion(regions, 32, 0, 32, 32);
            AddRegion(regions, 64, 0, 32, 32);
            AddRegion(regions, 96, 0, 32, 32);
            AddRegion(regions, 128, 0, 32, 32);
            AddRegion(regions, 160, 0, 32, 32);

            // walkLeft
            AddRegion(regions, 0, 32, 32, 32);
            AddRegion(regions, 32, 32, 32, 32);
            AddRegion(regions, 64, 32, 32, 32);
            AddRegion(regions, 96, 32, 32, 32);
            AddRegion(regions, 128, 32, 32, 32);
            AddRegion(regions, 160, 32, 32, 32);

            // walkRight
            AddRegion(regions, 0, 64, 32, 32);
            AddRegion(regions, 32, 64, 32, 32);
            AddRegion(regions, 64, 64, 32, 32);
            AddRegion(regions, 96, 64, 32, 32);
            AddRegion(regions, 128, 64, 32, 32);
            AddRegion(regions, 160, 64, 32, 32);

            // walkBack
            AddRegion(regions, 0, 96, 32, 32);
            AddRegion(regions, 32, 96, 32, 32);
            AddRegion(regions, 64, 96, 32, 32);
            AddRegion(regions, 96, 96, 32, 32);
            AddRegion(regions, 128, 96, 32, 32);
            AddRegion(regions, 160, 96, 32, 32);


            var atlas = new TextureAtlas("bunny", texture, regions);
            //var atlas = TextureAtlas.Create("characteratlas", texture, 32, 64);

            var animationfactory = new SpriteSheetAnimationFactory(atlas);

            animationfactory.Add("idle", new SpriteSheetAnimationData(new[] { 0 }, isLooping: false));
            animationfactory.Add("walkFront", new SpriteSheetAnimationData(new[] { 1,2,3,4,5,6 }, isLooping: false));
            animationfactory.Add("walkLeft", new SpriteSheetAnimationData(new[] { 7,8,9,10,11,12 }, isLooping: false));
            animationfactory.Add("walkRight", new SpriteSheetAnimationData(new[] {13,14,15,16,17,18 }, isLooping: false));
            animationfactory.Add("walkBack", new SpriteSheetAnimationData(new[] { 19,20,21,22,23,24 }, isLooping: false));


            _sprite = new AnimatedSprite(animationfactory);
            _sprite.Position = new Vector2(_payload.Gameobject.Position.X, _payload.Gameobject.Position.Y);

            //_sprite.Play("walkFront").IsLooping = true;

            IsLoaded = true;
        }

        private void AddRegion(Dictionary<string, Rectangle> regions, int x, int y, int width, int height)
        {
            var key = regions.Count.ToString();

            regions.Add(key, new Rectangle(x, y, width, height));
        }
    }
}
