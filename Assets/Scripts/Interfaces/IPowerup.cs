using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerup
{
    float EffectDuration { get; set; }
    float ChanceToSpawn { get; }
    bool IsActive { get; }

    void ExecutePowerup(Player player);

    void EndPowerup(Player player);
}
