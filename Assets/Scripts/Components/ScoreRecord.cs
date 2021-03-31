using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreRecord : MonoBehaviour
{
    public int currentScore = 0;

    public void AddToScore(int value)
    {
        currentScore += value;
    }
}
