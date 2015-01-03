using System;
using System.Collections.Generic;
using System.Linq;
using Ecs.Core.Functions;
using Ecs.Core.Messages;
using Ecs.Core.Transforms;

namespace Ecs.Core
{
    public sealed class Entity
    {
        private readonly List<Component> _components, _newComponents, _removedComponents;

        private Entity _parent;
        public readonly Transform Transform;
        private readonly Transform _worldTransform;

        private readonly string _name;
        public readonly int Id;

        private readonly EntityWorld _world;

        internal Entity(string name, int id, EntityWorld world)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (world == null) throw new ArgumentNullException("world");
            _name = name;
            Id = id;
            _world = world;
            _components = new List<Component>();
            _newComponents = new List<Component>();
            _removedComponents = new List<Component>();
            Transform = new Transform();
            _worldTransform = new Transform();
            _parent = null;
        }

        public void AddChild(Entity child)
        {
            child._parent = this;
        }

        public string Name
        {
            get { return _name; }
        }

        public int Layer { get; set; }
        public int IndexInLayer { get; set; }

        internal List<Component> Components 
        {
            get { return _components; }
        }

        internal EntityWorld World
        {
            get { return _world; }
        }

        public Transform WorldTransform
        {
            get
            {
                if (_parent == null)
                    return Transform;

                _worldTransform.Decompose(Transform.Matrix * _parent.WorldTransform.Matrix);
                return _worldTransform;
            }
        }

        public void Destroy()
        {
            _world.Destroy(this);
        }

        public void AddComponent(Component component)
        {
            component.Entity = this;
            _newComponents.Add(component);
        }

        public void RemoveComponent(Component component)
        {
            _removedComponents.Add(component);
        }

        public TComponentType GetComponent<TComponentType>()
        {
            foreach (var component in _components)
            {
                if (component.GetType() == typeof (TComponentType))
                    return component.Cast<TComponentType>();
            }
            foreach (var component in _newComponents)
            {
                if (component.GetType() == typeof(TComponentType))
                    return component.Cast<TComponentType>();
            }
            throw new ArgumentException(string.Format("Component for type '{0}' does not exist", typeof(TComponentType).Name));
        }

        public void Receive(Message message)
        {
            foreach (var component in _components)
            {
                if (component.ImplementsInterface<IReceiveMessage>())
                    (component as IReceiveMessage).ReceiveMessage(message);
            }
        }

        private void SyncronizeComponents()
        {
            while (_newComponents.Count > 0)
            {
                _components.Add(_newComponents[0]);
                _newComponents.RemoveAt(0);
            }

            while (_removedComponents.Count > 0)
            {
                _components.Remove(_removedComponents[0]);
                _removedComponents.RemoveAt(0);
            }
        }

        public void Start()
        {
            SyncronizeComponents();
            foreach (var component in _components)
            {
                if (component.ImplementsInterface<IStart>())
                    (component as IStart).Start();
            }
        }

        public void Update(float dt)
        {
            SyncronizeComponents();
            foreach (var component in _components)
            {
                if (component.ImplementsInterface<IUpdate>())
                    (component as IUpdate).Update(dt);
            }
        }

        public void Render()
        {
            foreach (var component in _components)
            {
                if (component.ImplementsInterface<IRender>())
                    (component as IRender).Render();
            }
        }
    }
}
