using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GridSearching
{
    class Timer
    {
        float time;
        float timer;
        bool isFinished = true;
    
        public Timer(float time)
        {
            this.time = time;
        }

        public void Update(GameTime gameTime)
        {
            if (!isFinished)
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer >= time)
                {
                    isFinished = true;
                }
            }
        }

        public bool IsFinished
        {
            get { return isFinished; }
        }

        public void Reset()
        {
            isFinished = false;
            timer = 0;
        }
    }
}
