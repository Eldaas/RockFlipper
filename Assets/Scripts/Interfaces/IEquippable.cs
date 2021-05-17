using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IEquippable
{
    EquipmentType EquipmentType { get; }
    Sprite EquipmentIcon { get; }
    void Equip();
}
