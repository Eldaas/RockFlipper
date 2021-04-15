using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class CreateFromTemplate
{
    private static readonly string path = "Assets/Scripts/Editor/ScriptTemplates";

    [MenuItem(itemName: "Assets/Create/Script Templates/New Powerup Type", isValidateFunction: false, priority: 51)]
    public static void NewPowerup()
    {
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile($"{path}/NewPowerupType.cs.txt", "NewPowerupType.cs");
    }
}
