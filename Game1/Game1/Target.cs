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
using Game1.Collisions;
using System.Xml;

namespace Game1
{
    public class Target
    {
        private Texture2D texture;
        public Color Color = Color.White;

        Vector2 position = new Vector2(600, 380);

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(600, 380), 132, 132);
        public BoundingRectangle Bounds => bounds;

        public void Update(GameTime gametime)
        {
            bounds.X = position.X - 16;
            bounds.Y = position.Y - 16;
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("target");
        }

        /// <summary>
        /// draws the animated sprite
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color);
        }
    }
}
