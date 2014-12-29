using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecs.Core
{
    public class IdentifierGenerator
    {
        private int _count;

        public int Next()
        {
            return _count++;
        }
    }
}
