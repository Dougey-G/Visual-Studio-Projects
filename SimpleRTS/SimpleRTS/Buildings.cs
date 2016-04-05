﻿using System;
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
    class Buildings
    {
        Base myBase;
        List<Building> unfinishedBuildings = new List<Building>();
        List<Barracks> barracks = new List<Barracks>();
        List<GoldMine> goldMines = new List<GoldMine>();
        GoldMine nearestGoldMine;
        GoldRefinery refinery;

        public Buildings() { }

        public Base GetBase
        {
            get { return myBase; }
            set { myBase = value; }
        }

        public List<GoldMine> GetGoldMines
        {
            get { return goldMines; }
            set { goldMines = value; }
        }

        public GoldMine NearestGoldMine
        {
            get { return nearestGoldMine; }
            set { nearestGoldMine = value; }
        }

        public GoldRefinery Refinery
        {
            get { return refinery; }
            set { refinery = value; }
        }

        public List<Building> UnfinishedBuildings
        {
            get { return unfinishedBuildings; }
        }

        public List<Barracks> Barracks
        {
            get { return barracks; }
        }


        public bool IsBuildingInProgress(Building.BuildingType buildingType)
        {
            foreach (Building building in unfinishedBuildings)
            {
                if (building.Type == buildingType)
                {
                    return true;
                }
            }
            return false;
        }


    }
}