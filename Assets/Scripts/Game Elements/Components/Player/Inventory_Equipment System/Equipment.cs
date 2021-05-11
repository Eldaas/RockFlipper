﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is the runtime object representing an equipment item. Values are generated from the configurable scriptable object files. These items are serialized to JSON.
[System.Serializable]
public class Equipment : IEquippable
{
    // TO DO: Create variables for each of the playerStats types that it may modify
    // (Or create a new PlayerStats object and use that?)

    public EquipmentProfile equipmentProfile;
    public string name;
    public List<EquipmentEffect> effects = new List<EquipmentEffect>();    

    
    #region Properties
    public EquipmentType EquipmentType { get => equipmentProfile.equipmentType; }
    public Sprite EquipmentIcon { get => equipmentProfile.equipmentIcon; }
    #endregion

    #region Public Methods
    public virtual void Equip()
    {
        // Common (all equipment types) equip logic here

        foreach(EquipmentEffect effect in effects)
        {
            effect.ApplyEffect();
        }
    }
    #endregion
}

public enum EquipmentType { None, Shield, Armour, Hull, Engine, Maneuvering, Collector }