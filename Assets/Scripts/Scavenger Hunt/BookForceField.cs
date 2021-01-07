using UnityEngine;

public class BookForceField : MonoBehaviour
{
    private PlayerDataSaver playerDataSaver;
    public GameObject portalRoot, prefabHit;
    public int hitCount = 0;

    public delegate void BookObtained();

    public static event BookObtained OnBookObtained;

    private void OnEnable()
    {
        if (playerDataSaver.GetBookObtained() == 1)
        {
            Destroy(portalRoot);
        }
    }

    private void Awake()
    {
        playerDataSaver = GetComponent<PlayerDataSaver>();
        MonsterDestroyer.OnBookHit += MonsterDestroyer_OnBookHit;
    }

    private void MonsterDestroyer_OnBookHit(string rayTag, GameObject go, RaycastHit hitPoint)
    {
        if (gameObject.CompareTag(rayTag) && go == this.gameObject)
        {
            hitCount++;
            GameObject obj = Instantiate(prefabHit, hitPoint.transform.position + RandomizeHit(), Quaternion.identity);
            Destroy(obj, 2f);
            if (hitCount >= 15)
            {
                OnBookObtained?.Invoke();
                BookObtainedByHits();
            }
        }
    }

    public Vector3 RandomizeHit()
    {
        int rand = UnityEngine.Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                return Vector3.up * UnityEngine.Random.Range(1, 3);
            case 1:
                return Vector3.right * UnityEngine.Random.Range(1, 3);
            case 2:
                return Vector3.left * UnityEngine.Random.Range(1, 3);
            default:
                return Vector3.zero;
        }
    }

    private void BookObtainedByHits()
    {
        playerDataSaver.SetBookObtained(1);
        Destroy(portalRoot, 2f);
    }
}