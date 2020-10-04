namespace Mapbox.Unity.MeshGeneration.Filters
{
    using Mapbox.Unity.MeshGeneration.Data;
    using UnityEngine;

    public class HeightFilter : FilterBase
    {
        public enum HeightFilterOptions
        {
            Above,
            Below
        }

        public override string Key { get { return "height"; } }

        [SerializeField]
        private float _height;

        [SerializeField]
        private HeightFilterOptions _type;

        public override bool Try(VectorFeatureUnity feature)
        {
            var hg = System.Convert.ToSingle(feature.Properties[Key]);
            if (_type == HeightFilterOptions.Above && hg > _height)
                return true;
            if (_type == HeightFilterOptions.Below && hg < _height)
                return true;

            return false;
        }
    }
}