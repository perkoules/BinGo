﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearCameraRenderInfo : MonoBehaviour {
#if USES_OPEN_VR
    public bool toggle = true;
    public bool isLeftEye = true;

    /// <summary>
    ///  called (once per eye)
    /// </summary>
    void OnPreRender() {
        Shader.SetGlobalInt("RenderingEye", isLeftEye ? 1 : 0);
        if (toggle) {
            isLeftEye = !isLeftEye;
        }
    }
#endif
}
