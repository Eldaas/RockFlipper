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

    public static void QuitGame()
    {
        Application.Quit();
    }

}
