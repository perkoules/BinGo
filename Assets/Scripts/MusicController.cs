using System;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; set;} 
    private AudioSource globalAudioSource;
    [SerializeField]
    private AudioClip beepSound, rubbishCollectedSound, clickSound, failedSound, moneySound;

    private void OnEnable()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }
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
    public void PlayRubbishCollectedSound()
    {
        globalAudioSource.PlayOneShot(rubbishCollectedSound);
    }
    public void PlayBeepSound()
    {
        globalAudioSource.PlayOneShot(beepSound);
    }
    public void PlayClickSound()
    {
        globalAudioSource.PlayOneShot(clickSound);
    }
    public void PlaySuccessSound()
    {
        globalAudioSource.PlayOneShot(rubbishCollectedSound);
    }

    public void PlayFailedSound()
    {
        globalAudioSource.PlayOneShot(failedSound);
    }
    public void PlayMoneySound()
    {
        globalAudioSource.PlayOneShot(moneySound);
    }
    public void StopMusic()
    {
        globalAudioSource.Stop();
    }
}