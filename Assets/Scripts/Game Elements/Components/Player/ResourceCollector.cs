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

        Debug.Log("Player collected " + numEvents + " particles!");

        
    }
}
