using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource globalAudioSource;

    public AudioClip beepSound, rubbishCollectedSound, clickSound;

    private void Awake()
    {
        globalAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        globalAudioSource.Play();
    }
    public void IsMusicOn(bool isMusicOn)
    {
        if (isMusicOn)
        {
            globalAudioSource.Play();
        }
        else
        {
            globalAudioSource.Stop();
        }
    }
}