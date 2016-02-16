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
        bool canJump = true;
        Bar hyperjumpCooldown;
        float jumpCooldownTime = 5000f;
        float jumpCooldownCounter = 0f;

       

        public _Mouse(Game game)
            : base(game)
        {
            hyperjumpCooldown = new Bar(game, -25f);
            base.spriteName = "Mouse";
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!isFrozen)
            {
                hyperjumpCooldown.Position = new Vector2(position.X, position.Y + hyperjumpCooldown.Offset);
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


                //Jump mouse to random position if space is pressed and cooldown is available
                if (canJump && keyboard.IsKeyDown(Keys.Space))
                {
                    RespawnInRandomLocation();
                    canJump = false;
                    jumpCooldownCounter = 0f;
                }
                else if (jumpCooldownCounter < jumpCooldownTime)
                {
                    jumpCooldownCounter += gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    canJump = true;
                }
                hyperjumpCooldown.Scale = new Vector2(jumpCooldownCounter / jumpCooldownTime, 1f);



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

        public override void UnFreezeContent()
        {
            jumpCooldownCounter = 0;
            canJump = false;
            base.UnFreezeContent();
        }
    }
}
