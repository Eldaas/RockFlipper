using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Level Powerups Profile")]
public class LevelPowerups : ScriptableObject
{
    public List<GameObject> powerups;
}
