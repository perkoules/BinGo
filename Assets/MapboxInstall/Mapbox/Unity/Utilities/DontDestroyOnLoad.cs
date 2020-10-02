namespace Mapbox.Unity.Utilities
{
    using UnityEngine;

    public class DontDestroyOnLoad : MonoBehaviour
    {
        private static DontDestroyOnLoad _instance;

        [SerializeField]
        private bool _useSingleInstance;

        protected virtual void Awake()
        {
            if (_instance != null && _useSingleInstance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}