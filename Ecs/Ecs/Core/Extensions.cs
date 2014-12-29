using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecs.Core
{
    internal static class Extensions
    {
        internal static bool ImplementsInterface<TInterface>(this object instance)
        {
            return instance is TInterface;

        }

        internal static T Cast<T>(this object instance)
        {
            return (T)instance;
        }
    }
}
