using GoogleARCore;
using GoogleARCore.Examples.AugmentedImage;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackImages : MonoBehaviour
{
    public AugmentedImageVisualizer AugmentedImageVisualizerPrefab;
    [SerializeField] bool canTrack = true;

    private Dictionary<int, AugmentedImageVisualizer> m_Visualizers = new Dictionary<int, AugmentedImageVisualizer>();
    private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();


    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        //LogoPoints.OnLogoFound += StopTracking;
    }

    private void StopTracking()
    {
        canTrack = false;
        //LogoPoints.OnLogoFound -= StopTracking;
    }
    private void Update()
    {
        if (canTrack)
        {
            Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.Updated);

            foreach (var image in m_TempAugmentedImages)
            {
                m_Visualizers.TryGetValue(image.DatabaseIndex, out AugmentedImageVisualizer visualizer);
                if (image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking && visualizer == null)
                {
                    // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    Anchor anchor = image.CreateAnchor(image.CenterPose);
                    visualizer = Instantiate(AugmentedImageVisualizerPrefab, anchor.transform);
                    visualizer.Image = image;
                    m_Visualizers.Add(image.DatabaseIndex, visualizer);
                }
                else if (image.TrackingMethod == AugmentedImageTrackingMethod.NotTracking && visualizer != null)
                {
                    Debug.Log("Stopped");
                }
            }

            foreach (var visualizer in m_Visualizers.Values)
            {
                if (visualizer.Image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
                {
                    //Debug.LogFormat("Tracking", Color.green);
                }
            } 
        }
    }
}