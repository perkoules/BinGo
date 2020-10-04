namespace Mapbox.Examples
{
    using Mapbox.Unity.Map;
    using UnityEngine;
    using UnityEngine.UI;

    [ExecuteInEditMode]
    public class LoadingPanelController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _content;

        [SerializeField]
        private Text _text;

        [SerializeField]
        private AnimationCurve _curve;

        private AbstractMap _map;

        private void Awake()
        {
            _map = FindObjectOfType<AbstractMap>();
            _map.OnInitialized += _map_OnInitialized;

            _map.OnEditorPreviewEnabled += OnEditorPreviewEnabled;
            _map.OnEditorPreviewDisabled += OnEditorPreviewDisabled;
        }

        private void _map_OnInitialized()
        {
            var visualizer = _map.MapVisualizer;
            _text.text = "LOADING";
            visualizer.OnMapVisualizerStateChanged += (s) =>
            {
                if (this == null)
                    return;

                if (s == ModuleState.Finished)
                {
                    _content.SetActive(false);
                }
                else if (s == ModuleState.Working)
                {
                    // Uncommment me if you want the loading screen to show again
                    // when loading new tiles.
                    //_content.SetActive(true);
                }
            };
        }

        private void OnEditorPreviewEnabled()
        {
            _content.SetActive(false);
        }

        private void OnEditorPreviewDisabled()
        {
            _content.SetActive(true);
        }

        private void Update()
        {
            var t = _curve.Evaluate(Time.time);
            _text.color = Color.Lerp(Color.clear, Color.white, t);
        }
    }
}