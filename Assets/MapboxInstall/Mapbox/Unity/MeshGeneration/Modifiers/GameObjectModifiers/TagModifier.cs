namespace Mapbox.Unity.MeshGeneration.Modifiers
{
    using Mapbox.Unity.MeshGeneration.Data;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Mapbox/Modifiers/Tag Modifier")]
    public class TagModifier : GameObjectModifier
    {
        [SerializeField]
        private string _tag;

        public override void Run(VectorEntity ve, UnityTile tile)
        {
            ve.GameObject.tag = _tag;
        }
    }
}