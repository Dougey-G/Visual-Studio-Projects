using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CatAndMouseGame
{
    public class Bar : Sprite
    {
        protected Rectangle rectangle;
        protected float offset;

        public Bar(Game game, float offset)
            : base(game)
        {
            game.Components.Add(this);
            this.offset = offset;
            base.spriteName = "cooldown";
            rectangle = new Rectangle();
            base.LoadContent();
        }

        public float Offset
        {
            get { return offset; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }
    }
}
