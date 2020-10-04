namespace Mapbox.Unity.Utilities
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class OpenUrlOnButtonClick : MonoBehaviour
    {
        [SerializeField]
        private string _url;

        protected virtual void Awake()
        {
            GetComponent<Button>().onClick.AddListener(VisitUrl);
        }

        private void VisitUrl()
        {
            Application.OpenURL(_url);
        }
    }
}