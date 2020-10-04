namespace Mapbox.Unity.MeshGeneration.Modifiers
{
    using Mapbox.Unity.MeshGeneration.Components;
    using Mapbox.Unity.MeshGeneration.Data;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Mapbox/Modifiers/Add Feature Behaviour Modifier")]
    public class FeatureBehaviourModifier : GameObjectModifier
    {
        private Dictionary<GameObject, FeatureBehaviour> _features;
        private FeatureBehaviour _tempFeature;

        public override void Initialize()
        {
            if (_features == null)
            {
                _features = new Dictionary<GameObject, FeatureBehaviour>();
            }
        }

        public override void Run(VectorEntity ve, UnityTile tile)
        {
            if (_features.ContainsKey(ve.GameObject))
            {
                _features[ve.GameObject].Initialize(ve);
            }
            else
            {
                _tempFeature = ve.GameObject.AddComponent<FeatureBehaviour>();
                _features.Add(ve.GameObject, _tempFeature);
                _tempFeature.Initialize(ve);
            }
        }
    }
}