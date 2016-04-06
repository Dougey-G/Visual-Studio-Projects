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
    class Peon : Unit
    {
        #region fields
        public enum PeonState { Idle, Moving, Mining, Fleeing, Building };
        public PeonState peonState = PeonState.Idle;

        public enum PeonObjective { Idle, Build, GoToBuild }
        public PeonObjective peonObjective = PeonObjective.Idle;

        Vector2 targetLocation;
        GoldMine goldMine;
        int goldCount = 0;
        int goldCountMax = 50;
        int goldToGetPerTime = 5;
        float timer = 0;
        float timeToGetGold = 1000f;
        TextRepresentation goldText;
        bool canBuild;
        Building.BuildingType buildingType;
        Node curNode, tempNode;
        #endregion

        public Peon(Game game, Vector2 position, Graph graph, AIAgent agent)
            : base(game, graph, agent, agent.RenderColor)
        {
            spriteName = "peon";
            speed = 1f;
            maxSpeed = 1f;
            maxAccel = .07f;
            stopRadius = 2f;
            slowRadius = 5f;
            this.position = position;
            graph.SetNodeBlocked(position);
            goldText = new TextRepresentation(game, position);
            Game.Components.Add(goldText);
        }

        public int GoldCount
        {
            get { return goldCount; }
            set { goldCount = value; }
        }

        public int GoldCountMax
        {
            get { return goldCountMax; }
            set { goldCountMax = value; }
        }


        public int GoldToGetPerTime
        {
            get { return goldToGetPerTime; }
            set { goldToGetPerTime = value; }
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!isActive)
            {
                goldText.Text = "";
                return;
            }
            goldText.Position = new Vector2(position.X - 8, position.Y - 8);
            goldText.Text = goldCount.ToString();
            if (peonState == PeonState.Moving)
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
                            peonState = PeonState.Idle;
                            return;
                        }
                    }
                    else
                    {
                        peonState = PeonState.Idle;
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
                        peonState = PeonState.Idle;
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
                    peonState = PeonState.Idle;
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
            else if (peonState == PeonState.Idle)
            {
                if (peonObjective == PeonObjective.GoToBuild)
                {
                    curNode = graph.GetNode(position);
                    foreach (Edge edge in curNode.Neighbors)
                    {
                        tempNode = edge.GetNeighbor(curNode);
                        //searches through the neighbor nodes to make sure it isnt on the wall or next to something else.
                        if (!tempNode.IsBlocked)
                        {
                            canBuild = true;
                            foreach (Edge innerEdge in tempNode.Neighbors)
                            {
                                if (innerEdge.GetNeighbor(tempNode).IsBlocked && innerEdge.GetNeighbor(tempNode) != curNode)
                                {
                                    canBuild = false;
                                }
                            }
                            if (canBuild)
                            {
                                agent.IsAgentGoingToBuildSomething = false;
                                if (buildingType == Building.BuildingType.GoldRefinery)
                                {
                                    GoldRefinery newRefinery = new GoldRefinery(game, tempNode, graph, agent);
                                    Game.Components.Add(newRefinery);
                                    peonObjective = PeonObjective.Build;
                                    return;
                                }
                                else if (buildingType == Building.BuildingType.Barracks)
                                {
                                    Barracks newBarracks = new Barracks(game, tempNode, graph, agent);
                                    Game.Components.Add(newBarracks);
                                    peonObjective = PeonObjective.Build;
                                    return;
                                }
                            }
                        }
                    }
                }
                graph.SetNodeBlocked(position);
            }
            else if (peonState == PeonState.Mining)
            {
                graph.SetNodeBlocked(position);

                if (goldMine.GoldRemaining == 0)
                {
                    peonState = PeonState.Idle;
                }
                else if (goldCount != goldCountMax)
                {
                    timer += gameTime.ElapsedGameTime.Milliseconds;
                    if (timer >= timeToGetGold)
                    {
                        timer = 0;
                        if (goldToGetPerTime > goldMine.GoldRemaining)
                        {
                            goldCount += goldMine.TakeResource(goldMine.GoldRemaining);
                            peonState = PeonState.Idle;
                        }
                        if (goldCount + goldToGetPerTime > goldCountMax)
                        {
                            goldCount += goldMine.TakeResource(goldCountMax - goldCount);
                        }
                        else
                        {
                            goldCount += goldMine.TakeResource(goldToGetPerTime);
                        }
                    }
                }
                else
                {
                    peonState = PeonState.Idle;
                }
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
            peonState = PeonState.Moving;
            graph.UnblockNode(position);
        }

        public void MineFromGoldMine(GoldMine goldMine)
        {
            this.goldMine = goldMine;
            Node curNode = graph.GetNode(position);
            foreach (Edge edge in curNode.Neighbors)
            {
                if (edge.GetNeighbor(curNode) == goldMine.GetNode)
                {
                    peonState = PeonState.Mining;
                    return;
                }
            }

            MoveToLocation(new Vector2(goldMine.GetNode.Position.X * Game1.WINDOW_WIDTH / (Game1.WINDOW_WIDTH / Game1.graphSize), goldMine.GetNode.Position.Y * Game1.WINDOW_HEIGHT / (Game1.WINDOW_HEIGHT / Game1.graphSize)));
        }

        public void BuildBuilding(Building.BuildingType buildingType, Vector2 position)
        {
            peonObjective = PeonObjective.GoToBuild;
            this.buildingType = buildingType;
            MoveToLocation(position);
        }
    }
}
