using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipItemButtonHandler : MonoBehaviour, IPointerClickHandler
{
    int clickCount;

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.TriggerEvent("UIClick");
        clickCount = eventData.clickCount;
        Equipment equipment = GetComponent<AssociatedEquipment>().equipment;

        HangarController.instance.hangarUi.EquipmentItemSelected(equipment, equipment.isEquipped);

        if (clickCount == 2)
        {
            HangarController.instance.hangarUi.equipmentStatsModal.SetActive(false);

            if (equipment.isEquipped)
            {
                EquipmentManager.instance.UnequipItem(equipment, true);
            }
            else
            {
                EquipmentManager.instance.EquipItem(equipment);
            }            
        }

    }
}
