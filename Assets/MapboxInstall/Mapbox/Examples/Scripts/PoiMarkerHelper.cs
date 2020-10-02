namespace Mapbox.Examples
{
    using Mapbox.Unity.MeshGeneration.Interfaces;
    using System.Collections.Generic;
    using UnityEngine;

    public class PoiMarkerHelper : MonoBehaviour, IFeaturePropertySettable
    {
        private Dictionary<string, object> _props;

        public void Set(Dictionary<string, object> props)
        {
            _props = props;
        }

        private void OnMouseUpAsButton()
        {
            foreach (var prop in _props)
            {
                Debug.Log(prop.Key + ":" + prop.Value);
            }
        }
    }
}