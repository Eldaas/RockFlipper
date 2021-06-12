using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public EquipmentType slotType;
    public AssociatedEquipment associatedEquipment;
    public GameObject placeholderImage;
    [SerializeField]
    private GameObject equipmentItemPrefab;

}
