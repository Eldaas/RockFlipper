using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGuiPanel : MonoBehaviour
{
    private void OnGUI()
    {
        int buttonHeight = Screen.height / 20;
        int buttonWidth = Screen.width / 10;
        int paddingBetween = Screen.width / 100;

        GUIStyle style = GUI.skin.GetStyle("button");
        style.fontSize = 18;

        if(GameManager.instance.devModeEnabled)
        {
            if (GUI.Button(new Rect(20, 40, buttonWidth, buttonHeight), "Give Player Money"))
            {
                DevGiveMoney();
            }

            if (GUI.Button(new Rect(buttonWidth + paddingBetween, 40, buttonWidth, buttonHeight), "Clear Equipment"))
            {
                DevClearEquipment();
            }

            if (GUI.Button(new Rect(buttonWidth * 2 + paddingBetween, 40, buttonWidth, buttonHeight), "Wipe Profile"))
            {
                WipeProfile();
            }

        }
    }

    private void DevGiveMoney()
    {
        ProfileManager.instance.currentProfile.balance += 100000;
        EventManager.TriggerEvent("UpdateBalance");
        ProfileManager.instance.SaveProfile();
    }

    private void DevClearEquipment()
    {
        ProfileManager.instance.currentProfile.currentEquipment.Clear();
        ProfileManager.instance.currentProfile.currentInventory.Clear();

        EventManager.TriggerEvent("UpdateInventory");
        EventManager.TriggerEvent("UpdateEquipmentSlots");
        ProfileManager.instance.SaveProfile();
    }

    private void WipeProfile()
    {
        PlayerProfile profile = ProfileManager.instance.currentProfile;

        profile.balance = 0f;
        profile.reputation = 0f;
        profile.numOfDeaths = 0;
        profile.currentEquipment.Clear();
        profile.currentInventory.Clear();

        profile.shieldModValueIndex = 1;
        profile.armourModValueIndex = 1;
        profile.hullModValueIndex = 1;
        profile.engineModValueIndex = 1;
        profile.thrusterModValueIndex = 1;
        profile.weaponModValueIndex = 1;

        ProfileManager.instance.SaveProfile();
        EventManager.TriggerEvent("UpdateBalance");
        EventManager.TriggerEvent("UpdateInventory");
        EventManager.TriggerEvent("UpdateEquipmentSlots");
        EventManager.TriggerEvent("UpdateStats");
        EventManager.TriggerEvent("UpdateModulePrices");        
    }
}
