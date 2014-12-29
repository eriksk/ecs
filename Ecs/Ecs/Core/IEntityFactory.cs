using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecs.Core
{
    public interface IEntityFactory
    {
        Entity Create();
        Entity Create(float x, float y);
    }
}
