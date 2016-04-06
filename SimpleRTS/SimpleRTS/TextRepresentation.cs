using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleRTS
{
    class TextRepresentation : DrawableGameComponent
    {
        string text = "Default";
        SpriteFont font;
        Game game;
        Vector2 position;

        SpriteBatch spriteBatch;
        public TextRepresentation(Game game, Vector2 position)
            :base (game)
        {
            this.game = game;
            this.position = new Vector2(position.X - 16, position.Y - 8);
            DrawOrder = 1;
            LoadContent();
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            font = Game.Content.Load<SpriteFont>("Arial");
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, position, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
