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
    public int p1NumberToSpawn;

    [Header("Powerup Two")]
    public Powerup p2;
    public int p2NumberToSpawn;

    [Header("Powerup Three")]
    public Powerup p3;
    public int p3NumberToSpawn;

    [Header("Powerup Four")]
    public Powerup p4;
    public int p4NumberToSpawn;

    [Header("Powerup Five")]
    public Powerup p5;
    public int p5NumberToSpawn;

    [Header("Powerup Six")]
    public Powerup p6;
    public int p6NumberToSpawn;

    [Header("Powerup Seven")]
    public Powerup p7;
    public int p7NumberToSpawn;

    [Header("Powerup Eight")]
    public Powerup p8;
    public int p8NumberToSpawn;

    [Header("Powerup Nine")]
    public Powerup p9;
    public int p9NumberToSpawn;

    private void Awake()
    {
        if (p1 != null) { runtimeList.Add(p1 as IPowerup); }
        if (p2 != null) { runtimeList.Add(p2 as IPowerup); }
        if (p3 != null) { runtimeList.Add(p3 as IPowerup); }
        if (p4 != null) { runtimeList.Add(p4 as IPowerup); }
        if (p5 != null) { runtimeList.Add(p5 as IPowerup); }
        if (p6 != null) { runtimeList.Add(p6 as IPowerup); }
        if (p7 != null) { runtimeList.Add(p7 as IPowerup); }
        if (p8 != null) { runtimeList.Add(p8 as IPowerup); }
        if (p9 != null) { runtimeList.Add(p9 as IPowerup); }

        runtimeList.Add(p2 as IPowerup);
        runtimeList.Add(p3 as IPowerup);
        runtimeList.Add(p4 as IPowerup);
        runtimeList.Add(p5 as IPowerup);
        runtimeList.Add(p6 as IPowerup);
        runtimeList.Add(p7 as IPowerup);
        runtimeList.Add(p8 as IPowerup);
        runtimeList.Add(p9 as IPowerup);
    }
}
