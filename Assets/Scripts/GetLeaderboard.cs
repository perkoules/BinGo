using UnityEngine;

public class GetLeaderboard : MonoBehaviour
{
    public GameObject leaderboardHolder;
    private void OnEnable()
    {
        if(leaderboardHolder.transform.childCount == 0)
        {
            FindObjectOfType<PlayfabManager>().GetLeaderboardRubbishCollected();
        }
        else
        {
            foreach (Transform child in leaderboardHolder.transform)
            {
                child.gameObject.SetActive(true);
            }
        }        
    }
    
    private void OnDisable()
    {
        foreach (Transform child in leaderboardHolder.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    public void ClearLeaderboard()
    {
        foreach (Transform child in leaderboardHolder.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
