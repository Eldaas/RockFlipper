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
        clickCount = eventData.clickCount;
        Equipment equipment = GetComponent<AssociatedEquipment>().equipment;
        bool isInSlot = false;

        if (CompareTag("EquipmentSlot")) isInSlot = true;
        else if (CompareTag("EquipmentItem")) isInSlot = false;

        HangarController.instance.hangarUi.EquipmentItemSelected(equipment, isInSlot);

        if (clickCount == 2)
        {

            HangarController.instance.hangarUi.equipmentStatsModal.SetActive(false);

            if (isInSlot)
            {
                EquipmentManager.instance.UnequipItem(equipment, true);
                tag = "EquipmentItem";
            }
            else
            {
                EquipmentManager.instance.EquipItem(equipment);
                tag = "EquipmentSlot";
            }

            ProfileManager.instance.SaveProfile();
            
        }

    }
}
