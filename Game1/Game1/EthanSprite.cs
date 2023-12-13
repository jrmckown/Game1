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

    public class EthanSprite
    {
        private GamePadState gamePadState;
        private KeyboardState keyboardState;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200, 360), 240, 22 * 8); // change bounds
        public BoundingRectangle Bounds => bounds;

        private Texture2D idleTexture;
        private Texture2D attackTexture;
        private Texture2D jumpTexture;

        private int _health = 100;
        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }


        private StateEnum state = StateEnum.Idle;

        private bool flipped;

        private double threeSecondTimer;
        private double attackTimer;

        private short animationFrame = 1;

        bool isGrounded = true;

        public Vector2 Position = new Vector2(800, 320); // set to right stage

        public Vector2 velocity;
        private Vector2 gravity = new Vector2(0, 1300);

        /// <summary>
        /// loads kinemon sprite
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            attackTexture = content.Load<Texture2D>("Ethan");
            // to do load jump animation
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
            if (Position.Y >= 320) isGrounded = true;

            else isGrounded = false;

            

            //reset velocity if on ground
            if (isGrounded) velocity.Y = 0;
               
            bounds.X = Position.X;
            bounds.Y = Position.Y;
        }
        /// <summary>
        /// draws the animated sprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // update animation timer
            attackTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //count 3 seconds before every attack
            threeSecondTimer += gameTime.ElapsedGameTime.TotalSeconds;

            // update animation frame



            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle source = new();
            Texture2D texture = attackTexture;

            if (threeSecondTimer > 1)
            {

                if (attackTimer > 0.3)
                {
                    animationFrame++;
                    if (animationFrame > 2) animationFrame = 0;
                    attackTimer -= 0.3;
                }
                threeSecondTimer = -1;
            }
            

            source = new Rectangle(animationFrame * 128, 0, 128, 128);


            spriteBatch.Draw(texture, Position, source, Color.White, 0, new Vector2(0), 2, spriteEffects, 0);
        }

    }
}