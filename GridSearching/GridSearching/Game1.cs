using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using C3.XNA;
using System;

namespace GridSearching
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        const int gridSize = 32;
        GridCell[,] grid;
        Graph graph;
        Random random = new Random();
        bool canReset = true;
        bool canClickAgain = true;
        Vector2 startPosition;
        Vector2 destination;


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
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            grid = new GridCell[graphics.PreferredBackBufferWidth / gridSize, graphics.PreferredBackBufferHeight / gridSize];
            for (int i = 0; i < graphics.PreferredBackBufferWidth / gridSize; i++)
            {
                for (int j = 0; j < graphics.PreferredBackBufferHeight / gridSize; j++)
                {
                    grid[i, j] = new GridCell(this, new Vector2(i * gridSize, j * gridSize), new Vector2(gridSize, gridSize));
                    Components.Add(grid[i, j]);
                }
            }
            StartRandomSearch();
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

            if (canClickAgain && mouse.LeftButton == ButtonState.Pressed)
            {
                canClickAgain = false;
                SetDestination();
                ResetGridForBreadth(true);
            }
            else if (canClickAgain && mouse.RightButton == ButtonState.Pressed)
            {
                canClickAgain = false;
                CreateObstacle();
            }
            else if (mouse.LeftButton == ButtonState.Released && mouse.RightButton == ButtonState.Released)
            {
                canClickAgain = true;
            }
            
            if (canReset && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                canReset = false;
                ResetGridForBreadth(false);
            }
            else if (canReset && Keyboard.GetState().IsKeyDown(Keys.B))
            {
                canReset = false;
                ResetGridForBreadth(true);
            }
            else if (canReset && Keyboard.GetState().IsKeyDown(Keys.D))
            {
                //canReset = false;
                //ResetGridForDijkstra();
                Console.WriteLine("Implement at a later time.");
            }
            else if (canReset && Keyboard.GetState().IsKeyDown(Keys.A))
            {
                //canReset = false;
                //ResetGridForAStar();
                Console.WriteLine("Implement at a later time.");
            }
            else if (!Keyboard.GetState().IsKeyDown(Keys.Space) && !Keyboard.GetState().IsKeyDown(Keys.B) && !Keyboard.GetState().IsKeyDown(Keys.D) && !Keyboard.GetState().IsKeyDown(Keys.A))
            {
                canReset = true;
            }
            // TODO: Add your update logic here
            graph.Update(gameTime);
            base.Update(gameTime);
        }

        void SetDestination()
        { 
            destination = new Vector2((int)Mouse.GetState().Position.X / (graphics.PreferredBackBufferWidth / gridSize) , (int)Mouse.GetState().Position.Y / gridSize);
        }

        void CreateObstacle()
        {
            grid[(int)Mouse.GetState().Position.X / (graphics.PreferredBackBufferWidth / gridSize), (int)Mouse.GetState().Position.Y / gridSize].IsBlocked = !grid[(int)Mouse.GetState().Position.X / (graphics.PreferredBackBufferWidth / gridSize), (int)Mouse.GetState().Position.Y / gridSize].IsBlocked;
            graph.UpdateObstacles(new Vector2((int)Mouse.GetState().Position.X / (graphics.PreferredBackBufferWidth / gridSize), (int)Mouse.GetState().Position.Y / gridSize));
        }

        void StartRandomSearch()
        {
            destination = new Vector2(random.Next(0, graphics.PreferredBackBufferWidth / gridSize - 1), random.Next(0, graphics.PreferredBackBufferHeight / gridSize - 1));
            startPosition = new Vector2(random.Next(0, graphics.PreferredBackBufferWidth / gridSize - 1), random.Next(0, graphics.PreferredBackBufferHeight / gridSize - 1));
            graph = new Graph(grid, graphics.PreferredBackBufferWidth / gridSize, graphics.PreferredBackBufferHeight / gridSize);
            graph.BreadthFirstSearchInitialize(startPosition, destination);
        }

        void ResetGridForBreadth(bool startFromCurrentPosition)
        {
            if (startFromCurrentPosition)
                startPosition = graph.curNodePosition;
            graph = new Graph(grid, graphics.PreferredBackBufferWidth / gridSize, graphics.PreferredBackBufferHeight / gridSize);
            graph.BreadthFirstSearchInitialize(startPosition, destination);
        }

        void ResetGridForDijkstra()
        {
            startPosition = graph.curNodePosition;
            graph = new Graph(grid, graphics.PreferredBackBufferWidth / gridSize, graphics.PreferredBackBufferHeight / gridSize, true);
            graph.DijkstraSearchInitialize(startPosition, destination);
        }

        void ResetGridForAStar()
        {
            startPosition = graph.curNodePosition;
            graph = new Graph(grid, graphics.PreferredBackBufferWidth / gridSize, graphics.PreferredBackBufferHeight / gridSize, false);
            graph.AStarSearchInitialize(startPosition, destination);
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
