using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleRTS
{
    class Barracks : Building
    {
        float timeToCreateSoldier = 20000; //20 seconds
        bool isWorking = false;
        float distanceToConsiderClose = 50f;

        Vector2 locationToPlaceSoldier;
        AIAgent agent;

        public Barracks(Game game, Node node, Graph graph, AIAgent agent)
            :base (game, node, graph, agent, agent.RenderColor)
        {
            buildingType = BuildingType.Barracks;
            spriteName = "barracks";
            this.agent = agent;
            this.node = node;
            isOperational = false;
            this.node.IsBlocked = true;
            timeToCreate = 60000; //60 seconds
            maxHealth = 1500;
            health = 1;
            agent.GoldCount -= 500;
            agent.buildings.UnfinishedBuildings.Add(this);
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

            if (!isOperational && !isBeingWorkedOn)
            {
                foreach (Peon peon in agent.units.Peons)
                {
                    //If the peon is in a working state and is near the goldrefinery
                    if (peon.peonObjective == Peon.PeonObjective.Build && Vector2.Distance(peon.Position, Position) < distanceToConsiderClose)
                    {
                        isBeingWorkedOn = true;
                        break;
                    }
                    isBeingWorkedOn = false;
                }
            }
            else if (!isOperational && isBeingWorkedOn)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer >= timeToCreate)
                {
                    timer = timeToCreate;
                    isOperational = true;
                    isBeingWorkedOn = false;
                    agent.buildings.UnfinishedBuildings.Remove(this);
                    agent.buildings.Barracks.Add(this);
                    foreach (Peon peon in agent.units.Peons)
                    {
                        //we are completed, give the peon back to the ai controller.
                        if (peon.peonObjective == Peon.PeonObjective.Build && Vector2.Distance(peon.Position, Position) < distanceToConsiderClose)
                        {
                            peon.peonObjective = Peon.PeonObjective.Idle;
                        }
                    }
                }
                health = maxHealth * (timer / timeToCreate);
                if (isOperational)
                {
                    timer = 0;
                }
            }
            else if (isWorking && isOperational)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                locationToPlaceSoldier = node.Position;

                if (timer >= timeToCreateSoldier)
                {
                    foreach (Edge edge in node.Neighbors)
                    {
                        if (!edge.GetNeighbor(node).IsBlocked)
                        {
                            locationToPlaceSoldier = new Vector2(edge.GetNeighbor(node).Position.X * Game1.WINDOW_WIDTH / (Game1.WINDOW_WIDTH / Game1.graphSize) + 16, edge.GetNeighbor(node).Position.Y * Game1.WINDOW_HEIGHT / (Game1.WINDOW_HEIGHT / Game1.graphSize) + 16);
                        }
                    }

                    //This is what makes sure that the base creates the peon without wasting the money if their is no place to spawn them.
                    if (locationToPlaceSoldier == node.Position)
                    {
                        return;
                    }

                    agent.units.AddSoldier(new Soldier(game, locationToPlaceSoldier, graph, agent));
                    game.Components.Add(agent.units.Soldiers.Last());
                    timer = 0;
                    isWorking = false;
                }
            }


        }

        public void CreateSoldier()
        {
            isWorking = true;
        }
    }
}
