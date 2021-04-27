using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerup
{
    float EffectDuration { get; set; }
    GameObject Prefab { get; set; }
    float ChanceToSpawn { get; }

    void ExecutePowerup(Player player);
}
