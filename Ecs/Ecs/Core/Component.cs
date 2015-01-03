using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecs.Core.Messages;
using Ecs.Core.Transforms;
using Microsoft.Xna.Framework;

namespace Ecs.Core
{
    public class Component
    {
        private Entity _entity;

        public Entity Entity
        {
            get { return _entity; }
            internal set { _entity = value; }
        }

        protected Transform Transform
        {
            get { return _entity.Transform; }
        }

        protected Transform WorldTransform
        {
            get { return _entity.WorldTransform; }
        }

        protected List<Entity> AllEntities
        {
            get { return _entity.World.Entities; }
        }

        protected List<Entity> GetByLayer(int layer)
        {
            // TODO: slow....
            return _entity.World.Entities.Where(x => x.Layer == layer).ToList();
        }

        /// <summary>
        /// Destroys this component and removes it from the entity
        /// </summary>
        protected void Destroy()
        {
            _entity.RemoveComponent(this);
        }

        protected void Send(Message message, Entity receiver)
        {
            receiver.Receive(message);
        }

        #region Instantiate
        
        protected Entity Instantiate(Prefab prefab)
        {
            return _entity.World.Instantiate(prefab);
        }
        protected Entity Instantiate(Prefab prefab, Vector2 position)
        {
            return _entity.World.Instantiate(prefab, position);
        }
        protected Entity Instantiate(Prefab prefab, Vector2 position, float rotation)
        {
            return _entity.World.Instantiate(prefab, position, rotation);
        }
        protected Entity Instantiate(Prefab prefab, Vector2 position, float rotation, Vector2 scale)
        {
            return _entity.World.Instantiate(prefab, position, rotation, scale);
        }

        #endregion
        
        protected TComponentType GetComponent<TComponentType>()
        {
            return _entity.GetComponent<TComponentType>();
        }
    }
}
