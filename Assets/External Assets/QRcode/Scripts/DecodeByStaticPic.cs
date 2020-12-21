using UnityEngine;
using UnityEngine.UI;

public class DecodeByStaticPic : MonoBehaviour
{
    public Texture2D targetTex;
    public Text resultText;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Decode()
    {
        string resultStr = QRController.DecodeByStaticPic(targetTex);
        resultText.text = resultStr;
    }
}