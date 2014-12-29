using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
