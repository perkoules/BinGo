namespace Mapbox.Unity.Map
{
    using Mapbox.Unity.MeshGeneration.Modifiers;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    [Serializable]
    public class SpawnPrefabOptions : ModifierProperties
    {
        public override Type ModifierType
        {
            get
            {
                return typeof(PrefabModifier);
            }
        }

        public GameObject prefab;
        public bool scaleDownWithWorld = true;

        [NonSerialized]
        public Action<List<GameObject>> AllPrefabsInstatiated = delegate { };
    }
}