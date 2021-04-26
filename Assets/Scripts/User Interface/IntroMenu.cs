using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroMenu : UIController
{
    [SerializeField]
    private Button visitHangarButton;
    [SerializeField]
    private Button selectProfileButton;
    [SerializeField]
    private Button visitProfileButton;
    [SerializeField]
    private Button highScoresButton;
    [SerializeField]
    private Button exitGameButton;

    private void Awake()
    {
        RegisterListeners();
    }

    protected override void RegisterListeners()
    {
        visitHangarButton.onClick.AddListener(VisitHangar);
        

        selectProfileButton.onClick.AddListener(SelectProfile);
        visitProfileButton.onClick.AddListener(VisitProfile);
        highScoresButton.onClick.AddListener(HighScores);
        exitGameButton.onClick.AddListener(ExitGame);
        
    }

    #region Private Methods
    private void ButtonValidation()
    {

    }

    private void VisitHangar()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");

        // Temporarily routes to level one
        GameManager.instance.LoadLevel(GameStates.LevelOne);
        
    }

    private void SelectProfile()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
    }

    private void VisitProfile()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
    }

    private void HighScores()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
    }

    private void ExitGame()
    {
        EventManager.TriggerEvent("UIButtonOptionSelected");
        Application.Quit();
    }
    #endregion
}
