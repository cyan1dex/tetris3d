using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using XELibrary;


namespace Tetris3D
{
    /// <summary>
    /// The state the game is currently in
    /// </summary>
    enum GameState
    {
        Title,
        Playing,
    }

    /// <summary>
    /// This class represents the Tetris Game and need heavy work
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        #region fields
        private GraphicsDeviceManager graphics;

        //Screens
        internal const int SCREEN_WIDTH = 1200;
        internal const int SCREEN_HEIGHT = 900;
        private SpriteBatch spriteBatch;

        //Components
        private InputHandler input;

        #endregion

        private ScreenManager screenManager;
        
        /// <summary>
        /// Constructs a new Game object
        /// </summary>
        /// TODO: Ensure this constructor uses a Singleton pattern
        public Game()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            this.graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;

            this.Content.RootDirectory = "Content";

            this.input = new InputHandler(this);
            this.Components.Add(input);
        }

        #region load
        /// <summary>
        /// Initialize game services and data
        /// </summary>
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);            

            this.screenManager = new ScreenManager(this, spriteBatch);
            this.screenManager.addScreen(new MainMenuScreen(this.screenManager.Game));

            this.Components.Add(this.screenManager);

            base.Initialize();
        }
        #endregion


        /// <summary>
        /// Update the game states, inputs, and components
        /// </summary>
        /// <param name="gameTime">The time since last update</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the games components
        /// </summary>
        /// <param name="gameTime">The time since last drawtime</param>
        protected override void Draw(GameTime gameTime)
        {
            this.graphics.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}