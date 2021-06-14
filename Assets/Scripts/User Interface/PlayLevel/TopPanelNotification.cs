using System.Collections;
using UnityEngine;
using TMPro;

public class TopPanelNotification : Notification
{
    public override void Deactivate()
    {
        SceneController.instance.sceneUi.tnpList.Remove(this);
        base.Deactivate();
    }

    public override void DeactivateImmediate()
    {
        SceneController.instance.sceneUi.tnpList.Remove(this);
        base.DeactivateImmediate();
    }

}