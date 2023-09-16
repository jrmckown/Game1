using Game1.Collisions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.Collisions
{
    /// <summary>
    /// a struct representing a Rectanglebound
    /// </summary>
    public struct BoundingRectangle
    {
        public float X;

        public float Y;

        public float Width;

        public float Height;

        public float Left => X;

        public float Right => X + Width;

        public float Top => Y;

        public float Bottom => Y + Height;

        /// <summary>
        /// constructs new boundingRectangle
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public BoundingRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y; Width = width; Height = height;
        }

        public BoundingRectangle(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width; Height = height;
        }

        /// <summary>
        /// tests for a collision between this and another bounding circle
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(other, this);
        }
    }
}
