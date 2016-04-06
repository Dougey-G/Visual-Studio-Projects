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
    class Units
    {
        List<Peon> peons = new List<Peon>();
        List<Soldier> soldiers = new List<Soldier>();

        public Units() { }

        public List<Peon> Peons
        {
            get { return peons; }
        }

        public List<Soldier> Soldiers
        {
            get { return soldiers; }
        }

        public void DisableAll()
        {
            foreach (Peon p in peons)
            {
                p.IsActive = false;
            }

            foreach (Soldier s in soldiers)
            {
                s.IsActive = false;
            }
        }


        public void AddPeon(Peon peon)
        {
            peons.Add(peon);
        }

        public void AddSoldier(Soldier soldier)
        {
            soldiers.Add(soldier);
        }
    }
}
