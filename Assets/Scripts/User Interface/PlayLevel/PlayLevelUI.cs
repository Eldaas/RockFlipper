using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayLevelUI : UIController
{
    [Header("UI Elements")]
    [SerializeField]
    private Button returnToBaseButton;

    private void Awake()
    {
        RegisterListeners();
    }

    void Start()
    {
        
    }

    #region Protected & Private Methods
    protected override void RegisterListeners()
    {
        returnToBaseButton.onClick.AddListener(ReturnToBase);
    }

    private void ReturnToBase()
    {
        UIManager.instance.LoadScreen(true);
        EventManager.TriggerEvent("ReturningToBase");
        StartCoroutine("EndScene");
    }

    private IEnumerator EndScene()
    {
        while(UIManager.instance.LoadScreenAlpha > 0f)
        {
            yield return new WaitForEndOfFrame();
        }

        GameManager.instance.LoadLevel(GameStates.Hangar);
    }
    #endregion

}
