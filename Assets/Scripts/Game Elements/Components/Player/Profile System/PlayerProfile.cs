﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile
{
    public string profileName;
    public float balance;
    public float reputation;

    public List<Equipment> currentEquipment = new List<Equipment>();
<<<<<<< HEAD:Assets/Scripts/Game Elements/Components/Player/Profile System/PlayerProfile.cs
    public List<Equipment> shopEquipment = new List<Equipment>();
=======
    public List<Equipment> currentInventory = new List<Equipment>();
>>>>>>> implement-inventory-equipment:Assets/Scripts/Game Elements/Components/Profile System/PlayerProfile.cs
}
