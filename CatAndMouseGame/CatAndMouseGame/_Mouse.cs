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
    public class _Mouse : Sprite
    {
        float moveAmount = 4f;
        bool wasSpaceDown = false;
        Vector2 velocity;

       

        public _Mouse(Game game)
            : base(game)
        {
            base.spriteName = "Mouse";
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!isFrozen)
            {
                KeyboardState keyboard = Keyboard.GetState();

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
                    velocity = Vector2.Normalize(velocity) * moveAmount;
                    position += velocity;

                }


                //Jump mouse to random position if space is pressed.
                if (!wasSpaceDown && keyboard.IsKeyDown(Keys.Space))
                {
                    RespawnInRandomLocation();
                    wasSpaceDown = true;
                }
                else if (keyboard.IsKeyUp(Keys.Space))
                {
                    wasSpaceDown = false;
                }



                //Clamp mouse in the screen
                if (position.Y < 0)
                {
                    position.Y = 0;
                }
                else if (position.Y > Game1.WINDOW_HEIGHT)
                {
                    position.Y = Game1.WINDOW_HEIGHT;
                }
                if (position.X < 0)
                {
                    position.X = 0;
                }
                else if (position.X > Game1.WINDOW_WIDTH)
                {
                    position.X = Game1.WINDOW_WIDTH;
                }

                //rotate based on orientation
                base.rotation = (float)Math.Atan2(velocity.X, -velocity.Y);
            }
        }
    }
}
