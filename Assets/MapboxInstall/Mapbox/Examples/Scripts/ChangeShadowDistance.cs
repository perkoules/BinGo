namespace Mapbox.Examples
{
    using UnityEngine;

    public class ChangeShadowDistance : MonoBehaviour
    {
        public int ShadowDistance;

        private void Start()
        {
            QualitySettings.shadowDistance = ShadowDistance;
        }
    }
}