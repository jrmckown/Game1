using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System.Drawing.Drawing2D;
using Game1.Collisions;

namespace Game1
{

    public class KinemonSprite
    {
        private GamePadState gamePadState;

        private KeyboardState keyboardState;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200, 360), 240, 22*8);
        public BoundingRectangle Bounds => bounds;

        private Texture2D texture;

        private bool flipped;

        private double animationTimer;

        private short animationFrame = 1;

        bool isGrounded = true;

        Vector2 position = new Vector2(200, 320); // set to middle stage

        Vector2 velocity;
        Vector2 gravity = new Vector2(0, 1300);

        /// <summary>
        /// loads kinemon sprite
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Kinemon929");
            
        }


        /// <summary>
        /// updates kinemon sprite
        /// </summary>
        /// <param name="gametime"></param>
        public void Update(GameTime gametime)
        {

            float t = (float)gametime.ElapsedGameTime.TotalSeconds;
            gamePadState = GamePad.GetState(0);
            keyboardState = Keyboard.GetState();

            //check if grounded
            if (position.Y >= 320) isGrounded = true;

            else isGrounded = false;

            //reset velocity if on ground
            if (isGrounded) velocity.Y = 0;

            // Apply keyboard movement
            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Space)) && isGrounded)
            {
                velocity += new Vector2(0, -700);
            }
            
            //if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) position += new Vector2(0, 5); // create crouching capability

            //strafe left
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                position += new Vector2(-5, 0);
                flipped = true;
            }

            //strafe right
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                position += new Vector2(5, 0);
                flipped = false;
            }

            //check if grounded
            if (!isGrounded)
            {
                velocity += gravity * t;
            }
            position += velocity * t;

            //if (velocity.Y == 70) velocity.Y = 0;
            //acceleration = 9.8 m/s^2

            bounds.X = position.X;
            bounds.Y = position.Y;
        }
        /// <summary>
        /// draws the animated sprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            // update animation frame
            if (animationTimer > 0.3)
            {
                animationFrame++;
                if (animationFrame > 2) animationFrame = 1;
                animationTimer -= 0.3;
            }
            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            var source = new Rectangle(animationFrame * 140, 0, 140, 127);
            spriteBatch.Draw(texture, position, source, Color.White, 0, new Vector2(0), 2, spriteEffects, 0);
        }

    }
}