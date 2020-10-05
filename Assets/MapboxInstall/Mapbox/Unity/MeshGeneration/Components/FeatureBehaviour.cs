namespace Mapbox.Unity.MeshGeneration.Components
{
    using Mapbox.Unity.MeshGeneration.Data;
    using System.Linq;
    using UnityEngine;

    public class FeatureBehaviour : MonoBehaviour
    {
        public VectorEntity VectorEntity;
        public Transform Transform;
        public VectorFeatureUnity Data;

        [Multiline(5)]
        public string DataString;

        public void ShowDebugData()
        {
            DataString = string.Join("\r\n", Data.Properties.Select(x => x.Key + " - " + x.Value.ToString()).ToArray());
        }

        public void ShowDataPoints()
        {
            foreach (var item in VectorEntity.Feature.Points)
            {
                for (int i = 0; i < item.Count; i++)
                {
                    var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    go.name = i.ToString();
                    go.transform.SetParent(transform, false);
                    go.transform.localPosition = item[i];
                }
            }
        }

        public void Initialize(VectorEntity ve)
        {
            VectorEntity = ve;
            Transform = transform;
            Data = ve.Feature;
        }

        public void Initialize(VectorFeatureUnity feature)
        {
            Transform = transform;
            Data = feature;
        }
    }
}