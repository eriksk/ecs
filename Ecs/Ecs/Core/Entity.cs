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
        private readonly DynamicList<Component> _components;
        private readonly DynamicList<System> _systems;

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

            _worldTransform = new Transform();
            Transform = new Transform();

            _components = new DynamicList<Component>();
            _systems = new DynamicList<System>();
            
            _parent = null;
        }

        public Entity Parent 
        {
            get { return _parent; }
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

        public List<Component> Components 
        {
            get { return _components.Items; }
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
            _components.Add(component);
        }

        public void RemoveComponent(Component component)
        {
            _components.Remove(component);
        }

        public void AddSystem(System system)
        {
            system.Entity = this;
            _systems.Add(system);
        }

        public void RemoveSystem(System system)
        {
            _systems.Remove(system);
        }

        public TComponentType GetComponent<TComponentType>()
        {
            foreach (var component in _components.Items)
            {
                if (component.GetType() == typeof (TComponentType))
                    return component.Cast<TComponentType>();
            }
            throw new ArgumentException(string.Format("Component for type '{0}' does not exist", typeof(TComponentType).Name));
        }

        public TSystemType GetSystem<TSystemType>()
        {
            foreach (var system in _systems.Items)
            {
                if (system.GetType() == typeof(TSystemType))
                    return system.Cast<TSystemType>();
            }
            throw new ArgumentException(string.Format("System for type '{0}' does not exist", typeof(TSystemType).Name));
        }

        public void Receive(Message message)
        {
            foreach (var system in _systems)
            {
                if (system.ImplementsInterface<IReceiveMessage>())
                    (system as IReceiveMessage).ReceiveMessage(message);
            }
        }

        public void Start()
        {
            foreach (var system in _systems)
            {
                if (system.ImplementsInterface<IStart>())
                    (system as IStart).Start();
            }
        }

        public void Update(float dt)
        {
            foreach (var system in _systems)
            {
                if (system.ImplementsInterface<IUpdate>())
                    (system as IUpdate).Update(dt);
            }
        }

        public void Render()
        {
            foreach (var system in _systems)
            {
                if (system.ImplementsInterface<IRender>())
                    (system as IRender).Render();
            }
        }
    }
}
