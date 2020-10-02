using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}