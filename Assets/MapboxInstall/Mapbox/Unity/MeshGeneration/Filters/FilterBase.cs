namespace Mapbox.Unity.MeshGeneration.Filters
{
    using Mapbox.Unity.MeshGeneration.Data;

    public interface ILayerFeatureFilterComparer
    {
        bool Try(VectorFeatureUnity feature);
    }

    public class FilterBase : ILayerFeatureFilterComparer
    {
        public virtual string Key { get { return ""; } }

        public virtual bool Try(VectorFeatureUnity feature)
        {
            return true;
        }

        public virtual void Initialize()
        {
        }
    }
}