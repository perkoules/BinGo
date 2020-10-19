using BarcodeScanner;
using BarcodeScanner.Scanner;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wizcorp.Utils.Logger;

public class ScanRubbish : MonoBehaviour
{
    private IScanner barcodeScanner;
    public TextMeshProUGUI codeFoundText;
    public RawImage imageToProject;
    public Image frames, line;
    public AudioSource audioSource;
    public GameObject arSession;
    public Button exitButton;
    // Disable Screen Rotation on that screen
    private void Awake()
    {
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }
    /*private void OnEnable()
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
        Init();
        yield return new WaitForSeconds(2);
        ClickStart();
    }*/

    public void Init()
    {
         // Create a basic scanner  
        barcodeScanner = new Scanner();
        barcodeScanner.Camera.Play();
        // Display the camera texture through a RawImage
        barcodeScanner.OnReady += (sender, arg) =>
        {
            // Set Orientation & Texture
            imageToProject.transform.localEulerAngles = barcodeScanner.Camera.GetEulerAngles();
            imageToProject.transform.localScale = barcodeScanner.Camera.GetScale();
            imageToProject.texture = barcodeScanner.Camera.Texture;

            // Keep Image Aspect Ratio
            var rect = imageToProject.GetComponent<RectTransform>();
            //var newHeight = rect.sizeDelta.x * barcodeScanner.Camera.Height / barcodeScanner.Camera.Width;
            //rect.sizeDelta = new Vector2(rect.sizeDelta.x, newHeight);
        };

        // Track status of the scanner
        barcodeScanner.StatusChanged += (sender, arg) =>
        {
            codeFoundText.text = "Status: " + barcodeScanner.Status;
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
            codeFoundText.text = "Found: " + barCodeType + " / " + barCodeValue;
            frames.color = Color.green;
            StartCoroutine(RubbishCooldown());
            line.gameObject.SetActive(false);
            // Feedback
            audioSource.Play();

#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        });
        frames.color = Color.white;
        line.gameObject.SetActive(true);
    }

    IEnumerator RubbishCooldown()
    {
        yield return new WaitForSeconds(5);
        frames.color = Color.white;
        exitButton.onClick.Invoke();
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
        gameObject.SetActive(false);
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
        imageToProject = null;
        barcodeScanner.Destroy();
        barcodeScanner = null;

        // Wait a bit
        yield return new WaitForSeconds(0.1f);

        callback.Invoke();
    }

    #endregion UI Buttons
}