using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScoop : MonoBehaviour
{

    public AudioClip crystalCollectSound;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Crystal"))
        {
            GameManager.instance.record.AddToScore(25);
            AudioManager.instance.PlayOneShot(crystalCollectSound);
            other.gameObject.SetActive(false);
        }
    }
}
