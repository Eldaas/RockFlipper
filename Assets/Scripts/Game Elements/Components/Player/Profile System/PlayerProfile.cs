using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile
{
    public string profileName;
    public float balance;
    public float reputation;
    public bool isDead;

    public List<Equipment> currentEquipment = new List<Equipment>();
    public List<Equipment> currentInventory = new List<Equipment>();

    public float shieldModPrice = 10000;
    public float armourModPrice = 10000;
    public float hullModPrice = 10000;
    public float engineModPrice = 10000;
    public float thrusterModPrice = 10000;
    public float weaponModPrice = 10000;
    public float deathCost = 1000;
}
