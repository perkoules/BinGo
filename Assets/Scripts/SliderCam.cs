﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SliderCam : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public Camera Arcam;
    public Slider slider;

    private void Awake()
    {
        slider.onValueChanged.AddListener(Zoom);
    }

    private void Zoom(float val)
    {
        txt.text = val.ToString();
        Arcam.farClipPlane = val;
    }
}