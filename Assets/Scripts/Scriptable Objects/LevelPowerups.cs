using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Data/Level GameObject Profile")]
public class LevelPowerups : ScriptableObject
{
    [HideInInspector]
    public List<GameObject> runtimeList = new List<GameObject>();

    [Header("GameObject One")]
    public GameObject p1;

    [Header("GameObject Two")]
    public GameObject p2;

    [Header("GameObject Three")]
    public GameObject p3;

    [Header("GameObject Four")]
    public GameObject p4;

    [Header("GameObject Five")]
    public GameObject p5;

    [Header("GameObject Six")]
    public GameObject p6;

    [Header("GameObject Seven")]
    public GameObject p7;

    [Header("GameObject Eight")]
    public GameObject p8;

    [Header("GameObject Nine")]
    public GameObject p9;

    public void GenerateRuntimeList()
    {
        if (p1 != null) { runtimeList.Add(p1); }
        if (p2 != null) { runtimeList.Add(p2); }
        if (p3 != null) { runtimeList.Add(p3); }
        if (p4 != null) { runtimeList.Add(p4); }
        if (p5 != null) { runtimeList.Add(p5); }
        if (p6 != null) { runtimeList.Add(p6); }
        if (p7 != null) { runtimeList.Add(p7); }
        if (p8 != null) { runtimeList.Add(p8); }
        if (p9 != null) { runtimeList.Add(p9); }
    }
}
