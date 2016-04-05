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
    class Soldier : Unit
    {
        public enum SoldierState { Idle, Moving, Attacking };
        public SoldierState soldierState = SoldierState.Idle;

        Vector2 targetLocation;
        TextRepresentation healthText;

        public Soldier(Game game, Vector2 position, Graph graph, AIAgent agent)
            : base(game, graph, agent, agent.RenderColor)
        {
            spriteName = "soldier";
            maxHealth = 100;
            health = maxHealth;
            speed = 1.5f;
            maxSpeed = 1.5f;
            maxAccel = .07f;
            stopRadius = 2f;
            slowRadius = 5f;
            this.position = position;
            graph.SetNodeBlocked(position);
            healthText = new TextRepresentation(game, position);
            game.Components.Add(healthText);
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            healthText.Position = new Vector2(position.X - 8, position.Y - 8);
            healthText.Text = health.ToString();

            if (soldierState == SoldierState.Moving)
            {
                if (Vector2.Distance(position, curTarget) < slowRadius + 5 && targetPath.Count > 2)
                {
                    if (!graph.IsNodeBlocked(targetPath.Peek()))
                    {
                        curTarget = targetPath.Pop();
                        if (!graph.IsNodeBlocked(targetPath.Peek()))
                        {
                            curTarget = (curTarget + targetPath.Pop()) / 2;
                        }
                        else
                        {
                            soldierState = SoldierState.Idle;
                            return;
                        }
                    }
                    else
                    {
                        soldierState = SoldierState.Idle;
                        return;
                    }
                    curTarget = new Vector2(curTarget.X + sprite.Width / 2, curTarget.Y + sprite.Height / 2);
                }
                else if (Vector2.Distance(position, curTarget) < slowRadius + 5 && targetPath.Count > 0)
                {
                    if (!graph.IsNodeBlocked(targetPath.Peek()))
                    {
                        curTarget = targetPath.Pop();
                    }
                    else
                    {
                        soldierState = SoldierState.Idle;
                    }
                    curTarget = new Vector2(curTarget.X + sprite.Width / 2, curTarget.Y + sprite.Height / 2);
                }
                else if (targetPath.Count == 0 && Vector2.Distance(position, curTarget) < stopRadius)
                {
                    if (graph.IsNodeBlocked(position))
                    {
                        MoveToLocation(targetLocation);
                        return;
                    }
                    graph.SetNodeBlocked(position);
                    soldierState = SoldierState.Idle;
                }

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
            else if (soldierState == SoldierState.Idle)
            {
                graph.SetNodeBlocked(position);
            }
        }

        public void MoveToLocation(Vector2 location)
        {
            targetLocation = location;
            targetPath.Clear();
            if (graph.GetNode(location).IsBlocked)
            {
                foreach (Edge edge in graph.GetNode(location).Neighbors)
                {
                    if (!edge.GetNeighbor(graph.GetNode(location)).IsBlocked)
                    {
                        targetPath = graph.ComputePath(Position, new Vector2(edge.GetNeighbor(graph.GetNode(location)).Position.X * Game1.WINDOW_WIDTH / (Game1.WINDOW_WIDTH / Game1.graphSize), edge.GetNeighbor(graph.GetNode(location)).Position.Y * Game1.WINDOW_HEIGHT / (Game1.WINDOW_HEIGHT / Game1.graphSize)));
                        break;
                    }
                }
            }
            else
            {
                targetPath = graph.ComputePath(Position, location);
            }

            if (targetPath.Count == 0)
            {
                return;
            }
            curTarget = targetPath.Pop();
            curTarget = new Vector2(curTarget.X + sprite.Width / 2, curTarget.Y + sprite.Height / 2);
            soldierState = SoldierState.Moving;
            graph.UnblockNode(position);
        }
    }
}
