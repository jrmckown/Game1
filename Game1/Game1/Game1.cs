﻿using Game1.Screens;
using Game1.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;

namespace Game1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly ScreenManager _screenManager;
        Target target;
        
        KinemonSprite kinemon;
        Background background;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            Window.Title = "Alley Fighter";
            IsMouseVisible = true;
            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);
            _graphics.ApplyChanges();

            AddInitialScreens();
        }

        private void AddInitialScreens()
        {
            _screenManager.AddScreen(new BackgroundScreen(), null);
            _screenManager.AddScreen(new MainMenuScreen(this), null);
            _screenManager.AddScreen(new SplashScreen(), null);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            /*
            kinemon = new KinemonSprite();
            background = new Background();
            target = new Target();
            
            */
            base.Initialize();
        }

        protected override void LoadContent()
        {
            /*
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            kinemon.LoadContent(Content);
            background.LoadContent(Content);
            target.LoadContent(Content);
            */
        }

        protected override void Update(GameTime gameTime)
        {
            /*
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            kinemon.Update(gameTime);
            target.Update(gameTime);

            if (target.Bounds.CollidesWith(kinemon.Bounds))
            {
                target.Color = Color.Orange;
            }
            else
            {
                target.Color = Color.White;
            }
            */
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            /*
            _spriteBatch.Begin();
            
            background.Draw(_spriteBatch);
            target.Draw(_spriteBatch);
            
            kinemon.Draw(gameTime, _spriteBatch);

            _spriteBatch.End();
            */
            base.Draw(gameTime);
        }
    }
}