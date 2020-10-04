namespace Mapbox.Examples
{
    using System.Collections.Generic;
    using UnityEngine;

    public class HighlightFeature : MonoBehaviour
    {
        private static Material _highlightMaterial;

        private List<Material> _materials = new List<Material>();

        private MeshRenderer _meshRenderer;

        private void Start()
        {
            if (_highlightMaterial == null)
            {
                _highlightMaterial = Instantiate(GetComponent<MeshRenderer>().material);
                _highlightMaterial.color = Color.red;
            }

            _meshRenderer = GetComponent<MeshRenderer>();

            foreach (var item in _meshRenderer.sharedMaterials)
            {
                _materials.Add(item);
            }
        }

        public void OnMouseEnter()
        {
            _meshRenderer.sharedMaterial = _highlightMaterial;
        }

        public void OnMouseExit()
        {
            _meshRenderer.materials = _materials.ToArray();
        }
    }
}