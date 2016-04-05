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
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region fields
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        bool isAvailable = true;
        bool isAvailable2 = true;

        public const int graphSize = 32; //The amount of pixels per grid unit
        Graph graph;
        public const int WINDOW_WIDTH = 1024;
        public const int WINDOW_HEIGHT = 768;

        Peon peon;
        Peon peon2;
        List<GoldMine> goldMines = new List<GoldMine>();
        Base base1;
        AIAgent agent;

        Random random = new Random();
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.ApplyChanges();

            //Creates the graph
            graph = new Graph(WINDOW_WIDTH / graphSize, WINDOW_HEIGHT / graphSize);
            goldMines.Add(new GoldMine(this, graph.GetNode(new Vector2(random.Next(0 + graphSize * 3, WINDOW_WIDTH - graphSize * 3), random.Next(0 + graphSize * 3, WINDOW_HEIGHT - graphSize * 3))), graph, null));
            Components.Add(goldMines.Last());
            goldMines.Add(new GoldMine(this, graph.GetNode(new Vector2(random.Next(0 + graphSize * 3, WINDOW_WIDTH - graphSize * 3), random.Next(0 + graphSize * 3, WINDOW_HEIGHT - graphSize * 3))), graph, null));
            Components.Add(goldMines.Last());
            goldMines.Add(new GoldMine(this, graph.GetNode(new Vector2(random.Next(0 + graphSize * 3, WINDOW_WIDTH - graphSize * 3), random.Next(0 + graphSize * 3, WINDOW_HEIGHT - graphSize * 3))), graph, null));
            Components.Add(goldMines.Last());
            goldMines.Add(new GoldMine(this, graph.GetNode(new Vector2(random.Next(0 + graphSize * 3, WINDOW_WIDTH - graphSize * 3), random.Next(0 + graphSize * 3, WINDOW_HEIGHT - graphSize * 3))), graph, null));
            Components.Add(goldMines.Last());
            agent = new AIAgent(this, graph.GetNode(new Vector2(0, 0)), graph, goldMines, Color.Blue);
            Components.Add(agent);
            agent = new AIAgent(this, graph.GetNode(new Vector2(0, WINDOW_HEIGHT - 1)), graph, goldMines, Color.Green);
            Components.Add(agent);
            agent = new AIAgent(this, graph.GetNode(new Vector2(WINDOW_WIDTH - 1, 0)), graph, goldMines, Color.White);
            Components.Add(agent);
            agent = new AIAgent(this, graph.GetNode(new Vector2(WINDOW_WIDTH - 1, WINDOW_HEIGHT - 1)), graph, goldMines, Color.Red);
            Components.Add(agent);

            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            MouseState mouse = Mouse.GetState();

            if (isAvailable && mouse.LeftButton == ButtonState.Pressed)
            {
                isAvailable = false;
            }

            else if (mouse.LeftButton == ButtonState.Released)
            {
                isAvailable = true;
            }

            if (isAvailable2 && mouse.RightButton == ButtonState.Pressed)
            {
                isAvailable2 = false;
            }
            else if (mouse.RightButton == ButtonState.Released)
            {
                isAvailable2 = true;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); 

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
