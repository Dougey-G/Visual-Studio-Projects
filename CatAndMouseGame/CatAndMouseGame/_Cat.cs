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
    public class _Cat : Sprite
    {
        float maxSpeed = 6f;
        float speed = 6;
        Vector2 velocity = new Vector2(0, -1);
        Vector2 acceleration;
        Vector2 orientation;
        float maxAccel = .05f;
        float stopRadius = 30f;
        float slowRadius = 100f;
        float distance;
        _Mouse target;

        public _Cat(Game game, _Mouse target)
            : base(game)
        {
            this.target = target;
            base.spriteName = "Cat";
            acceleration = target.Position - this.Position;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!isFrozen)
            {
                //rotate based on orientation
                base.rotation = (float)Math.Atan2(orientation.X, -orientation.Y);
                acceleration = target.Position - this.Position;
                distance = Math.Abs(acceleration.Length());
                if (distance < stopRadius)
                {
                    speed = 0;
                    Game1.CatIsWinner();
                }
                else if (distance < slowRadius)
                {
                    speed = maxSpeed * distance / slowRadius;
                }
                else
                {
                    speed = maxSpeed;
                }
                acceleration = Vector2.Normalize(target.Position - this.Position) * maxAccel;
                velocity += velocity * gameTime.ElapsedGameTime.Milliseconds + .5f * acceleration * gameTime.ElapsedGameTime.Milliseconds * gameTime.ElapsedGameTime.Milliseconds;
                velocity = Vector2.Normalize(velocity) * speed;
                position += velocity;

                if (velocity != Vector2.Zero)
                {
                    orientation = velocity;
                }


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
            }
          
        }
    }
}
