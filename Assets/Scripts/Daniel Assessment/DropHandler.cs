using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData data)
    {
        EventManager.TriggerEvent("UIRelease");
        AssociatedEquipment assEquip = data.pointerDrag.GetComponent<AssociatedEquipment>();
        Equipment equipment = assEquip.equipment;

        HangarController.instance.hangarUi.equipmentStatsModal.SetActive(false);

        if (equipment.isEquipped && !CompareTag("EquipmentSlot")) // If the item is equipped and icon has been dropped on inventory
        {
            EquipmentManager.instance.UnequipItem(equipment, true);
            Destroy(assEquip.gameObject);
        }
        else if (!equipment.isEquipped && CompareTag("EquipmentSlot")) // If the item is not equipped and icon has been dropped on equipment slot
        {
            EquipmentManager.instance.EquipItem(equipment);
            Destroy(assEquip.gameObject);
        }
    }
}
