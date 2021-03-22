using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;
    public AudioClip crystalCollect;

    private void Awake()
    {
        #region Singleton
        AudioManager[] list = FindObjectsOfType<AudioManager>();
        if (list.Length > 1)
        {
            Destroy(this);
            Debug.Log("Multiple instances of the Audio Manager component detected. Destroying an instance.");
        }
        else
        {
            instance = this;
        }
        #endregion
    }

    public void PlayOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
