using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecs.Core.Cloning;

namespace Ecs.Core
{
    public class Prefab
    {
        private readonly string _name;
        private readonly Component[] _components;
        private readonly System[] _systems;
        private readonly int _layer;

        public Prefab(string name, Component[] components, System[] systems, int layer = 0)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (components == null) throw new ArgumentNullException("components");
            if (systems == null) throw new ArgumentNullException("systems");
            _name = name;
            _components = components;
            _systems = systems;
            _layer = layer;
        }

        public string Name 
        {
            get { return _name; }
        }

        public int Layer
        {
            get { return _layer; }
        }

        internal void InitializeFor(Entity entity)
        {
            foreach (var component in _components)
                entity.AddComponent(component.Clone(component.GetType()));
            foreach (var system in _systems)
                entity.AddSystem(system.Clone(system.GetType()));
        }
    }
}
