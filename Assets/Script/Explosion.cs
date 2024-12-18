using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;

    private bool isFirstOnEable;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        isFirstOnEable = true;
    }

    private void OnEnable()
    {
        if (!isFirstOnEable)
            PlaySounds();

        isFirstOnEable = false;
    }

    private void PlaySounds()
    {
        if(audioSource.clip != null)
            audioSource.PlayOneShot(clip);
    }
}
