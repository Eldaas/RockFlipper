using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tallying : MonoBehaviour
{
    [SerializeField]
    private int minToDrop;
    [SerializeField]
    private int maxToDrop;
    [SerializeField]
    private float deltaTime;
    private float timeBetweenDrops;
    private float timeSinceLastDrop;

    [SerializeField]
    private ParticleSystem ironParticles;
    private int ironCollected;
    
    [SerializeField]
    private ParticleSystem silverParticles;
    private int silverCollected;

    [SerializeField]
    private ParticleSystem goldParticles;
    private int goldCollected;

    private void Start()
    {
        deltaTime = Time.time;
    }

    private void Update()
    {
        deltaTime += Time.deltaTime;
    }

    public void StartSequence()
    {
        ironCollected = GameManager.instance.levelRecord.ironCollected;
        silverCollected = GameManager.instance.levelRecord.silverCollected;
        goldCollected = GameManager.instance.levelRecord.goldCollected;
        

    }

    private IEnumerator DropResources()
    {
        while(ironCollected > 0)
        {
            if(Time.time > deltaTime + timeBetweenDrops)
            {
                int randomInt = Utility.GenerateRandomInt(minToDrop, maxToDrop);
                ParticleSystem.Burst burst = ironParticles.emission.GetBurst(0);
                burst.count = randomInt;
                ironParticles.emission.SetBurst(0, burst);
            }
            


            yield return null;
        }

        while(silverCollected > 0)
        {
            yield return null;
        }

        while(goldCollected > 0)
        {
            yield return null;
        }
    }

    private void SetBurstTime(ParticleSystem particles, float repeatRate)
    {
        ParticleSystem.Burst burst = particles.emission.GetBurst(0);

    }

}
