using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleRTS
{
    class GoldRefinery : Building
    {
        float distanceToConsiderClose = 50f;
        AIAgent agent;
        TextRepresentation healthText;

        public GoldRefinery(Game game, Node node, Graph graph, AIAgent agent)
            : base(game, graph, agent, agent.RenderColor)
        {
            buildingType = BuildingType.GoldRefinery;
            spriteName = "goldRefinery";
            this.agent = agent;
            this.node = node;
            this.node.IsBlocked = true;
            this.Position = node.Position * Game1.graphSize;
            this.Position = new Vector2(Position.X + 16, Position.Y + 16);
            agent.buildings.UnfinishedBuildings.Add(this);
            timeToCreate = 45000; //45 seconds
            maxHealth = 1000;
            health = 1;
            agent.GoldCount -= 400;
            healthText = new TextRepresentation(game, Position);
            Game.Components.Add(healthText);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            int healthString = (int)health;
            healthText.Text = healthString.ToString() + " / " + maxHealth.ToString();
            if (isOperational)
            {
                return;
            }
            else 
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

            if (isBeingWorkedOn)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer >= timeToCreate)
                {
                    timer = timeToCreate;
                    isOperational = true;
                    isBeingWorkedOn = false;
                    agent.buildings.UnfinishedBuildings.Remove(this);
                    agent.buildings.Refinery = this;
                    foreach (Peon peon in agent.units.Peons)
                    {
                        peon.GoldCountMax = 60;
                        peon.GoldToGetPerTime = 6;
                        //we are completed, give the peon back to the ai controller.
                        if (peon.peonObjective == Peon.PeonObjective.Build && Vector2.Distance(peon.Position, Position) < distanceToConsiderClose)
                        {
                            peon.peonObjective = Peon.PeonObjective.Idle;
                        }
                    }
                }
                health = maxHealth * (timer / timeToCreate);
            }
        }
    }
}
