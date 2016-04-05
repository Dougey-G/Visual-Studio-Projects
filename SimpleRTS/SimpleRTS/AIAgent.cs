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
    class AIAgent : GameComponent
    {
        protected enum AIState { Passive, Aggressive, Defensive };
        protected Color renderColor;
        protected AIState aiState;
        public Units units;
        public Buildings buildings;
        protected float distanceToNearestGoldMine = 999999f;
        protected int goldCount = 100;
        protected TextRepresentation goldText;
        protected List<GoldMine> goldMines;
        protected bool isAgentGoingToBuildSomething = false;
        protected Vector2 buildingLocation;

        public AIAgent(Game game, Node location, Graph graph, List<GoldMine> goldMines, Color renderColor)
            : base(game)
        {
            this.renderColor = renderColor;
            Base myBase = new Base((Game1)game, location, graph, this);
            game.Components.Add(myBase);
            units = new Units();
            buildings = new Buildings();
            buildings.GetBase = myBase;
            buildings.GetGoldMines = goldMines;
            goldText = new TextRepresentation(game, buildings.GetBase.Position);
            Game.Components.Add(goldText);
            this.goldMines = goldMines;
            foreach (GoldMine mine in goldMines)
            {
                if (Vector2.Distance(mine.Position, buildings.GetBase.Position) < distanceToNearestGoldMine)
                {
                    distanceToNearestGoldMine = Vector2.Distance(mine.Position, buildings.GetBase.Position);
                    buildings.NearestGoldMine = mine;
                }
            }
        }


        public Color RenderColor
        {
            get { return renderColor; }
        }

        public int GoldCount
        {
            get { return goldCount; }
            set { goldCount = value; }
        }

        public bool IsAgentGoingToBuildSomething
        {
            get { return isAgentGoingToBuildSomething; }
            set { isAgentGoingToBuildSomething = value; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            goldText.Text = goldCount.ToString();
            if (buildings.NearestGoldMine.GoldRemaining == 0)
            {
                distanceToNearestGoldMine = 99999;
                foreach (GoldMine mine in goldMines)
                {
                    if (Vector2.Distance(mine.Position, buildings.GetBase.Position) < distanceToNearestGoldMine && mine.GoldRemaining != 0)
                    {
                        distanceToNearestGoldMine = Vector2.Distance(mine.Position, buildings.GetBase.Position);
                        buildings.NearestGoldMine = mine;
                    }
                }
            }

            if (units.Peons.Count < 8 && goldCount >= 50 && !buildings.GetBase.IsWorking)
            {
                buildings.GetBase.CreatePeon();
                goldCount -= 50;
            }
            foreach (Peon peon in units.Peons)
            {
                if (peon.peonObjective == Peon.PeonObjective.Build || peon.peonObjective == Peon.PeonObjective.GoToBuild)
                {
                    //Do nothing because they are preoccupied
                }
                else if (peon.peonObjective == Peon.PeonObjective.Idle && peon.peonState == Peon.PeonState.Idle && peon.GoldCount < peon.GoldCountMax)
                {
                    if (goldCount >= 400 && !buildings.IsBuildingInProgress(Building.BuildingType.GoldRefinery) && buildings.Refinery == null && !isAgentGoingToBuildSomething)
                    {
                        if (buildings.GetBase.Position.Y + (Game1.graphSize * 3) < Game1.WINDOW_HEIGHT)
                        {
                            buildingLocation = new Vector2(buildings.GetBase.Position.X, buildings.GetBase.Position.Y + (Game1.graphSize * 2));
                        }
                        else
                        {
                            buildingLocation = new Vector2(buildings.GetBase.Position.X, buildings.GetBase.Position.Y - (Game1.graphSize * 4));
                        }
                       
                        peon.BuildBuilding(Building.BuildingType.GoldRefinery, buildingLocation);
                        isAgentGoingToBuildSomething = true;
                    }
                    else if (goldCount >= 500 && !buildings.IsBuildingInProgress(Building.BuildingType.Barracks) && buildings.Barracks.Count == 0 && !isAgentGoingToBuildSomething)
                    {
                        if (buildings.GetBase.Position.X + (Game1.graphSize * 3) < Game1.WINDOW_WIDTH)
                        {
                            buildingLocation = new Vector2(buildings.GetBase.Position.X + (Game1.graphSize * 4), buildings.GetBase.Position.Y);
                        }
                        else
                        {
                            buildingLocation = new Vector2(buildings.GetBase.Position.X - (Game1.graphSize * 4), buildings.GetBase.Position.Y);
                        }

                        peon.BuildBuilding(Building.BuildingType.Barracks, buildingLocation);
                        isAgentGoingToBuildSomething = true;
                    }
                    else
                    {
                        peon.MineFromGoldMine(buildings.NearestGoldMine);
                    }
                }
                else if (peon.peonState == Peon.PeonState.Idle && peon.GoldCount == peon.GoldCountMax)
                {
                    if (Vector2.Distance(peon.Position, buildings.GetBase.Position) < 50)
                    {
                        goldCount += peon.GoldCount;
                        peon.GoldCount = 0;
                    }
                    peon.MoveToLocation(buildings.GetBase.Position);
                }
            }
        }


    }
}
