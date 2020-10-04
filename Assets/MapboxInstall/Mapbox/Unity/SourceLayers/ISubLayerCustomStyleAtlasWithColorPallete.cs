namespace Mapbox.Unity.Map
{
    using Mapbox.Unity.MeshGeneration.Data;
    using UnityEngine;

    public interface ISubLayerCustomStyleAtlasWithColorPallete : ISubLayerCustomStyleOptions, ISubLayerStyle
    {
        ScriptablePalette ColorPalette { get; set; }

        void SetAsStyle(Material TopMaterial, Material SideMaterial, AtlasInfo uvAtlas, ScriptablePalette palette);
    }
}