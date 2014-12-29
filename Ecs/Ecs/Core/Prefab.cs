using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecs.Core.Functions;

namespace Ecs.Core
{
    public class Prefab
    {
        private readonly string _name;
        private readonly Component[] _components;
        private readonly int _layer;

        public Prefab(string name, Component[] components, int layer = 0)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (components == null) throw new ArgumentNullException("components");
            _name = name;
            _components = components;
            _layer = layer;

            foreach (var component in _components)
            {
                if (!component.ImplementsInterface<IClone>())
                {
                    throw new Exception(string.Format("Component of type '{0}' is not cloneable and cannot be a prefab", component.GetType().Name));
                }
            }
        }

        public string Name 
        {
            get { return _name; }
        }

        public int Layer
        {
            get { return _layer; }
        }

        internal void CreateInstance(Entity entity)
        {
            foreach (var component in _components)
                entity.AddComponent(component.Cast<IClone>().Clone());
        }
    }
}
