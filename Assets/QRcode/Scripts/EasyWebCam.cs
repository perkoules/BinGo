﻿using TBEasyWebCam.CallBack;
using TBEasyWebCam.Setting;
using UnityEngine;

namespace TBEasyWebCam
{
    public class EasyWebCam : MonoBehaviour
    {
        public static IEasyWebCam easyWebCamInterface;

        public ResolutionMode mCamResolution = ResolutionMode.MediumResolution;
        public static FocusMode mFocusMode = FocusMode.AutoFocus;
        public static bool isActive = false;

        public static Texture2D WebCamPreview
        {
            get
            {
                if (easyWebCamInterface != null)
                {
                    return easyWebCamInterface.WebCamPreview;
                }
                else
                {
                    return null;
                }
            }
        }

        static EasyWebCam()
        {
            //	Debug.Log("wo cha nimade enter le easywebcamX .....");
        }

        private void Awake()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID
			isActive = true;
			easyWebCamInterface = new EasyWebCamAndroid ();
			if (easyWebCamInterface.isCameraUsable ()) {
			}
			NativePlugin.LogStr= 401;
#elif UNITY_IOS

			isActive = true;
			easyWebCamInterface = new EasyWebCamiOS ();
			NativePlugin.LogStr= 401;

#endif

            setPreviewResolution(mCamResolution);
        }

        private void Start()
        {
            EasyWebCam.OnPreviewStart += PreviewStart;
            EasyWebCam.OnPreviewUpdate += CameraUpdate;
            RenderListenerUtility.onPause += OnPause;
            RenderListenerUtility.onQuit += OnRelease;
        }

        private void PreviewStart()
        {
            setFocusMode(mFocusMode);
        }

        private void CameraUpdate()
        {
        }

        private void Update()
        {
            if (easyWebCamInterface != null && EasyWebCamBase.isRunning)
            {
                easyWebCamInterface.UpdateImage();
                if (Input.GetMouseButtonDown(0))
                {
                    setFocusMode(mFocusMode);
                }
            }
        }

        public static event EasyWebCamStartedDelegate OnPreviewStart
        {
            add
            {
                if (easyWebCamInterface != null)
                {
                    EasyWebCamBase.onEasyWebCamStart += value;
                }
            }
            remove
            {
                if (easyWebCamInterface != null)
                {
                    EasyWebCamBase.onEasyWebCamStart -= value;
                }
            }
        }

        public static event EasyWebCamUpdateDelegate OnPreviewUpdate
        {
            add
            {
                if (easyWebCamInterface != null)
                {
                    EasyWebCamBase.OnEasyWebCamUpdate += value;
                }
            }
            remove
            {
                if (easyWebCamInterface != null)
                {
                    EasyWebCamBase.OnEasyWebCamUpdate -= value;
                }
            }
        }

        public static event EasyWebCamStopedDelegate OnPreviewStoped
        {
            add
            {
                if (easyWebCamInterface != null)
                {
                    EasyWebCamBase.OnEasyWebCamStoped += value;
                }
            }
            remove
            {
                if (easyWebCamInterface != null)
                {
                    EasyWebCamBase.OnEasyWebCamStoped -= value;
                }
            }
        }

        /// <summary>
        /// Play this instance.
        /// </summary>
        public static void Play()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID || UNITY_IOS
			if (easyWebCamInterface != null) {
				easyWebCamInterface.Play ();
			}
#endif
        }

        /// <summary>
        /// Stop this instance.
        /// </summary>
        public static void Stop()
        {
#if UNITY_EDITOR

#elif UNITY_ANDROID || UNITY_IOS
			if (easyWebCamInterface != null) {
			easyWebCamInterface.Stop ();
			}
#endif
        }

        /// <summary>
        /// Sets the preview resolution.
        /// </summary>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public static void setPreviewResolution(ResolutionMode resolutionMode)
        {
            if (easyWebCamInterface != null)
            {
                int preWidth = 0;
                int preHeight = 0;
                resolutionMode.Dimensions(out preWidth, out preHeight);

                easyWebCamInterface.setPreviewResolution(preWidth, preHeight);
            }
        }

        /// <summary>
        /// Takes the photo.
        /// </summary>
        public static void TakePhoto()
        {
            if (easyWebCamInterface != null)
            {
                easyWebCamInterface.TakePhoto();
            }
        }

        /// <summary>
        /// Sets the focus mode.
        /// </summary>
        /// <param name="paramode">Paramode.</param>
        public static void setFocusMode(FocusMode paramode)
        {
            if (!isPlaying())
            {
                return;
            }
            if (easyWebCamInterface != null)
            {
                easyWebCamInterface.setFocusMode((int)paramode);
            }
            mFocusMode = paramode;
        }

        public static void tapFocus()
        {
            if (!isPlaying())
            {
                return;
            }
            if (easyWebCamInterface != null)
            {
                easyWebCamInterface.tapFocus();
            }
        }

        /// <summary>
        /// Sets the flash mode.
        /// </summary>
        /// <param name="paramode">Paramode.</param>
        public static void setFlashMode(FlashMode paramode)
        {
            if (!isPlaying())
            {
                return;
            }
            if (easyWebCamInterface != null)
            {
                easyWebCamInterface.setFocusMode((int)paramode);
            }
        }

        /// <summary>
        /// Sets the torch mode.
        /// </summary>
        /// <param name="paramode">Paramode.</param>
        public static void setTorchMode(TorchMode paramode)
        {
            if (!isPlaying())
            {
                return;
            }
            if (easyWebCamInterface != null)
            {
                easyWebCamInterface.setTorchMode((int)paramode);
            }
        }

        public static void SwitchCamera(int mode)
        {
            if (easyWebCamInterface != null)
            {
                easyWebCamInterface.SwitchCamera(mode);
            }
        }

        public static int Width()
        {
            if (!isPlaying())
            {
                return 0;
            }
            if (easyWebCamInterface != null)
            {
                return easyWebCamInterface.previewWidth;
            }
            return 0;
        }

        public static int Height()
        {
            if (!isPlaying())
            {
                return 0;
            }
            if (easyWebCamInterface != null)
            {
                return easyWebCamInterface.previewHeight;
            }
            return 0;
        }

        public static int getFrame()
        {
            if (easyWebCamInterface != null)
            {
                return easyWebCamInterface.getEnterFrame();
            }
            else
            {
                return -1;
            }
        }

        private void OnPause(bool isPaused)
        {
            if (easyWebCamInterface != null)
            {
                easyWebCamInterface.OnPause(isPaused);
            }
        }

        private void OnRelease()
        {
            if (easyWebCamInterface != null)
            {
                easyWebCamInterface.Release();
            }
        }

        public static void Release()
        {
            if (easyWebCamInterface != null)
            {
                easyWebCamInterface.Release();
            }
        }

        public static bool isPlaying()
        {
            return EasyWebCamBase.isRunning;
        }
    }
}