namespace Mapbox.Unity.Map
{
    using Mapbox.Unity.MeshGeneration.Modifiers;
    using Mapbox.Unity.Utilities;
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class LayerModifierOptions
    {
        public PositionTargetType moveFeaturePositionTo;

        [NodeEditorElement("Mesh Modifiers")]
        public List<MeshModifier> MeshModifiers;

        [NodeEditorElement("Game Object Modifiers")]
        public List<GameObjectModifier> GoModifiers;
    }
}