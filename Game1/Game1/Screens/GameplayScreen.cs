﻿using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game1.StateManagement;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using SharpDX.MediaFoundation;

namespace Game1.Screens
{
    public class GameplayScreen : GameScreen, IParticleEmitter
    {
        private ContentManager _content;
        private SpriteFont _gameFont;

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;
        private Texture2D font;
        private SoundEffect _punch;
        private GraphicsDeviceManager _graphics;
        private SpriteFont menufont;

        Target target = new Target();
        bool _leftTarget = false;
        //firework particle system
        FireworkParticleSystem _firework;

        private KeyboardState keyboardState;
        private KeyboardState priorKeyboardState;

        private Song backgroundMusic;
        KinemonSprite kinemon = new KinemonSprite();
        EthanSprite ethan = new EthanSprite();

        Background background = new Background();
        Game game { get; set; }

        public Vector2 Position { get; set; }


        public Vector2 Velocity { get; set; }

        public GameplayScreen(Game g)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            //_content.RootDirectory = "Content";
            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
            game = g;
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            

            _punch = _content.Load<SoundEffect>("Hit_Hurt68");
            backgroundMusic = _content.Load<Song>("Eggy Toast - Irritant");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = .7f;
            
            MediaPlayer.Play(backgroundMusic);
            kinemon.LoadContent(_content);
            ethan.LoadContent(_content);
            background.LoadContent(_content);
            target.LoadContent(_content);
            menufont = _content.Load<SpriteFont>("menufont");

            //initialize firework particles
            _firework = new FireworkParticleSystem(ScreenManager.Game, 20);
            ScreenManager.Game.Components.Add(_firework);

            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //Exit();
            ethan.Update(gameTime);
            kinemon.Update(gameTime);
            target.Update(gameTime);
            
            if (!target.Collected && target.Bounds.CollidesWith(kinemon.Bounds))
            {
                target.Color = Color.Orange;
                target.Collected = true;
                _punch.Play();
            }

            if (false) // if kinemon sword bounds intersect with ethan bounds
            {
                ethan.Health -= 25;
            } 

            else if (target.Bounds.CollidesWith(kinemon.Bounds))
            {
                target.Color = Color.Orange;
            }
            else
            {
                target.Color = Color.White;
            }

            //base.Update(gameTime);
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;
            priorKeyboardState = keyboardState;
            keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                MediaPlayer.Pause();
                ScreenManager.AddScreen(new PauseMenuScreen(game), ControllingPlayer);
            }

            //activate firework
            if (keyboardState.IsKeyDown(Keys.U) && priorKeyboardState.IsKeyUp(Keys.U))
            {
                //firework stuff
                _firework.PlaceFirework(kinemon.Position );
                _punch.Play();
                
            }
            /*if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // Otherwise move the player position.
                var movement = Vector2.Zero;

                if (keyboardState.IsKeyDown(Keys.Left))
                    movement.X--;

                if (keyboardState.IsKeyDown(Keys.Right))
                    movement.X++;

                if (keyboardState.IsKeyDown(Keys.Up))
                    movement.Y--;

                if (keyboardState.IsKeyDown(Keys.Down))
                    movement.Y++;

                var thumbstick = gamePadState.ThumbSticks.Left;

                movement.X += thumbstick.X;
                movement.Y -= thumbstick.Y;

                if (movement.Length() > 1)
                    movement.Normalize();

                _playerPosition += movement * 8f;
            }*/
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);
            var spriteBatch = ScreenManager.SpriteBatch;
            // Player-synched scrolling
            float playerX = MathHelper.Clamp(kinemon.Position.X, 300, 1000);
            float offsetX = 300 - playerX;
            // Clamp the resulting vector to the visible region
            

            // Create the translation matrix representing the offset
            Matrix transform = Matrix.CreateTranslation(offsetX, 0, 0);
            // Draw the transformed game world
            spriteBatch.Begin(transformMatrix: transform);
            // TODO: Draw game sprites within the world, however you need to.

            background.Draw(spriteBatch);
            target.Draw(spriteBatch);
            spriteBatch.DrawString(menufont, $"Boss Health: {ethan.Health}", new Vector2(900, 50), Color.Black);
            ethan.Draw(gameTime, spriteBatch);
            kinemon.Draw(gameTime, spriteBatch);


            spriteBatch.End();
            base.Draw(gameTime);

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
