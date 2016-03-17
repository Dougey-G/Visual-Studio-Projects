using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GridSearching
{
    public class HalfDeadCat : Sprite
    {
        float maxSpeed = 3f;
        float speed = 3;
        Vector2 acceleration;
        Vector2 orientation;
        Vector2 oldVelocity;
        float maxAccel = .07f;
        float stopRadius = 5f;
        float slowRadius = 30f;
        float distance;
        Stack<Vector2> targets = new Stack<Vector2>();
        Vector2 curTarget;
        int cellSize = 0;

        public HalfDeadCat(Game game)
            : base(game)
        {
            velocity = new Vector2(0, -1);
            base.spriteName = "halfDeadCat";
            base.LoadContent();
        }

        public void SetImmediatePosition(Vector2 position, int cellSize)
        {
            isFrozen = true;
            this.cellSize = cellSize;
            this.position = new Vector2(position.X + cellSize / 2, position.Y + cellSize / 2);
        }

        public void GiveTargets(Queue<Vector2> positions)
        {
            targets.Clear();
            Queue<Vector2> temp = positions;
            foreach (Vector2 position in temp)
            {
                targets.Push(new Vector2(position.X * cellSize + cellSize/ 2, position.Y * cellSize + cellSize/ 2));
            }
            temp.Clear();
            curTarget = targets.Pop();
            //Console.WriteLine(curTarget);
            isFrozen = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //Console.WriteLine(Vector2.Distance(position, curTarget) + ", " + targets.Count);
            if (Vector2.Distance(position, curTarget) < 20 && targets.Count > 0)
            {
                curTarget = targets.Pop();
                Console.WriteLine(curTarget);
            }
            if (!isFrozen)
            {
                //rotate based on orientation
                base.rotation = (float)Math.Atan2(orientation.X, -orientation.Y);
                acceleration = curTarget - this.Position;
                distance = Math.Abs(acceleration.Length());
                if (distance < stopRadius)
                {
                    speed = 0;
                }
                else if (distance < slowRadius)
                {
                    speed = maxSpeed * distance / slowRadius;
                }
                else
                {
                    speed = maxSpeed;
                }

                oldVelocity = velocity;
                acceleration = Vector2.Normalize(curTarget - this.Position) * maxAccel;
                velocity += velocity * gameTime.ElapsedGameTime.Milliseconds + .5f * acceleration * gameTime.ElapsedGameTime.Milliseconds * gameTime.ElapsedGameTime.Milliseconds;
                velocity = Vector2.Normalize(velocity) * speed;
                position += velocity;


                if (velocity != Vector2.Zero)
                {
                    orientation = velocity;
                }
            }

        }
    }
}
