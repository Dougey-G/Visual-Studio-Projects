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
    public class _Mouse
    {
        Texture2D sprite;
        Rectangle drawRectangle;
        int moveAmount = 5;
        bool wasSpaceDown = false;
        Vector2 velocity;

        Random rand = new Random();

        public _Mouse(ContentManager contentManager, string spriteName, int x, int y)
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


        public void Update(GameTime gameTime, KeyboardState keyboard)
        {

            //Control mouse with keyboard
            velocity = Vector2.Zero;
            if (keyboard.IsKeyDown(Keys.W))
            {
                velocity.Y -= moveAmount;
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                velocity.X -= moveAmount;
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                velocity.Y += moveAmount;
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                velocity.X += moveAmount;
            }
            if (velocity != Vector2.Zero)
            {
                velocity.Normalize();
                drawRectangle.X += (int)(velocity.X * moveAmount);
                drawRectangle.Y += (int)(velocity.Y * moveAmount);
            }


            //Jump mouse to random position if space is pressed.
            if (!wasSpaceDown && keyboard.IsKeyDown(Keys.Space))
            {
                drawRectangle.X = rand.Next(0, Game1.WINDOW_WIDTH);
                drawRectangle.Y = rand.Next(0, Game1.WINDOW_HEIGHT);
                wasSpaceDown = true;
            }
            else if (keyboard.IsKeyUp(Keys.Space))
            {
                wasSpaceDown = false;
            }



            //Clamp mouse in the screen
            if (drawRectangle.Top < 0)
            {
                drawRectangle.Y = 0;
            }
            else if (drawRectangle.Bottom > Game1.WINDOW_HEIGHT)
            {
                drawRectangle.Y = Game1.WINDOW_HEIGHT - drawRectangle.Height;
            }
            if (drawRectangle.Left < 0)
            {
                drawRectangle.X = 0;
            }
            else if (drawRectangle.Right > Game1.WINDOW_WIDTH)
            {
                drawRectangle.X = Game1.WINDOW_WIDTH - drawRectangle.Width;
            }
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
