using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CatAndMouseGame
{
    public class _Cat
    {
        Texture2D sprite;
        Rectangle drawRectangle;
        int moveAmount = 7;

        public _Cat(ContentManager contentManager, string spriteName, int x, int y)
        {
            LoadContent(contentManager, spriteName, x, y);
        }

        private int X
        {
            get { return drawRectangle.Center.X; }
        }

        private int Y
        {
            get { return drawRectangle.Center.Y; }
        }


        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }

        private void LoadContent(ContentManager contentManager, string spriteName, int x, int y)
        {
            sprite = contentManager.Load<Texture2D>(spriteName);
            drawRectangle = new Rectangle(x - sprite.Width / 2, y - sprite.Height / 2, sprite.Width, sprite.Height);
        }
    }
}

