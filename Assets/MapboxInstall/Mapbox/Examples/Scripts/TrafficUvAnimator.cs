using UnityEngine;

namespace Mapbox.Examples
{
    public class TrafficUvAnimator : MonoBehaviour
    {
        public Material[] Materials;
        public float Speed;
        private Vector2 _offset;

        private void Start()
        {
        }

        private void Update()
        {
            _offset.Set(_offset.x + Time.deltaTime * Speed, 0.2f);

            foreach (var item in Materials)
            {
                item.SetTextureOffset("_MainTex", _offset);
            }
        }
    }
}