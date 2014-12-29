using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecs.Core.Functions
{
    public interface IClone
    {
        Component Clone();
    }
}
