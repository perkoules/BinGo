using UnityEngine;

namespace Michsky.UI.ModernUIPack
{
    public class LaunchURL : MonoBehaviour
    {
        public void UrlLinkOrWeb(string URL)
        {
            Application.OpenURL(URL);
        }
    }
}