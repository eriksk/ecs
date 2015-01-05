using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ecs.Core.Attributes
{
    public enum PropertyHint
    {
        Value,
        Array,
        ComplexType
    }

    public class PublicProperty : Attribute
    {
        private readonly PropertyHint _hint;

        public PublicProperty(PropertyHint hint = PropertyHint.Value)
        {
            _hint = hint;
        }

        public PropertyHint Hint
        {
            get { return _hint; }
        }
    }
}
