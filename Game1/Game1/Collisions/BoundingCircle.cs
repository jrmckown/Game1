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
    /// a struct representing a circular bound
    /// </summary>
    public struct BoundingCircle
    {
        /// <summary>
        /// center of the boundingcircle
        /// </summary>
        public Vector2 Center;
        /// <summary>
        /// radius of circle
        /// </summary>
        public float Radius;

        /// <summary>
        /// constructs new boundingCircle
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// tests for a collision between this and another bounding circle
        /// </summary>
        /// <param name="other"></param>
        /// <returns>true for collision, false otherwise</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }
    }
}
