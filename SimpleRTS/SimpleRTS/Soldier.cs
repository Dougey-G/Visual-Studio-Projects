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
        public enum SoldierState { Idle, Attacking, Defending, Fleeing };
        public SoldierState soldierState = SoldierState.Idle;

        Vector2 targetLocation;
        Building target;
        float soldierRange = 40;
        float timer = 0;
        float timeBetweenAttacks = 1000f; //1 second

        TextRepresentation healthText;

        public Soldier(Game game, Vector2 position, Graph graph, AIAgent agent)
            : base(game, graph, agent, agent.RenderColor)
        {
            spriteName = "Marine";
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
            if (!isActive)
            {
                healthText.Text = "";
                return;
            }
            healthText.Position = new Vector2(position.X - 8, position.Y - 8);
            healthText.Text = health.ToString();

            if (soldierState == SoldierState.Attacking)
            {
                //If soldier is out of range, move them until they are in range
                if (Vector2.Distance(position, targetLocation) > soldierRange)
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
                                Attack(target);
                                return;
                            }
                        }
                        else
                        {
                            Attack(target);
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
                            Attack(target);
                            return;
                        }
                        curTarget = new Vector2(curTarget.X + sprite.Width / 2, curTarget.Y + sprite.Height / 2);
                    }
                    else if (targetPath.Count == 0 && Vector2.Distance(position, curTarget) < stopRadius)
                    {
                        if (graph.IsNodeBlocked(position))
                        {
                            Attack(target);
                            return;
                        }
                        //graph.SetNodeBlocked(position);
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
                else// if (!graph.IsNodeBlocked(position))
                {
                    //graph.SetNodeBlocked(position);
                    if (target.IsActive)
                    {
                        AttackTarget(gameTime);
                    }
                    else
                    {
                        soldierState = SoldierState.Idle;
                    }
                    
                }
            }
            else if (soldierState == SoldierState.Idle)
            {
                graph.SetNodeBlocked(position);
            }
        }

        public bool MoveToLocation(Vector2 location)
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
                return false;
            }
            curTarget = targetPath.Pop();
            curTarget = new Vector2(curTarget.X + sprite.Width / 2, curTarget.Y + sprite.Height / 2);
            graph.UnblockNode(position);
            return true;
        }

        public void Attack(Building target)
        {
            if (MoveToLocation(target.Position))
            {
                soldierState = SoldierState.Attacking;
                this.target = target;
            }
        }

        void AttackTarget(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.Milliseconds;
            if (timer >= timeBetweenAttacks)
            {
                timer = 0;
                target.TakeDamage(10);
            }
        }
    }
}
