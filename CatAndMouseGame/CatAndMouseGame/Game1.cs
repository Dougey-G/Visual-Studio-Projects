using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CatAndMouseGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int WINDOW_WIDTH = 1024;
        public static int WINDOW_HEIGHT = 768;

        Random rand;

        static _Mouse mouse;
        _Cat cat;
        HalfDeadCat halfDeadCat;
        CrazyCat crazyCat;

        SpriteFont basicFont;
        float time = 30000;
        string timeText = "Time remaining: ";
        static bool isWinner = false;
        static string winnerText;
      

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
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
            IsMouseVisible = true;
            mouse = new _Mouse(this);
            Components.Add(mouse);
            cat = new _Cat(this, mouse);
            Components.Add(cat);
            halfDeadCat = new HalfDeadCat(this, mouse);
            Components.Add(halfDeadCat);
            crazyCat = new CrazyCat(this, mouse);
            Components.Add(crazyCat);
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
            rand = new Random();
            // TODO: use this.Content to load your game content here

            basicFont = Content.Load<SpriteFont>("BasicFont");

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

            // TODO: Add your update logic here
            KeyboardState keyboard = Keyboard.GetState();


            if (!isWinner && time > 0)
            {
                time -= gameTime.ElapsedGameTime.Milliseconds;
            }
            else if (time <= 0)
            {
                isWinner = true;
                winnerText = "The mouse wins! \n Press spacebar to start new game";
            }

            if (isWinner && keyboard.IsKeyDown(Keys.Space))
            {
                RestartGame();
            }
            else if (isWinner)
            {
                halfDeadCat.FreezeContent();
                mouse.FreezeContent();
                cat.FreezeContent();
                crazyCat.FreezeContent();
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
            spriteBatch.Begin();
            spriteBatch.DrawString(basicFont, timeText + (time / 1000).ToString(), new Vector2(100, 100), Color.White);

            if (isWinner)
            {
                spriteBatch.DrawString(basicFont, winnerText, new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }


        public static void AgentsAreWinners()
        {
            isWinner = true;
            winnerText = "The agents win! \n Press spacebar to start new game";
        }

        void RestartGame()
        {
            isWinner = false;
            time = 30000;
            mouse.UnFreezeContent();
            mouse.RespawnInRandomLocation();
            cat.UnFreezeContent();
            cat.RespawnInRandomLocation();
            halfDeadCat.UnFreezeContent();
            halfDeadCat.RespawnInRandomLocation();
            crazyCat.UnFreezeContent();
            crazyCat.RespawnInRandomLocation();
        }
    }
}
