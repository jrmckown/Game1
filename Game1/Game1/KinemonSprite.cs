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

        FireworkParticleSystem _firework;
        private KeyboardState keyboardState;

        private BoundingRectangle swordBounds = new BoundingRectangle(new Vector2(200+120, 360-60), 80, 40);

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(200, 360), 240, 22*8);
        public BoundingRectangle Bounds => bounds;

        private Texture2D idleTexture;
        private Texture2D attackTexture;
        private Texture2D jumpTexture;

        private int health = 100;

        private StateEnum state = StateEnum.Idle;

        private bool flipped;

        private double idleTimer;
        private double attackTimer;

        private short animationFrame = 1;

        bool isGrounded = true;

        public Vector2 Position = new Vector2(200, 320); // set to middle stage

        public Vector2 velocity;
        private Vector2 gravity = new Vector2(0, 1300);

        /// <summary>
        /// loads kinemon sprite
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            idleTexture = content.Load<Texture2D>("idle_kinemon");
            attackTexture = content.Load<Texture2D>("Kinemon929");
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

            //state machine
            switch (state)
            {
                case StateEnum.Idle:
                    if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Space)) && isGrounded)
                    {
                        velocity += new Vector2(0, -700);
                        state = StateEnum.Jump;
                        
                    }
                    if (keyboardState.IsKeyDown(Keys.U))
                    {
                        state = StateEnum.Attack;
                        attackTimer = 0;
                        animationFrame= 0;
                    }
                    break;
                case StateEnum.Jump:
                    if (isGrounded)
                    {
                        state = StateEnum.Idle;
                        
                    }
                    if (keyboardState.IsKeyDown(Keys.U))
                    {
                        state = StateEnum.Attack;
                        animationFrame= 0;
                        attackTimer = 0;
                    }
                    break;
                case StateEnum.Attack:
                    attackTimer += gametime.ElapsedGameTime.TotalSeconds;
                    // todo damage
                    if (attackTimer > 0.3)
                    {
                        if (isGrounded)
                        {
                            state = StateEnum.Idle;
                        }
                        else state = StateEnum.Jump;
                    }
                    break;
            }


            
            //if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) position += new Vector2(0, 5); // create crouching capability

            

            //strafe left
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                Position += new Vector2(-5, 0);
                flipped = true;
            }

            //strafe right
            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                Position += new Vector2(5, 0);
                flipped = false;
            }

            //stop screen scrolling too far
            if (Position.X < 0) Position.X = 0;
            if (Position.X > 1200) Position.X = 1200;

            //check if grounded
            if (!isGrounded)
            {
                velocity += gravity * t;
            }
            Position += velocity * t;

            //if (velocity.Y == 70) velocity.Y = 0;
            //acceleration = 9.8 m/s^2
            

            bounds.X = Position.X;
            bounds.Y = Position.Y;
            swordBounds.X = Position.X + 120;
            swordBounds.Y = Position.Y - 60;
        }
        /// <summary>
        /// draws the animated sprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // update animation timer
            idleTimer += gameTime.ElapsedGameTime.TotalSeconds;
            
            // update animation frame
            


            SpriteEffects spriteEffects = (flipped) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Rectangle source = new();
            Texture2D texture = idleTexture;
            switch (state)
            {
                case StateEnum.Idle:
                    {
                        if (idleTimer > 0.3)
                        {
                            animationFrame++;
                            if (animationFrame > 2) animationFrame = 1;
                            idleTimer -= 0.3;
                        }
                        texture = idleTexture;
                        source= new Rectangle(animationFrame * 116, 0, 116, 106);
                        break;
                    }
                case StateEnum.Attack:
                    {
                        if (attackTimer > 0.3)
                        {
                            animationFrame++;
                            if (animationFrame > 1) animationFrame = 0;
                            attackTimer -= 0.3;
                        }

                        texture = attackTexture;
                        source = new Rectangle(animationFrame * 140, 127, 140, 127); 
                        break;
                    }
                case StateEnum.Jump:
                    {
                        if (idleTimer > 0.3)
                        {
                            animationFrame++;
                            if (animationFrame > 2) animationFrame = 1;
                            idleTimer -= 0.3;
                        }
                        texture = idleTexture;
                        source = new Rectangle(animationFrame * 116, 0, 116, 106);
                        break;
                    }
            }
            
            spriteBatch.Draw(texture, Position, source, Color.White, 0, new Vector2(0), 2, spriteEffects, 0);
        }

    }
}