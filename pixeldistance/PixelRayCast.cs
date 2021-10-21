using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace pixeldistance
{
    static class PixelRayCast
    {
        public static bool RaycastHitRectangle(List<PDPoint> map, Vector2 observer, Vector2 currpoint)
        {
            foreach (PDPoint point in map)
            {
                if (point.Type == PointType.Wall)
                {
                    bool hits = Intersects(observer, currpoint, point.Point);
                    if (hits)
                        return true;
                }
            }

            return false;
        }

        public static bool Intersects(Vector2 observer, Vector2 target, Vector2 wallpoint)
        {
            var minX = Math.Min(observer.X, target.X);
            var maxX = Math.Max(observer.X, target.X);
            var minY = Math.Min(observer.Y, target.Y);
            var maxY = Math.Max(observer.Y, target.Y);

            if (wallpoint.X > maxX || wallpoint.X < minX)
            {
                return false;
            }

            if (wallpoint.Y > maxY || wallpoint.Y < minY)
            {
                return false;
            }

            if (wallpoint.X < minX && maxX < wallpoint.X)
            {
                return true;
            }

            if (wallpoint.Y < minY && maxY < wallpoint.Y)
            {
                return true;
            }

            Func<float, float> yForX = x => observer.Y - (x - observer.X) * ((observer.Y - target.Y) / (target.X - observer.X));

            var yAtRectLeft = Math.Floor(yForX(wallpoint.X-1));
            var yAtRectRight = Math.Ceiling(yForX(wallpoint.X+1));

            if (wallpoint.Y < yAtRectLeft && wallpoint.Y < yAtRectRight)
            {
                return false;
            }

            if (wallpoint.Y > yAtRectLeft && wallpoint.Y > yAtRectRight)
            {
                return false;
            }

            return true;
        }
    }
}
