using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Level Powerup Profile")]
public class LevelPowerups : ScriptableObject
{
    [HideInInspector]
    public List<IPowerup> runtimeList = new List<IPowerup>();

    [Header("Powerup One")]
    public Powerup p1;

    [Header("Powerup Two")]
    public Powerup p2;

    [Header("Powerup Three")]
    public Powerup p3;

    [Header("Powerup Four")]
    public Powerup p4;

    [Header("Powerup Five")]
    public Powerup p5;

    [Header("Powerup Six")]
    public Powerup p6;

    [Header("Powerup Seven")]
    public Powerup p7;

    [Header("Powerup Eight")]
    public Powerup p8;

    [Header("Powerup Nine")]
    public Powerup p9;

    public void GenerateRuntimeList()
    {
        if (p1 != null) { runtimeList.Add(p1); }
        if (p2 != null) { runtimeList.Add(p2); }
        if (p3 != null) { runtimeList.Add(p3); }
        if (p4 != null) { runtimeList.Add(p4); }
        if (p5 != null) { runtimeList.Add(p5); }
        if (p6 != null) { runtimeList.Add(p6); }
        if (p7 != null) { runtimeList.Add(p7); }
        if (p8 != null) { runtimeList.Add(p8); }
        if (p9 != null) { runtimeList.Add(p9); }
    }
}
