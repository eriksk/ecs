using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecs.Core.Functions;
using Ecs.Core.Messages;
using Ecs.Core.Transforms;

namespace Ecs.Core
{
    public sealed class Entity
    {
        private readonly List<Component> _components;
        public Transform Transform;
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
            Transform = new Transform();
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

        public void Destroy()
        {
            _world.Destroy(this);
        }

        public void AddComponent(Component component)
        {
            component.Entity = this;
            _components.Add(component);
        }

        public TComponentType GetComponent<TComponentType>()
        {
            foreach (var component in _components)
            {
                if (component.GetType() == typeof (TComponentType))
                    return component.Cast<TComponentType>();
            }
            throw new ArgumentException("Component for type '{0}' does not exist", typeof(TComponentType).Name);
        }

        public void Receive(Message message)
        {
            foreach (var component in _components)
            {
                if (component.ImplementsInterface<IReceiveMessage>())
                    (component as IReceiveMessage).ReceiveMessage(message);
            }
        }

        public void Start()
        {
            foreach (var component in _components)
            {
                if (component.ImplementsInterface<IStart>())
                    (component as IStart).Start();
            }
        }

        public void Update(float dt)
        {
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
