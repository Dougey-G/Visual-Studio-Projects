using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections;
using System.Reflection;
using GDD3400_RTS_Lib;

namespace GDD3400_PlanningAgent_Lib
{
    // All of your code needs to belong to the PlanningAgent class or
    // needs to be a privately defined class (as in AnotherClass below)
    // you cannot define any other public classes in this library or
    // else the DLL launching will fail.
    
    // Define any other classes with the default protection so that
    // the DLL does not recognize them but you may use them within
    // the PlanningAgent class

    /// <summary>
    /// Enemy class: Stores all the information about an enemy so we can decide how to proceed
    /// </summary>
    class Enemy
    {
        float estimatedGoldIncomePerSecond;
        int resourceCount = int.MaxValue;
        ResourceSprite closestMine;
        UnitSprite mainBase;
        GameState gameState;
        public List<UnitSprite> units;
        public List<UnitSprite> peons;
        public List<UnitSprite> soldiers;
        public List<UnitSprite> bases;
        public List<UnitSprite> barracks;
        public List<UnitSprite> refineries;

        int agentNbr;

        public Enemy(int agentNbr){ this.agentNbr = agentNbr; }

        public void Update(GameTime gameTime, GameState gameState)
        {
            this.gameState = gameState;

            // If the gameState is null, do nothing, this is bad...
            if (gameState == null)
                return;

            // Identify all of my units
            units = gameState.Units.Where(y => y.AgentNbr == AgentNbr).ToList();

            // Identify all of my peons
            peons = units.Where(y => y.UnitType == UnitType.PEON).ToList();

            // Identify all of my soldiers
            soldiers = units.Where(y => y.UnitType == UnitType.SOLDIER).ToList();

            // Identify all of my barracks
            barracks = units.Where(y => y.UnitType == UnitType.BARRACKS).ToList();

            // Identify all of my bases
            bases = units.Where(y => y.UnitType == UnitType.BASE).ToList();

            //// Identify all of my refineries
            refineries = units.Where(y => y.UnitType == UnitType.REFINERY).ToList();

            if (bases.Count > 0)
            {
                mainBase = bases[0];
            }

            FindClosestMine();
            EstimateGoldIncome();
        }

        public int AgentNbr
        {
            get { return agentNbr; }
        }

        public float EstimatedGoldIncomePerSecond
        {
            get { return estimatedGoldIncomePerSecond; }
        }

        void EstimateGoldIncome()
        {
            float distanceToCurrentMine = 0;
            if (closestMine != null && mainBase != null)
            {
                distanceToCurrentMine = Vector2.Distance(closestMine.Position, mainBase.Position) / GDD3400_RTS_Lib.Constants.CELL_SIZE;
            }
            else
            {
                estimatedGoldIncomePerSecond = 0;
            }

            float timeToGather;
            if (peons.Count != 0 && closestMine.Value != 0)
            {
                timeToGather = ((distanceToCurrentMine / peons[0].Speed) + peons[0].MiningTime);
                estimatedGoldIncomePerSecond = (100 / timeToGather) * peons.Count;
            }
            else
            {
                estimatedGoldIncomePerSecond = 0;
            }
        }

        private void FindClosestMine()
        {
            // If this is the first time or a mine is destroyed (Resource count has decreased
            // Find the closest mine
            if (closestMine == null || gameState.Resources.Count < resourceCount)
            {
                float closestMineDist = float.MaxValue;
                foreach (ResourceSprite mineResource in gameState.Resources.Where(x => x.ResourceType == ResourceType.MINE).ToList())
                {
                    foreach (UnitSprite baseUnit in bases)
                    {
                        float mineDist = Vector2.Distance(mineResource.Position, baseUnit.Position);
                        if (mineDist < closestMineDist)
                        {
                            closestMineDist = mineDist;
                            closestMine = mineResource;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Main class for the PlanningAgent, this is the only one
    /// that can be public.  Inherits from the Agent class
    /// </summary>
    public class PlanningAgent : Agent
    {
        int resourceCount = int.MaxValue;
        float estimatedGoldIncomePerSecond;
        ResourceSprite closestMine = null;
        Random rand = new Random();
        GameState gameState;
        UnitSprite mainBase;

        List<UnitSprite> myUnits;

        List<UnitSprite> myPeons;
        List<UnitSprite> mySoldiers;
        List<UnitSprite> myBases;
        List<UnitSprite> myBarracks;
        List<UnitSprite> myRefineries;

        List<Enemy> enemies = new List<Enemy>();
        bool ranOnce = false;

        public PlanningAgent()
        {

        }

        #region Private Methods

        private void EstimateGoldIncome()
        {
            float distanceToCurrentMine = 0;
            if (closestMine != null && mainBase != null)
            {
                distanceToCurrentMine = Vector2.Distance(closestMine.Position, mainBase.Position) / GDD3400_RTS_Lib.Constants.CELL_SIZE;
            }
            else
            {
                estimatedGoldIncomePerSecond = 0;
            }

            float timeToGather;
            if (myPeons.Count != 0 && closestMine.Value != 0)
            {
                timeToGather = ((distanceToCurrentMine / myPeons[0].Speed) + myPeons[0].MiningTime);
                estimatedGoldIncomePerSecond = (100 / timeToGather) * myPeons.Count;
            }
            else
            {
                estimatedGoldIncomePerSecond = 0;
            }
        }

        private void RunOnce()
        {
            foreach (UnitSprite b in gameState.Units.Where(y => y.AgentNbr != AgentNbr && y.UnitType == UnitType.BASE).ToList())
            {
                enemies.Add(new Enemy(b.AgentNbr));
            }
        }

        private void FindClosestMine()
        {
            // If this is the first time or a mine is destroyed (Resource count has decreased
            // Find the closest mine
            if (closestMine == null || gameState.Resources.Count < resourceCount)
            {
                float closestMineDist = float.MaxValue;
                foreach (ResourceSprite mineResource in gameState.Resources.Where(x => x.ResourceType == ResourceType.MINE).ToList())
                {
                    foreach (UnitSprite baseUnit in myBases)
                    {
                        float mineDist = Vector2.Distance(mineResource.Position, baseUnit.Position);
                        if (mineDist < closestMineDist)
                        {
                            closestMineDist = mineDist;
                            closestMine = mineResource;
                        }
                    }
                }
            }
        }

        private Point FindRandomOpenCellToBuildWithinRange(int minXRange, int maxXRange, int minYRange, int maxYRange)
        {
            Point p;
            int i = 0;
            int j = 0;
            Point gridPosition = Tools.WorldToGrid(myBases[0].Position);

            do
            {
                i = gridPosition.X + (rand.Next(maxXRange - minXRange) + minXRange) * (rand.Next(2) > 0 ? -1 : 1);
                j = gridPosition.Y + (rand.Next(maxYRange - minYRange) + minYRange) * (rand.Next(2) > 0 ? -1 : 1);
                p = new Point(i, j);
            } while (!Tools.IsValidGridLocation(p) || !gameState.Grid[i, j].IsBuildable);// || !gameState.Grid[i, j].IsWalkable );

            return p;
        }

        private void ProcessPeons()
        {
            // For each peon
            foreach (UnitSprite unit in myPeons)
            {
                if (unit.CurrentAction == UnitAction.IDLE)
                {
                    // If we have enough gold and need a barracks, build a barracks
                    if (Gold > Constants.COST[(int)UnitType.BARRACKS]
                        && myBarracks.Count < 3)
                    {
                        Point toBuild = FindRandomOpenCellToBuildWithinRange(
                            2, Constants.GRID_HEIGHT / 2 - 4, 
                            2, Constants.GRID_WIDTH / 2 - 6);
                        Build(unit, toBuild, UnitType.BARRACKS);
                    }
                    //// If we have enough gold and need a refinery, build a refinery
                    //else if (Gold > Constants.COST[(int)UnitType.REFINERY]
                    //    && myRefineries.Count < 1 && myBarracks.Count >= 3)
                    //{
                    //    Point toBuild = FindRandomOpenCellToBuildWithinRange(
                    //        2, Constants.GRID_HEIGHT / 2 - 4, 
                    //        2, Constants.GRID_WIDTH / 2 - 6);
                    //    Build(unit, toBuild, UnitType.REFINERY);
                    //}
                    // Ohterwise, just mine
                    else if (mainBase != null && closestMine.Value > 0)
                    {
                        Gather(unit, closestMine, mainBase);
                    }
                }
            }
        }
        private void ProcessBases()
        {
            // Process the Base
            foreach (UnitSprite unit in myBases)
            {
                if (unit.CurrentAction == UnitAction.IDLE && myPeons.Count < 15
                    && Gold >= Constants.COST[(int)UnitType.PEON])
                {
                    Train(unit, UnitType.PEON);
                }
            }
        }

        private void ProcessBarracks()
        {
            // Process the Barracks
            foreach (UnitSprite unit in myBarracks)
            {
                if (unit.CurrentAction == UnitAction.IDLE && Gold >= Constants.COST[(int)UnitType.SOLDIER])
                {
                    Train(unit, UnitType.SOLDIER);
                }
            }
        }

        private void ProcessSoldiers()
        {
            // For each soldier, determine what they should attack
            foreach (UnitSprite unit in mySoldiers)
            {
                if (unit.CurrentAction == UnitAction.IDLE)
                {
                    
                }
            }
        }

        #endregion

        #region Public Methods

        public override void Update(GameTime gameTime, GameState gameState)
        {
            this.gameState = gameState;

            // If the gameState is null, do nothing, this is bad...
            if (gameState == null)
                return;

            // Identify all my units
            myUnits = gameState.Units.Where(y => y.AgentNbr == AgentNbr).ToList();

            // Identify all of my peons
            myPeons = myUnits.Where(y => y.UnitType == UnitType.PEON).ToList();

            // Identify all of my soldiers
            mySoldiers = myUnits.Where(y => y.UnitType == UnitType.SOLDIER).ToList();

            // Identify all of my barracks
            myBarracks = myUnits.Where(y => y.UnitType == UnitType.BARRACKS).ToList();

            // Identify all of my bases
            myBases = myUnits.Where(y => y.UnitType == UnitType.BASE).ToList();

            //// Identify all of my refineries
            myRefineries = myUnits.Where(y => y.UnitType == UnitType.REFINERY).ToList();

            if (!ranOnce)
            {
                ranOnce = true;
                RunOnce();
            }

            foreach (Enemy e in enemies)
            {
                e.Update(gameTime, gameState);
            }

            if (myBases.Count > 0)
            {
                mainBase = myBases[0];
            }

            

            FindClosestMine();

            EstimateGoldIncome();

            ProcessPeons();

            ProcessSoldiers();

            ProcessBarracks();

            ProcessBases();

            // Use this debugger to display any messages you want to the
            // primary XNA window, just add a string to this list.  As an
            // alternative, you can of course print anything to the Console
            // using WriteLine() as you usually would, but that can get
            // confusing as you might not know which agent the output
            // belongs to!
            if (Constants.SHOW_MESSAGES)
            {
                lock(debugger.messages)
                {
                    debugger.messages = new List<string>();
                    debugger.messages.Add("Gold       " + Gold);
                    debugger.messages.Add("Peons      " + myPeons.Count);
                    debugger.messages.Add("Soldiers   " + mySoldiers.Count);
                    debugger.messages.Add("Barracks   " + myBarracks.Count);
                    debugger.messages.Add("Bases      " + myBases.Count);
                    debugger.messages.Add("Refineries " + myRefineries.Count);
                    debugger.messages.Add("Income     " + estimatedGoldIncomePerSecond);
                }
            }

        }

        #endregion
    }
}
