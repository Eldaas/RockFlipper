using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{

    public static int GenerateRandomInt(int min, int max)
    {
        return Random.Range(min, max);
    }

    public static float GenerateRandomFloat(float min, float max)
    {
        return Random.Range(min, max);
    }

    public static Vector3 GenerateRandomOffset(Vector3 minOffSet, Vector3 maxOffset)
    {
        float randomX = GenerateRandomFloat(minOffSet.x, maxOffset.x);
        float randomY = GenerateRandomFloat(minOffSet.y, maxOffset.y);
        float randomZ = GenerateRandomFloat(minOffSet.z, maxOffset.z);
        return new Vector3(randomX, randomY, randomZ);
    }

    public static Vector3 GenerateRandomOffset(Vector3 maxOffset)
    {
        float randomX = GenerateRandomFloat(0f, maxOffset.x);
        float randomY = GenerateRandomFloat(0f, maxOffset.y);
        float randomZ = GenerateRandomFloat(0f, maxOffset.z);
        return new Vector3(randomX, randomY, randomZ);
    }

    public static void QuitGame()
    {
        Application.Quit();
    }

}
