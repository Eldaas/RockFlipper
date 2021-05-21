﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile
{
    public string profileName;
    public float balance;
    public float reputation;
    public int numOfDeaths;
    public bool isDead;

    public List<Equipment> currentEquipment = new List<Equipment>();
    public List<Equipment> currentInventory = new List<Equipment>();

    public int shieldModValueIndex = 1;
    public int armourModValueIndex = 1;
    public int hullModValueIndex = 1;
    public int engineModValueIndex = 1;
    public int thrusterModValueIndex = 1;
    public int weaponModValueIndex = 1;
}
