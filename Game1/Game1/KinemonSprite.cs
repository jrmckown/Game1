using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Drawing.Drawing2D;

namespace Game1
{

    public class KinemonSprite
    {
        private GamePadState gamePadState;

        private KeyboardState keyboardState;

        private Texture2D texture;

        private bool flipped;

        private double animationTimer;

        private short animationFrame = 1;

        bool isGrounded = true;

        Vector2 position = new Vector2(200, 200); // change to middle stage

        Vector2 velocity;

        /// <summary>
        /// loads bat sprite
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("idle");
        }


        /// <summary>
        /// updates bat pattern
        /// </summary>
        /// <param name="gametime"></param>
        public void Update(GameTime gametime)
        {

            float t = (float)gametime.ElapsedGameTime.TotalSeconds;
            gamePadState = GamePad.GetState(0);
            keyboardState = Keyboard.GetState();

            //move kinemon in direction
            // Apply the gamepad movement with inverted Y axis
            position += gamePadState.ThumbSticks.Left * new Vector2(5, -5);
            if (gamePadState.ThumbSticks.Left.X < 0) flipped = true;
            if (gamePadState.ThumbSticks.Left.X > 0) flipped = false;


            // Apply keyboard movement
            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) && isGrounded) velocity += new Vector2(0, -10);
            //if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) position += new Vector2(0, 5); // create crouching capability

            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                position += new Vector2(-5, 0);
                flipped = true;
            }

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                position += new Vector2(5, 0);
                flipped = false;
            }

            position += velocity * t;
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
            var source = new Rectangle(animationFrame * 30, 0, 30, 22);
            spriteBatch.Draw(texture, position, source, Color.White, 0, new Vector2(0), 6, spriteEffects, 0);
        }

    }
}