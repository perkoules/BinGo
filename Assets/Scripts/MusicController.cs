using System;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource globalAudioSource;

    public AudioClip beepSound, rubbishCollectedSound, clickSound, failedSound, moneySound;

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