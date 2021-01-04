//-----------------------------------------------------------------------
// <copyright file="AugmentedImageVisualizer.cs" company="Google LLC">
//
// Copyright 2018 Google LLC. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.AugmentedImage
{
    using GoogleARCore;
    using System;
    using UnityEngine;

    /// <summary>
    /// Uses 4 frame corner objects to visualize an AugmentedImage.
    /// </summary>
    public class AugmentedImageVisualizer : MonoBehaviour
    {
        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public AugmentedImage Image;
        public GameObject objToSpawn;
        private bool canTrack = true;

        private void Start()
        {
            LogoPoints.OnLogoFound += StopTracking;
        }

        private void StopTracking()
        {
            canTrack = false;
            LogoPoints.OnLogoFound -= StopTracking;
        }
        public void Update()
        {
            if (canTrack)
            {
                if (Image == null || Image.TrackingMethod != AugmentedImageTrackingMethod.FullTracking)
                {
                    objToSpawn.SetActive(false);
                    return;
                }
                else
                {
                    /*canTrack = false;
                    OnImageFound();*/
                    float halfWidth = Image.ExtentX / 2;
                    float halfHeight = Image.ExtentZ / 2;
                    objToSpawn.transform.localPosition = new Vector3(halfWidth, 0, halfHeight );
                    objToSpawn.SetActive(true);
                    Debug.Log("IS ON===============================================================");
                }                
            }
        }
        public delegate void ImageFound();
        public static ImageFound OnImageFound;

    }
}