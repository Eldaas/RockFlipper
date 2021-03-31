using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Data/Player Health Container")]
public class PlayerHealth : ScriptableObject
{
    public float hull;
    public float armour;
    public float shields;
}
