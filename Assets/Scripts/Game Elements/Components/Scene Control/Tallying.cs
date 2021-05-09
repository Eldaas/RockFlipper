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
    private float timeBetweenDrops;

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
        
    }

    private void Update()
    {
        
    }

    public void StartSequence()
    {
        ironCollected = GameManager.instance.levelRecord.ironCollected;
        silverCollected = GameManager.instance.levelRecord.silverCollected;
        goldCollected = GameManager.instance.levelRecord.goldCollected;

        StartCoroutine(DropResources());
    }

    private IEnumerator DropResources()
    {
        int randomInt = Utility.GenerateRandomInt(minToDrop, maxToDrop);

        while (ironCollected > 0)
        {
            ironParticles.Emit(randomInt);
            ironCollected -= randomInt;
            yield return new WaitForSeconds(timeBetweenDrops);
        }

        while (silverCollected > 0)
        {
            silverParticles.Emit(randomInt);
            silverCollected -= randomInt;
            yield return new WaitForSeconds(timeBetweenDrops);
        }

        while (goldCollected > 0)
        {
            goldParticles.Emit(randomInt);
            goldCollected -= randomInt;
            yield return new WaitForSeconds(timeBetweenDrops);
        }
    }

}
