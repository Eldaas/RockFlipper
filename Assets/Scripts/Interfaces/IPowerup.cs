using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPowerup
{
    float EffectDuration { get; set; }
    GameObject Prefab { get; set; }

    void ExecutePowerup();
}
