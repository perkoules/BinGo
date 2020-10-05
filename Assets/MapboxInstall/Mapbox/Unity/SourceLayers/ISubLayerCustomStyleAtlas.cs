namespace Mapbox.Unity.Map
{
    using Mapbox.Unity.MeshGeneration.Data;
    using UnityEngine;

    public interface ISubLayerCustomStyleAtlas : ISubLayerCustomStyleOptions, ISubLayerStyle
    {
        AtlasInfo UvAtlas { get; set; }

        void SetAsStyle(Material TopMaterial, Material SideMaterial, AtlasInfo uvAtlas);
    }
}