using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : IEquippable
{
    // TO DO: Create variables for each of the playerStats types that it may modify
    // (Or create a new PlayerStats object and use that?)

    #region Properties
    public EquipmentType EquipmentType { get => EquipmentType; set => EquipmentType = value; }
    #endregion


}

public enum EquipmentType { None, Shield, Armour, Hull, Engine, Maneuvering, Collector }