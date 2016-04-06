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
    class GoldMine : Building
    {
        int resourceRemaining;
        int maxResource = 5000;
        TextRepresentation goldText;

        public GoldMine(Game game, Node node, Graph graph, AIAgent agent)
            :base (game, node, graph, agent, Color.White)
        {
            spriteName = "goldMine";
            this.node = node;
            this.node.IsBlocked = true;
            this.Position = node.Position * Game1.graphSize;
            this.Position = new Vector2(Position.X + 16, Position.Y + 16);
            resourceRemaining = maxResource;
            goldText = new TextRepresentation(game, Position);
            game.Components.Add(goldText);
        }

        public int GoldRemaining
        {
            get { return resourceRemaining; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            goldText.Text = resourceRemaining.ToString();
        }

        public int TakeResource(int amount)
        {
            if (resourceRemaining >= amount)
            {
                resourceRemaining -= amount;
                return amount;
            }
            else if (resourceRemaining > 0)
            {
                int remaining = resourceRemaining;
                resourceRemaining = 0;
                return remaining;
            }
            else
            {
                return 0;
            }
        }
    }
}
