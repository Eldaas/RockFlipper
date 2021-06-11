using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Audio
{
    public static void UISound(UISounds soundtype)
    {
        switch(soundtype)
        {
            case UISounds.Click:
                EventManager.TriggerEvent("UIClick");
                break;
            case UISounds.Release:
                EventManager.TriggerEvent("UIRelease");
                break;
            case UISounds.Select:
                EventManager.TriggerEvent("UISelect");
                break;
            case UISounds.Success:
                EventManager.TriggerEvent("UISuccess");
                break;
            case UISounds.Error:
                EventManager.TriggerEvent("UIError");
                break;
            case UISounds.Notification:
                EventManager.TriggerEvent("UINotification");
                break;
            case UISounds.Equip:
                EventManager.TriggerEvent("UIEquip");
                break;
            case UISounds.Destroy:
                EventManager.TriggerEvent("UIDestroy");
                break;
        }
    }
}

public enum UISounds { Click, Release, Select, Success, Error, Notification, Equip, Destroy }