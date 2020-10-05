namespace Mapbox.Examples
{
    using KDTree;
    using Mapbox.Unity.MeshGeneration;
    using Mapbox.Unity.MeshGeneration.Data;
    using UnityEngine;

    public class HighlightBuildings : MonoBehaviour
    {
        public KdTreeCollection Collection;
        public int MaxCount = 100;
        public float Range = 10;
        private Ray ray;
        private Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        private Vector3 pos;
        private float rayDistance;
        private NearestNeighbour<VectorEntity> pIter;

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (groundPlane.Raycast(ray, out rayDistance))
                {
                    pos = ray.GetPoint(rayDistance);
                    pIter = Collection.NearestNeighbors(new double[] { pos.x, pos.z }, MaxCount, Range);
                    while (pIter.MoveNext())
                    {
                        pIter.Current.Transform.localScale = Vector3.zero;
                    }
                }
            }
        }
    }
}