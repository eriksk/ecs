using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Ecs.Core
{
    public class EntityWorld
    {
        private readonly List<Entity> _entities;
        private readonly List<Entity> _destroyedEntities, _newEntities; 
        private readonly IdentifierGenerator _identifierGenerator;
        private readonly List<LayerFilter> _layerFilters;

        public EntityWorld()
        {
            _entities = new List<Entity>();
            _destroyedEntities = new List<Entity>();
            _newEntities = new List<Entity>();
            _identifierGenerator = new IdentifierGenerator();
            _layerFilters = new List<LayerFilter>();
        }

        public List<Entity> Entities 
        {
            // TODO: slow....
            get { return _entities.Concat(_newEntities).ToList(); }
        }

        public int Count 
        {
            get { return _entities.Count + _newEntities.Count; }
        }

        public void Clear()
        {
            _entities.Clear();
            _newEntities.Clear();
            _destroyedEntities.Clear();
        }

        public Entity Instantiate(Prefab prefab)
        {
            return Instantiate(prefab, new Vector2());
        }

        public Entity Instantiate(Prefab prefab, Vector2 position)
        {
            return Instantiate(prefab, position, 0f);
        }

        public Entity Instantiate(Prefab prefab, Vector2 position, float rotation)
        {
            return Instantiate(prefab, position, rotation, new Vector2(1f, 1f));
        }

        public Entity Instantiate(Prefab prefab, Vector2 position, float rotation, Vector2 scale)
        {
            var entity = Create(prefab.Name + " (Copy)", prefab.Layer);
            
            entity.Transform.Position = position;
            entity.Transform.Rotation = rotation;
            entity.Transform.Scale = scale;
            prefab.InitializeFor(entity);

            return entity;
        }

        /// <summary>
        /// Adds a layer filter, replaces the old one for the layer
        /// </summary>
        /// <param name="filter"></param>
        public void AddLayerFilter(LayerFilter filter)
        {
            if (_layerFilters.Any(x => x.Layer == filter.Layer))
            {
                _layerFilters.Remove(_layerFilters.First(x => x.Layer == filter.Layer));
            }

            _layerFilters.Add(filter);
        }

        public Entity Find(string name)
        {
            // TODO: index
            return _entities.FirstOrDefault(x => x.Name == name) ?? _newEntities.First(x => x.Name == name);
        }

        public Entity Find(int id)
        {
            // TODO: index
            return _entities.FirstOrDefault(x => x.Id == id) ?? _newEntities.First(x => x.Id == id);
        }

        public Entity Create(string name, int layer)
        {
            var entity = new Entity(name, _identifierGenerator.Next(), this);
            entity.Layer = layer;
            _newEntities.Add(entity);
            return entity;
        }

        public IEnumerable<Entity> GetByLayer(int layer)
        {
            return _entities.Where(entity => entity.Layer == layer);
        }

        public void Destroy(Entity entity)
        {
            _destroyedEntities.Add(entity);
        }

        public void Update(float dt)
        {
            while (_newEntities.Count > 0)
            {
                //_newEntities[0].Start();
                _entities.Add(_newEntities[0]);
                _newEntities.RemoveAt(0);
            }

            foreach (var entity in _entities)
                entity.Update(dt);

            while (_destroyedEntities.Count > 0)
            {
                _entities.Remove(_destroyedEntities[0]);
                _destroyedEntities.RemoveAt(0);
            }
        }

        private void SortInRenderOrder()
        {
            // insertion sort for quick almost sorted lists

            for (int i = 1; i < _entities.Count; i++)
            {
                int j = i;
                while (j > 0)
                {
                    if (_entities[j - 1].Layer > _entities[j].Layer)
                    {
                        var temp = _entities[j - 1];
                        _entities[j - 1] = _entities[j];
                        _entities[j] = temp;
                        j--;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void ApplyLayerFilters()
        {
            CreateDefaultFilters();

            foreach (var filter in _layerFilters)
            {
                for (int i = 1; i < _entities.Count; i++)
                {
                    int j = i;
                    while (j > 0)
                    {
                        if (_entities[j - 1].Layer == _entities[j].Layer)
                        {
                            if (filter.Filter(_entities[j - 1], _entities[j]))
                            {
                                var temp = _entities[j - 1];
                                _entities[j - 1] = _entities[j];
                                _entities[j] = temp;
                                j--;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        private void CreateDefaultFilters()
        {
            foreach (Entity entity in _entities)
            {
                if (_layerFilters.All(x => x.Layer != entity.Layer))
                {
                    _layerFilters.Add(new LayerFilter(entity.Layer));
                }
            }
        }

        public void PreRender()
        {
            SortInRenderOrder();
            ApplyLayerFilters();
        }

        public void Render()
        {
            PreRender();
            foreach (var entity in _entities)
            {
                entity.Render();
            }
        }
    }
}
