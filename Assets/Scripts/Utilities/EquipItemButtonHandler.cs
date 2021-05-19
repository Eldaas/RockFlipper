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
        Equipment equipment = GetComponent<AssociatedEquipment>().equipment;

        if (clickCount == 1)
        {
            HangarController.instance.hangarUi.EquipmentItemSelected(equipment);
        }
        else if (clickCount == 2)
        {
            Debug.Log("Double-click registered.");
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
