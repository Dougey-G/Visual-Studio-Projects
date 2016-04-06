using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleRTS
{
    class Base : Building
    {
        float timer = 0;
        float timeToCreatePeon = 10000; // 10 seconds
        bool isWorking = false;

        Vector2 locationToPlacePeon;
        AIAgent agent;

        public Base(Game1 game, Node node, Graph graph, AIAgent agent)
            :base(game, node, graph, agent, agent.RenderColor)
        {
            buildingType = BuildingType.Base;
            spriteName = "base";
            this.agent = agent;
            this.node = node;
            isOperational = true;
            this.node.IsBlocked = true;
            this.Position = node.Position * Game1.graphSize;
            this.Position = new Vector2(Position.X + 16, Position.Y + 16);
            health = 1000;
            maxHealth = 1000;
        }

        public bool IsWorking
        {
            get { return isWorking; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (!isActive)
            {
                healthText.Text = "";
                return;
            }
            if (isWorking)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                locationToPlacePeon = node.Position;

                if (timer >= timeToCreatePeon)
                {
                    foreach (Edge edge in node.Neighbors)
                    {
                        if (!edge.GetNeighbor(node).IsBlocked)
                        {
                            locationToPlacePeon = new Vector2(edge.GetNeighbor(node).Position.X * Game1.WINDOW_WIDTH / (Game1.WINDOW_WIDTH / Game1.graphSize) + 16, edge.GetNeighbor(node).Position.Y * Game1.WINDOW_HEIGHT / (Game1.WINDOW_HEIGHT / Game1.graphSize) + 16);
                        }
                    }

                    //This is what makes sure that the base creates the peon without wasting the money if their is no place to spawn them.
                    if (locationToPlacePeon == node.Position)
                    {
                        return;
                    }

                    agent.units.AddPeon(new Peon(game, locationToPlacePeon, graph, agent));
                    game.Components.Add(agent.units.Peons.Last());
                    if (agent.buildings.Refinery != null)
                    {
                        agent.units.Peons.Last().GoldCountMax = 60;
                        agent.units.Peons.Last().GoldToGetPerTime = 6;
                    }

                    timer = 0;
                    isWorking = false;
                }
            }
        }

        public void CreatePeon()
        {
            isWorking = true;
        }
    }
}
