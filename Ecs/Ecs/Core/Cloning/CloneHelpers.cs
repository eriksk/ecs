using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ecs.Core.Attributes;

namespace Ecs.Core.Cloning
{
    internal static class CloneHelpers
    {
        private static void CopyProperties(object sourceObject, object targetObject, bool deepCopy = true)
        {
            foreach (PropertyInfo sourceProperty in sourceObject.GetType().GetProperties())
            {
                if (sourceProperty.IsDefined(typeof (PublicProperty), true))
                {
                    foreach (PropertyInfo targetProperty in targetObject.GetType().GetProperties())
                    {
                        if (sourceProperty.Name.ToUpper() == targetProperty.Name.ToUpper())
                        {
                            var sourceValue = sourceProperty.GetValue(sourceObject, null);
                            if (sourceValue != null && sourceProperty.IsDefined(typeof (PublicProperty), true))
                                CopyProperty(targetProperty, targetObject, sourceValue, deepCopy)();
                        }
                    }
                }
            }
        }

        private static Action CopyProperty(PropertyInfo propertyInfo, object targetObject, object sourceValue, bool deepCopy)
        {
            if (!deepCopy || sourceValue.GetType().FullName.StartsWith("System."))
                return () => propertyInfo.SetValue(targetObject, sourceValue, null);
            
            return () => CopyProperties(sourceValue, propertyInfo.GetValue(targetObject, null));
        }

        internal static T Clone<T>(this T instance, Type type)
        {
            var clone = Activator.CreateInstance(type);
            CopyProperties(instance, clone);
            return (T)clone;
        }
    }
}
