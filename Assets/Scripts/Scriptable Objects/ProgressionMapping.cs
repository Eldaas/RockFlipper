using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Progression Mapping/Progression Map Profile")]
public class ProgressionMapping : ScriptableObject
{
    public AnimationCurve deathCostScale;
    public int deathIndexRate;
    public float baseCost;
    public float maxCost;

    public AnimationCurve equipmentCostScale;
    public int equipIndexRate;
    public float basePrice;
    public float maxPrice;
}
