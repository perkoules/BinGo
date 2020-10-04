﻿namespace Mapbox.Unity.Map
{
    using Mapbox.Unity.Utilities;
    using System;

    [Serializable]
    public class UnifiedMapOptions
    {
        public MapPresetType mapPreset = MapPresetType.LocationBasedMap;
        public MapOptions mapOptions = new MapOptions();

        [NodeEditorElement("Image Layer")]
        public ImageryLayerProperties imageryLayerProperties = new ImageryLayerProperties();

        [NodeEditorElement("Terrain Layer")]
        public ElevationLayerProperties elevationLayerProperties = new ElevationLayerProperties();

        [NodeEditorElement("Vector Layer")]
        public VectorLayerProperties vectorLayerProperties = new VectorLayerProperties();
    }
}