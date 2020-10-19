using BarcodeScanner;
using BarcodeScanner.Scanner;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wizcorp.Utils.Logger;

public class SimpleDemo : MonoBehaviour
{
    private IScanner barcodeScanner;
    public TextMeshProUGUI TextHeader;
    public RawImage Image;
    public Image frames;
    public AudioSource Audio;
    public GameObject arSession;
    private Camera rubbishCamera;

    // Disable Screen Rotation on that screen
    private void Awake()
    {
        rubbishCamera = GetComponent<Camera>();
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }
    private void OnEnable()
    {
        arSession.SetActive(false);
        StartCoroutine(CheckIfEnabled());
    }
    private IEnumerator CheckIfEnabled()
    {
        if (!rubbishCamera.enabled)
        {
            yield return new WaitUntil(() => rubbishCamera.enabled);
        }
        yield return new WaitForSeconds(2);
        ClickStart();
    }

    public void Start()
    {
         // Create a basic scanner  
        barcodeScanner = new Scanner();
        barcodeScanner.Camera.Play();
        // Display the camera texture through a RawImage
        barcodeScanner.OnReady += (sender, arg) =>
        {
            // Set Orientation & Texture
            Image.transform.localEulerAngles = barcodeScanner.Camera.GetEulerAngles();
            Image.transform.localScale = barcodeScanner.Camera.GetScale();
            Image.texture = barcodeScanner.Camera.Texture;

            // Keep Image Aspect Ratio
            var rect = Image.GetComponent<RectTransform>();
            //var newHeight = rect.sizeDelta.x * barcodeScanner.Camera.Height / barcodeScanner.Camera.Width;
            //rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
        };

        // Track status of the scanner
        barcodeScanner.StatusChanged += (sender, arg) =>
        {
            TextHeader.text = "Status: " + barcodeScanner.Status;
        };
    }

    /// <summary>
    /// The Update method from unity need to be propagated to the scanner
    /// </summary>
    private void Update()
    {
        if (barcodeScanner == null)
        {
            return;
        }
        barcodeScanner.Update();
    }

    #region UI Buttons

    public void ClickStart()
    {
        if (barcodeScanner == null)
        {
            Log.Warning("No valid camera - Click Start");
            return;
        }

        // Start Scanning
        barcodeScanner.Scan((barCodeType, barCodeValue) =>
        {
            barcodeScanner.Stop();
            TextHeader.text = "Found: " + barCodeType + " / " + barCodeValue;
            frames.color = Color.green;
            // Feedback
            Audio.Play();

#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        });
        frames.color = Color.white;
    }

    public void ClickStop()
    {
        if (barcodeScanner == null)
        {
            Log.Warning("No valid camera - Click Stop");
            return;
        }

        // Stop Scanning
        barcodeScanner.Stop();
    }

    public void ClickBack()
    {
        // Try to stop the camera before loading another scene
        StartCoroutine(StopCamera(() =>
        {
            Debug.Log("Rubbish Camera Stopped");
        }));
    }

    /// <summary>
    /// This coroutine is used because of a bug with unity (http://forum.unity3d.com/threads/closing-scene-with-active-webcamtexture-crashes-on-android-solved.363566/)
    /// Trying to stop the camera in OnDestroy provoke random crash on Android
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    public IEnumerator StopCamera(Action callback)
    {
        // Stop Scanning
        Image = null;
        barcodeScanner.Destroy();
        barcodeScanner = null;

        // Wait a bit
        yield return new WaitForSeconds(0.1f);

        callback.Invoke();
    }

    #endregion UI Buttons
}