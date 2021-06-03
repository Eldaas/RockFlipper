using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoresUI : UIController
{
    [Header("General Refs")]
    [SerializeField]
    private HighScores scores;
    [SerializeField]
    private GameObject recordPrefab;
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private Button closeButton;

    [Header("Your Data")]
    [SerializeField]
    private TextMeshProUGUI nameField;
    [SerializeField]
    private TextMeshProUGUI wealthField;
    [SerializeField]
    private TextMeshProUGUI timeField;

    [Header("Search")]
    [SerializeField]
    private TMP_Dropdown dropdown;
    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private Button resetButton;
    [SerializeField]
    private Button searchButton;

    [Header("Leaderboard")]
    [SerializeField]
    private GameObject wealthBoard;
    [SerializeField]
    private GameObject timesBoard;
    [SerializeField]
    private Button switchBoardButton;
    [SerializeField]
    private TextMeshProUGUI switchBoardButtonText;
    [SerializeField]
    private Transform wealthContent;
    [SerializeField]
    private Transform timesContent;

    // Misc
    private enum Leaderboard { Wealth, Times }
    private Leaderboard currentBoard = Leaderboard.Wealth;

    #region Unity Methods
    private void Awake()
    {
        
    }

    private void Start()
    {
        RegisterListeners();
        Initialise();
        StartCoroutine(PopulateData());
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private & Protected Methods
    protected override void RegisterListeners()
    {
        // Buttons
        closeButton.onClick.AddListener(delegate { GameManager.instance.LoadLevel(GameStates.IntroMenu); });
        switchBoardButton.onClick.AddListener(SwitchBoard);
    }

    /// <summary>
    /// Clears all of the records already within the UI lists.
    /// </summary>
    private void ClearRecords()
    {
        foreach(Transform transform in wealthContent.transform)
        {
            Destroy(transform.gameObject);
        }

        foreach(Transform transform in timesContent.transform)
        {
            Destroy(transform.gameObject);
        }
    }

    /// <summary>
    /// Initialises UI elements with default data.
    /// </summary>
    private void Initialise()
    {
        ValidateInputs();

        nameField.text = $"Name: {ProfileManager.instance.currentProfile.profileName}";
        wealthField.text = $"Wealth: {ProfileManager.instance.currentProfile.balance}";
        timeField.text = $"Time Played: {ProfileManager.instance.currentProfile.totalPlayTime} seconds";
    }

    /// <summary>
    /// Coroutine to load the data from the server and have the game wait while this happens.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PopulateData()
    {
        Transform loading = wealthContent.transform.GetChild(0);
        TextMeshProUGUI loadingText = loading.GetComponent<TextMeshProUGUI>();
        string baseString = "Loading data...";
        float nextTime = 0f;
        int dots = 0;

        while (!scores.dataRetrieved)
        {
            if(Time.time >= nextTime)
            {
                nextTime = Time.time + 1f;
                if (dots < 3)
                {
                    loadingText.text += ".";
                    dots++;
                }
                else
                {
                    dots = 0;
                    loadingText.text = baseString;
                }
            }
            
            yield return new WaitForEndOfFrame();
        }

        ClearRecords();

        LinkedListNode<HighScores.DreamloData.Dreamlo.Leaderboard.HighScoreRecord> scoreNode = scores.ascendingScores.Last;
        LinkedListNode<HighScores.DreamloData.Dreamlo.Leaderboard.HighScoreRecord> timeNode = scores.ascendingTimes.Last;
        Debug.Log($"Scores count: {scores.ascendingScores.Count}, Times count: {scores.ascendingTimes.Count}");
        Debug.Log($"Last time node: {scores.ascendingTimes.Last.Value.name}");

        for (int i = 0; i < scores.data.dreamlo.leaderboard.entry.Count; i++)
        {
            Debug.Log($"Record: {timeNode.Value.name}");
            GameObject wealthLine = Instantiate(recordPrefab, wealthContent.transform);
            GameObject timeLine = Instantiate(recordPrefab, timesContent.transform);

            TextMeshProUGUI wealthLineText = wealthLine.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI timeLineText = timeLine.GetComponent<TextMeshProUGUI>();

            wealthLineText.text = $"Rank #{i + 1} - {scoreNode.Value.name} - Wealth: {scoreNode.Value.score}";
            timeLineText.text = $"Rank #{i + 1} - {timeNode.Value.name} - Time Played: {timeNode.Value.seconds} seconds";
            

            scoreNode = scoreNode.Previous;
            timeNode = scoreNode.Previous;
        }

        ValidateInputs();
    }

    /// <summary>
    /// Ensures buttons are inactive before the data is loaded.
    /// </summary>
    private void ValidateInputs()
    {
        switchBoardButton.interactable = scores.dataRetrieved;
        resetButton.interactable = scores.dataRetrieved;
        searchButton.interactable = scores.dataRetrieved;
        inputField.interactable = scores.dataRetrieved;
        dropdown.interactable = scores.dataRetrieved;
    }

    /// <summary>
    /// Toggles between the online wealth and time leaderboards.
    /// </summary>
    private void SwitchBoard()
    {
        if(currentBoard == Leaderboard.Wealth)
        {
            wealthBoard.SetActive(false);
            timesBoard.SetActive(true);
            switchBoardButtonText.text = "View Wealth Rankings";
            currentBoard = Leaderboard.Times;
        }
        else
        {
            wealthBoard.SetActive(true);
            timesBoard.SetActive(false);
            switchBoardButtonText.text = "View Time Rankings";
            currentBoard = Leaderboard.Wealth;
        }
    }
    #endregion
}

