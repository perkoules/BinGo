namespace Mapbox.Examples
{
    using Mapbox.Unity.MeshGeneration.Data;
    using UnityEngine;

    public class FeatureSelectionDetector : MonoBehaviour
    {
        private FeatureUiMarker _marker;
        private VectorEntity _feature;

        public void OnMouseUpAsButton()
        {
            _marker.Show(_feature);
        }

        internal void Initialize(FeatureUiMarker marker, VectorEntity ve)
        {
            _marker = marker;
            _feature = ve;
        }
    }
}