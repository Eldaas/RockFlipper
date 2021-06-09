using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfileRecord : MonoBehaviour
{
    public TextMeshProUGUI nameField;
    public TextMeshProUGUI statsField;
    [SerializeField]
    private Button selectButton;
    [SerializeField]
    private Button homeButton;

    #region Unity Methods
    private void Start()
    {
        RegisterListeners();
    }

    private void OnDestroy()
    {
        selectButton.onClick.RemoveAllListeners();
        homeButton.onClick.RemoveAllListeners();
    }
    #endregion

    #region Public Methods
    public void DeselectRecord()
    {
        homeButton.gameObject.SetActive(false);
        selectButton.gameObject.SetActive(true);
    }
    #endregion

    #region Private Methods
    private void RegisterListeners()
    {
        selectButton.onClick.AddListener(SelectRecord);
        homeButton.onClick.AddListener(LoadHangar);
    }

    private void SelectRecord()
    {
        if(IntroMenu.instance.selectedProfileRecord != null)
        {
            IntroMenu.instance.selectedProfileRecord.DeselectRecord();
        }
        
        ProfileManager.instance.LoadProfile(nameField.text);
        IntroMenu.instance.selectedProfileRecord = this;
        selectButton.gameObject.SetActive(false);
        homeButton.gameObject.SetActive(true);
    }

    private void LoadHangar()
    {
        GameManager.instance.LoadLevel(GameStates.Hangar);
    }
    #endregion




}
