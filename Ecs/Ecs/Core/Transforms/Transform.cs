using System;
using Microsoft.Xna.Framework;

namespace Ecs.Core.Transforms
{
    public class Transform
    {
        public Vector2 Position, Scale;
        public float Rotation;

        public Transform()
        {
            Position = new Vector2();
            Scale = new Vector2(1f, 1f);
            Rotation = 0f;
        }

        public void Decompose(Matrix matrix)
        {
            Vector3 position3, scale3;
            Quaternion rotationQ;
            matrix.Decompose(out scale3, out rotationQ, out position3);
            Vector2 direction = Vector2.Transform(Vector2.UnitX, rotationQ);

            Rotation = (float)Math.Atan2(direction.Y, direction.X);
            Position = new Vector2(position3.X, position3.Y);
            Scale = new Vector2(scale3.X, scale3.Y);
        }

        public Matrix Matrix
        {
            get
            {
                return Matrix.CreateScale(Scale.X, Scale.Y, 1f) *
                       Matrix.CreateRotationZ(Rotation) *
                       Matrix.CreateTranslation(Position.X, Position.Y, 0f);
            }
        }

        public Transform Clone()
        {
            return new Transform()
            {
                Position = Position,
                Scale = Scale,
                Rotation = Rotation
            };
        }
    }
}
