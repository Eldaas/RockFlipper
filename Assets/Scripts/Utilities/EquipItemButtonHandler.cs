using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipItemButtonHandler : MonoBehaviour, IPointerClickHandler
{
    int clickCount;

    public void OnPointerClick(PointerEventData eventData)
    {
        clickCount = eventData.clickCount;

        if (clickCount == 2)
        {
            Equipment equipment = GetComponent<AssociatedEquipment>().equipment;

            if (CompareTag("EquipmentItem"))
            {
                EquipmentManager.instance.playerInventory.Remove(equipment);
                EquipmentManager.instance.playerEquipment.Add(equipment);
                tag = "EquipmentSlot";
            }
            else if (CompareTag("EquipmentSlot"))
            {
                EquipmentManager.instance.playerInventory.Add(equipment);
                EquipmentManager.instance.playerEquipment.Remove(equipment);
                tag = "EquipmentItem";
            }

            ProfileManager.instance.SaveProfile();
            EventManager.TriggerEvent("UpdateEquipmentSlots");
            EventManager.TriggerEvent("UpdateInventory");
        }

    }
}
