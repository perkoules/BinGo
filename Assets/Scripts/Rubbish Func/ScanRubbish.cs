using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TBEasyWebCam;
public class ScanRubbish : MonoBehaviour
{
    public QRController barcodeController;
    public GameObject scanLineObj, torchOff, torchOn;


#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    private bool isTorchOn = false;
#endif

    /// <summary>
    /// when you set the var is true,if the result of the decode is web url,it will open with browser.
    /// </summary>
    public bool isOpenBrowserIfUrl;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (this.barcodeController != null)
        {
            this.barcodeController.onQRScanFinished += new QRController.QRScanFinished(CollectRubbish.Instance.QrScanFinished);
        }
    }

    

    public void Reset()
    {
        if (this.barcodeController != null)
        {
            this.barcodeController.Reset();
        }
        /*if (this.UiText != null)
        {
            this.UiText.text = string.Empty;
        }
        if (this.rescanButton != null)
        {
            this.rescanButton.SetActive(false);
        }*/
        if (this.scanLineObj != null)
        {
            this.scanLineObj.SetActive(true);
        }
    }

    public void Play()
    {
        Reset();
        if (this.barcodeController != null)
        {
            this.barcodeController.StartWork();
        }
    }

    public void Stop()
    {
        if (this.barcodeController != null)
        {
            this.barcodeController.StopWork();
        }
        /*
        if (this.rescanButton != null)
        {
            this.rescanButton.SetActive(false);
        }*/
        if (this.scanLineObj != null)
        {
            this.scanLineObj.SetActive(false);
        }
    }

    public void GotoNextScene(string scenename)
    {
        if (this.barcodeController != null)
        {
            this.barcodeController.StopWork();
        }
        //Application.LoadLevel(scenename);
        SceneManager.LoadScene(scenename);
    }

    /// <summary>
    /// Toggles the torch by click the ui button
    /// note: support the feature by using the EasyWebCam Component
    /// </summary>
    public void ToggleTorch()
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (EasyWebCam.isActive)
        {
        if (isTorchOn)
            {
                torchOff.SetActive(true);
                torchOn.SetActive(false);
                EasyWebCam.setTorchMode(TBEasyWebCam.Setting.TorchMode.Off);
            }
            else
            {
                torchOff.SetActive(false);
                torchOn.SetActive(true);
                EasyWebCam.setTorchMode(TBEasyWebCam.Setting.TorchMode.On);
            }
            isTorchOn = !isTorchOn;
        }
#endif
    }

    public void TorchOff()
    {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (EasyWebCam.isActive)
        {
            EasyWebCam.setTorchMode(TBEasyWebCam.Setting.TorchMode.Off);            
        }
#endif
    }
}