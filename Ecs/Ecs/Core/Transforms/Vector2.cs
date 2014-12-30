using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecs.Core.Transforms
{
    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 operator* (Vector2 t, Vector2 other)
        {
            return new Vector2(t.X * other.X, t.Y * other.Y);
        }

        public static Vector2 operator *(Vector2 t, float scalar)
        {
            return new Vector2(t.X * scalar, t.Y * scalar);
        }

        public static Vector2 operator +(Vector2 t, Vector2 other)
        {
            return new Vector2(t.X + other.X, t.Y + other.Y);
        }
    }
}
