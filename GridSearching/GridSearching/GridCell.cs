using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C3.XNA;
using Microsoft.Xna.Framework.Content;

namespace GridSearching
{
    class GridCell : DrawableGameComponent
    { 
        SpriteBatch spriteBatch;
        Vector2 position;
        Vector2 size;
        Color color;
        bool isBlocked = false;

        public GridCell(Game game, Vector2 position, Vector2 size)
            : base(game)
        {
            this.position = position;
            this.size = size;
            color = Color.CornflowerBlue;
            LoadContent();
        }

        public bool IsBlocked
        {
            get { return isBlocked; }
            set
            {
                isBlocked = value;
                if (isBlocked)
                {
                    color = Color.Black;
                }
                else
                {
                    color = Color.CornflowerBlue;
                }
            }
        }

        public bool IsVisited
        {
            set
            {
                if (value)
                {
                    ChangeColor = Color.Orange;
                }
                else
                {
                    ChangeColor = Color.CornflowerBlue;
                }
            }
        }

        public void Test()
        {
            if (color == Color.CornflowerBlue)
            {
                color = Color.Red;
            }
            else
            {
                color = Color.CornflowerBlue;
            }

        }

        public Color ChangeColor
        {
            set
            {
                if (color != Color.Red && color != Color.Green)
                    color = value;
                else if (value == Color.CornflowerBlue)
                {
                    color = value;
                }
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.FillRectangle(position, size, color);
            spriteBatch.DrawRectangle(position, size, Color.Black);    
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
