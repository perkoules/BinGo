using System.Collections;
using UnityEngine;

public class DownloadingDataMessage : MonoBehaviour
{
    private void Awake()
    {
        LeaderboardRetriever.OnDataRetrieved += DeleteWindow;
    }

    private void DeleteWindow()
    {
        StartCoroutine(Destruction());
    }

    private void OnDestroy()
    {
        LeaderboardRetriever.OnDataRetrieved -= DeleteWindow;
    }

    IEnumerator Destruction()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}