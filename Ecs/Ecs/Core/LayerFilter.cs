namespace Ecs.Core
{
    public class LayerFilter
    {
        private readonly int _layer;

        public LayerFilter(int layer)
        {
            _layer = layer;
        }

        public int Layer
        {
            get { return _layer; }
        }

        public virtual bool Filter(Entity e1, Entity e2)
        {
            return e1.IndexInLayer > e2.IndexInLayer;
        }
    }
}