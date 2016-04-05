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
    class Building : DrawableGameComponent
    {
        public enum BuildingType { GoldMine, Barracks, Base, GoldRefinery };
        protected BuildingType buildingType;

        protected Color renderColor;
        protected float health;
        protected float maxHealth;
        protected Vector2 location;
        protected Node node;
        protected Graph graph;
        protected string spriteName;
        protected Texture2D sprite;
        protected Game game;
        protected bool isOperational = false;
        protected float timer = 0;
        protected float timeToCreate;
        protected bool isBeingWorkedOn = false;


        SpriteBatch spriteBatch;

        public Building(Game game, Graph graph, AIAgent agent, Color renderColor)
            : base(game)
        {
            this.graph = graph;
            this.renderColor = renderColor;
            this.game = game;
            LoadContent();
        }

        public Vector2 Position
        {
            get { return location; }
            set { location = value; }
        }

        public bool IsOperational
        {
            get { return isOperational; }
        }

        public BuildingType Type
        {
            get { return buildingType; }
        }

        public Node GetNode
        {
            get { return node; }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Initialize()
        {
            sprite = Game.Content.Load<Texture2D>(spriteName);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, location, null, renderColor, 0, new Vector2(sprite.Width / 2, sprite.Height / 2), Vector2.One, SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void TakeDamage(float amount)
        {
            if (isOperational)
            {
                health -= amount;
            }
            else
            {
                health -= amount;
                timer = (health / maxHealth) * timeToCreate;
            }
        }
    }
}