using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace GridSearching
{
    public class Sprite : DrawableGameComponent
    {

        protected Texture2D sprite;
        protected Rectangle drawRectangle;
        protected float rotation = 0;
        protected string spriteName;
        protected Vector2 position;
        protected Vector2 scale;
        protected Vector2 velocity;

        SpriteBatch spriteBatch;
        Random rand = new Random();

        protected bool isFrozen = true;

        public Sprite(Game game)
            : base(game)
        {
            drawRectangle = new Rectangle();
            position = new Vector2(drawRectangle.Center.X, drawRectangle.Center.Y);
            scale = Vector2.One;
            LoadContent();
        }

        public int X
        {
            get { return drawRectangle.Center.X; }
        }

        public int Y
        {
            get { return drawRectangle.Center.Y; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
        }


        public override void Initialize()
        {
            sprite = Game.Content.Load<Texture2D>(spriteName);
            drawRectangle.Width = sprite.Width;
            drawRectangle.Height = sprite.Height;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            //spriteBatch.Draw(sprite, drawRectangle, Color.White);
            spriteBatch.Draw(sprite, position, null, Color.White, rotation, new Vector2(sprite.Width / 2, sprite.Height / 2), scale, SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
        }

        public virtual void FreezeContent()
        {
            isFrozen = true;
        }

        public virtual void UnFreezeContent()
        {
            isFrozen = false;
        }
    }
}
