using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndLevelUI : UIController
{
    [Header("UI Elements")]
    [SerializeField]
    private Button okButton;

    private void Awake()
    {
        RegisterListeners();
    }

    protected override void RegisterListeners()
    {
        okButton.onClick.AddListener(CloseWindow);
    }

    private void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}
