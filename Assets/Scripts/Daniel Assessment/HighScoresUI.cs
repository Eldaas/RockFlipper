using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScoresUI : UIController
{
    #region Fields
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

    [Header("Hashing")]
    [SerializeField]
    private TMP_InputField hashInput;
    [SerializeField]
    private Button hashResetButton;
    [SerializeField]
    private Button hashHashButton;

    [Header("Search")]
    [SerializeField]
    private TMP_Dropdown dropdown;
    [SerializeField]
    private TMP_InputField searchInput;
    [SerializeField]
    private Button searchResetButton;
    [SerializeField]
    private Button searchButton;

    [Header("Leaderboard")]
    [SerializeField]
    private GameObject wealthBoard;
    [SerializeField]
    private GameObject timesBoard;
    [SerializeField]
    private GameObject resultsBoard;
    [SerializeField]
    private Button switchBoardButton;
    [SerializeField]
    private TextMeshProUGUI switchBoardButtonText;
    [SerializeField]
    private Transform wealthContent;
    [SerializeField]
    private Transform timesContent;
    [SerializeField]
    private Transform resultsContent;

    // Misc
    private enum Leaderboard { Wealth, Times, Results }
    private Leaderboard currentBoard = Leaderboard.Wealth;
    #endregion

    #region Unity Methods
    private void Start()
    {
        RegisterListeners();
        Initialise();
        StartCoroutine(PopulateData());
    }
    #endregion

    #region Private & Protected Methods
    /// <summary>
    /// Registers event listeners for UI onClick events, as well as any custom events within the EventManager.
    /// </summary>
    protected override void RegisterListeners()
    {
        closeButton.onClick.AddListener(delegate { GameManager.instance.LoadLevel(GameStates.IntroMenu); });
        switchBoardButton.onClick.AddListener(delegate { SwitchBoard(Leaderboard.Wealth); });
        searchButton.onClick.AddListener(SearchQuery);
        searchResetButton.onClick.AddListener(ResetSearch);
        hashResetButton.onClick.AddListener(ResetHash);
        hashHashButton.onClick.AddListener(HashString);
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

        ResetSearch();
    }

    /// <summary>
    /// Initialises UI elements with default data.
    /// </summary>
    private void Initialise()
    {
        //ValidateInputs();

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

        int i = 0;
        foreach(HighScores.DreamloData.Dreamlo.Leaderboard.HighScoreRecord record in scores.data.dreamlo.leaderboard.entry)
        {
            i++;

            GameObject wealthLine = Instantiate(recordPrefab, wealthContent.transform);
            GameObject timeLine = Instantiate(recordPrefab, timesContent.transform);

            TextMeshProUGUI wealthLineText = wealthLine.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI timeLineText = timeLine.GetComponent<TextMeshProUGUI>();

            wealthLineText.text = $"Rank #{i} - {scoreNode.Value.name} - Wealth: {scoreNode.Value.score}";
            timeLineText.text = $"Rank #{i} - {timeNode.Value.name} - Time Played: {timeNode.Value.seconds} seconds";

            scoreNode = scoreNode.Previous;
            timeNode = timeNode.Previous;
        }

        ValidateInputs();
    }

    /// <summary>
    /// Ensures buttons are inactive before the data is loaded.
    /// </summary>
    private void ValidateInputs()
    {
        switchBoardButton.interactable = scores.dataRetrieved;
        searchResetButton.interactable = scores.dataRetrieved;
        searchButton.interactable = scores.dataRetrieved;
        searchInput.interactable = scores.dataRetrieved;
        dropdown.interactable = scores.dataRetrieved;
        hashInput.interactable = scores.dataRetrieved;
        hashHashButton.interactable = scores.dataRetrieved;
        hashResetButton.interactable = scores.dataRetrieved;
    }

    /// <summary>
    /// Toggles between the online wealth and time leaderboards.
    /// </summary>
    private void SwitchBoard(Leaderboard board)
    {
        if(currentBoard != board)
        {
            if (board == Leaderboard.Wealth)
            {
                ResetSearch();
                wealthBoard.SetActive(false);
                timesBoard.SetActive(true);
                resultsBoard.SetActive(false);
                switchBoardButtonText.text = "View Wealth Rankings";
                currentBoard = board;
                switchBoardButton.onClick.RemoveAllListeners();
                switchBoardButton.onClick.AddListener(delegate { SwitchBoard(Leaderboard.Times); });
            }
            else if (board == Leaderboard.Times)
            {
                ResetSearch();
                wealthBoard.SetActive(true);
                timesBoard.SetActive(false);
                resultsBoard.SetActive(false);
                switchBoardButtonText.text = "View Time Rankings";
                currentBoard = board;
                switchBoardButton.onClick.RemoveAllListeners();
                switchBoardButton.onClick.AddListener(delegate { SwitchBoard(Leaderboard.Wealth); });

            }
            else if (board == Leaderboard.Results)
            {
                wealthBoard.SetActive(false);
                timesBoard.SetActive(false);
                resultsBoard.SetActive(true);
                switchBoardButtonText.text = "Return to Rankings";
                currentBoard = board;
                switchBoardButton.onClick.RemoveAllListeners();
                switchBoardButton.onClick.AddListener(delegate { SwitchBoard(Leaderboard.Wealth); });
            }
        }
    }

    /// <summary>
    /// Executes the search routine, calling the relevant function for the search method the user has selected.
    /// </summary>
    private void SearchQuery()
    {
        if(dropdown.value == 0) // Search by name
        {
            SearchByName(searchInput.text);
        }
        else if(dropdown.value == 1) // Search by rank number
        {
            int result;
            bool parseSuccess = int.TryParse(searchInput.text, out result);
            if (parseSuccess)
            {
                SearchByRank(result);
            }
            else
            {
                Debug.Log("User submitted a query which couldn't be parsed to an integer: " + searchInput.text);
                EventManager.TriggerEvent("IncorrectInput");
            }
        }
        else if(dropdown.value == 2) // Search by score
        {
            int result;
            bool parseSuccess = int.TryParse(searchInput.text, out result);
            if(parseSuccess)
            {
                SearchByScore(result);
            }
            else
            {
                Debug.Log("User submitted a query which couldn't be parsed to an integer: " + searchInput.text);
                EventManager.TriggerEvent("IncorrectInput");
            }
        }
    }

    /// <summary>
    /// Uses LinearSearch from C++ DLL to find a certain score within the sorted scores array, and then gets the relevant data object from the binary tree (as the index of the array == the index of the binary tree).
    /// </summary>
    /// <param name="score"></param>
    private void SearchByScore(int score)
    {
        string query = searchInput.text;
        int arrIndex = HighScores.LinearSearch(scores.scores, scores.scores.Length, score);

        if(arrIndex != -1)
        {
            BinaryTree.BinaryTreeNode node = scores.dataTree.Find(arrIndex);

            if(node != null)
            {
                GameObject newRecord = Instantiate(recordPrefab, resultsContent.transform);
                newRecord.GetComponent<TextMeshProUGUI>().text = $"Rank #{arrIndex + 1} - Name: {node.data.name} || Score: {node.data.score} || Total Time: {node.data.seconds} seconds";
                SwitchBoard(Leaderboard.Results);
            }
            else
            {
                EventManager.TriggerEvent("NoResults");
            }
        }
        else
        {
            EventManager.TriggerEvent("NoResults");
        }
    }

    /// <summary>
    /// Uses a comparator pattern (Find) to check if the input string matches the record string.
    /// </summary>
    /// <param name="name"></param>
    private void SearchByName(string name)
    {
        BinaryTree.BinaryTreeNode result = scores.dataTree.Find(name);

        if(result != null)
        {
            GameObject newRecord = Instantiate(recordPrefab, resultsContent.transform);
            newRecord.GetComponent<TextMeshProUGUI>().text = $"Rank #{result.index + 1} - Name: {result.data.name} || Score: {result.data.score} || Total Time: {result.data.seconds} seconds";
            SwitchBoard(Leaderboard.Results);
        }
        else
        {
            EventManager.TriggerEvent("NoResults");
        }
    }

    /// <summary>
    /// Searches the binary tree for the rank the user has submitted. This equals the index of the binary tree - 1, as the binary tree is zero-based.
    /// </summary>
    /// <param name="rank">The rank being searched for within the binary tree.</param>
    private void SearchByRank(int rank)
    {
        BinaryTree.BinaryTreeNode result = scores.dataTree.Find(rank - 1);

        if(result != null)
        {
            GameObject newRecord = Instantiate(recordPrefab, resultsContent.transform);
            newRecord.GetComponent<TextMeshProUGUI>().text = $"Rank #{result.index + 1} - Name: {result.data.name} || Score: {result.data.score} || Total Time: {result.data.seconds} seconds";
            SwitchBoard(Leaderboard.Results);
        }
        else
        {
            EventManager.TriggerEvent("NoResults");
        }
    }

    /// <summary>
    /// Resets all search UI components.
    /// </summary>
    private void ResetSearch()
    {
        searchInput.text = string.Empty;
        ClearResults();
    }

    /// <summary>
    /// Resets all hashing UI components.
    /// </summary>
    private void ResetHash()
    {
        hashInput.text = string.Empty;
        ClearResults();
    }

    /// <summary>
    /// Removes all results from the results window.
    /// </summary>
    private void ClearResults()
    {
        foreach (Transform child in resultsContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// Calls the hashing utility method to convert an input string into an integer hash. This is returned and output as a result on the results board.
    /// </summary>
    private void HashString()
    {
        int result = HashUtility.HashString(hashInput.text);
        GameObject newResult = Instantiate(recordPrefab, resultsContent);
        TextMeshProUGUI text = newResult.GetComponent<TextMeshProUGUI>();
        text.text = result.ToString();
        SwitchBoard(Leaderboard.Results);
    }
    #endregion
}