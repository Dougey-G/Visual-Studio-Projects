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
    class Unit : DrawableGameComponent
    {
        #region fields

        protected Texture2D sprite;
        protected Color renderColor;
        protected float rotation = 0;
        protected string spriteName;
        protected Vector2 position;
        protected Vector2 velocity;
        protected Vector2 oldVelocity;
        protected Vector2 orientation;
        protected Vector2 acceleration;
        protected float distance;
        protected float stopRadius;
        protected float slowRadius;
        protected float speed;
        protected float maxSpeed;
        protected float maxAccel;
        protected Graph graph;
        protected Stack<Vector2> targetPath = new Stack<Vector2>();
        protected Vector2 curTarget;
        protected Game game;
        protected AIAgent agent;
        protected float health;
        protected float maxHealth;
        protected bool isActive = true;

        SpriteBatch spriteBatch;
        #endregion

        public Unit(Game game, Graph graph, AIAgent agent, Color renderColor)
            : base(game)
        {
            this.graph = graph;
            this.game = game;
            this.agent = agent;
            this.renderColor = renderColor;
            LoadContent();
        }

        #region properties
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        #endregion

        #region methods
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
            if (!isActive)
            {
                return;
            }
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, position, null, renderColor, rotation, new Vector2(sprite.Width / 2, sprite.Height / 2), Vector2.One, SpriteEffects.None, 0);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        #endregion
    }
}
