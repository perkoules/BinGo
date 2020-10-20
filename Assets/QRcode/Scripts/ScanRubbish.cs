using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TBEasyWebCam;
public class ScanRubbish : MonoBehaviour
{
    public QRController e_qrController;
    //public TextMeshProUGUI UiText;
    public GameObject rescanButton;
    public GameObject scanLineObj, torchOff, torchOn;

    public Image frames;
    public Button exitButton;

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
        if (this.e_qrController != null)
        {
            this.e_qrController.onQRScanFinished += new QRController.QRScanFinished(this.QrScanFinished);
        }
    }

    private void QrScanFinished(string dataText)
    {
        if (isOpenBrowserIfUrl)
        {
            if (Utility.CheckIsUrlFormat(dataText))
            {
                if (!dataText.Contains("http://") && !dataText.Contains("https://"))
                {
                    dataText = "http://" + dataText;
                }
                Application.OpenURL(dataText);
            }
        }
        //this.UiText.text = dataText;
        frames.color = Color.green;
        StartCoroutine(RubbishCooldown());
        if (this.rescanButton != null)
        {
            this.rescanButton.SetActive(true);
        }
        if (this.scanLineObj != null)
        {
            this.scanLineObj.SetActive(false);
        }
    }

    private IEnumerator RubbishCooldown()
    {
        yield return new WaitForSeconds(5);

        frames.color = Color.white;
        Reset();
    }

    public void Reset()
    {
        if (this.e_qrController != null)
        {
            this.e_qrController.Reset();
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
        if (this.e_qrController != null)
        {
            this.e_qrController.StartWork();
        }
    }

    public void Stop()
    {
        if (this.e_qrController != null)
        {
            this.e_qrController.StopWork();
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
        if (this.e_qrController != null)
        {
            this.e_qrController.StopWork();
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
}