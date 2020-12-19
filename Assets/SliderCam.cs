using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SliderCam : MonoBehaviour
{
    public TextMeshProUGUI txt, txt2;
    public Camera Arcam;
    public Slider slider, slider2;

    private void Awake()
    {
        slider.onValueChanged.AddListener(Zoom);
        slider2.onValueChanged.AddListener(ZoomSize);
    }

    private void Zoom(float val)
    {
        txt.text = val.ToString();
        Arcam.farClipPlane = val;
    }
    private void ZoomSize(float val)
    {
        txt2.text = val.ToString();
        Arcam.orthographicSize = val;
    }
}
