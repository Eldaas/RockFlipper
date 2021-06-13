using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        ParticleSystem ps = other.GetComponent<ParticleSystem>();
        List<ParticleCollisionEvent> events = new List<ParticleCollisionEvent>();
        ps.GetCollisionEvents(other, events);
        int numEvents = events.Count;
        string psTag = ps.tag;

        switch(psTag)
        {
            case "Iron":
                GameManager.instance.levelRecord.ironCollected += numEvents;
                break;
            case "Silver":
                GameManager.instance.levelRecord.silverCollected += numEvents;
                break;
            case "Gold":
                GameManager.instance.levelRecord.goldCollected += numEvents;
                break;
            default:
                Debug.LogError("ResourceCollector => No tags matched.");
                break;
        }

        other.SetActive(false);
        EventManager.TriggerEvent("ResourceCollected");
    }
}
