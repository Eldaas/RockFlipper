using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGuiPanel : MonoBehaviour
{
    [SerializeField]
    private int topOffset;
    [SerializeField]
    private int leftOffset;
    [SerializeField]
    private int buttonWidth;
    [SerializeField]
    private int buttonHeight;
    [SerializeField]
    private int paddingBetween;


    private void OnGUI()
    {
        GUIStyle style = GUI.skin.GetStyle("button");
        style.fontSize = 18;

        if(GameManager.instance.devModeEnabled)
        {
            if (GUI.Button(new Rect(leftOffset, topOffset, buttonWidth, buttonHeight), "Give Player Money"))
            {
                DevGiveMoney();
            }

            if (GUI.Button(new Rect(leftOffset + buttonWidth + paddingBetween, topOffset, buttonWidth, buttonHeight), "Clear Equipment"))
            {
                DevClearEquipment();
            }

            if (GUI.Button(new Rect(leftOffset + buttonWidth * 2 + paddingBetween, topOffset, buttonWidth, buttonHeight), "Wipe Profile"))
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
