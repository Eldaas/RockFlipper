using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

[DefaultExecutionOrder(-1)]
public class HighScores : MonoBehaviour
{
    public DreamloData data;
    public bool dataRetrieved = false;

    public LinkedList<DreamloData.Dreamlo.Leaderboard.HighScoreRecord> ascendingScores;
    public LinkedList<DreamloData.Dreamlo.Leaderboard.HighScoreRecord> ascendingTimes;
    public BinaryTree dataTree = new BinaryTree();

    #region External Methods
    [DllImport("SortingSearchingDLL")]
    private static extern void BubbleSort(int[] arr, int n);
    [DllImport("SortingSearchingDLL")]
    private static extern void QuickSort(int[] arr, int low, int high);

    #endregion

    private void Awake()
    {
        PlayerProfile profile = ProfileManager.instance.currentProfile;
        //AddScore(profile.profileName, profile.balance.ToString(), profile.totalPlayTime.ToString());
    }

    private void Start()
    {
        //AddDummyScores();
        StartCoroutine(GetRequest("http://dreamlo.com/lb/60b5ed888f40bb64eca5f56d/json"));
        
    }

    /// <summary>
    /// Queries the Dreamlo online leaderboard for its data. This is returned in JSON format and parsed into a local data class structure.
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                // Seems to send an OK string before it sends the body text. STUPID. WASTED 4 HOURS OF MY TIME.
                if(webRequest.downloadHandler.text != "OK")
                {
                    data = JsonUtility.FromJson<DreamloData>(webRequest.downloadHandler.text);
                    SortAscendingScore();
                    SortAscendingTime();
                    dataRetrieved = true;
                }
            }
        }
    }

    /// <summary>
    /// Used for debug purposes
    /// </summary>
    private void AddDummyScores()
    {
        AddScore("Bob", "12345", "678");
        AddScore("Lisa", "54321", "876");
        AddScore("Andrew", "123", "321");
    }

    /// <summary>
    /// Sends a new score to the Dreamlo online leaderboard.
    /// </summary>
    /// <param name="name">The name of the record to send. This is the player's profile name.</param>
    /// <param name="balance">The player's balance, which is synonymous with the player's score.</param>
    /// <param name="totalTime">The player's total time spent in a game level, therefore the player's total time spent playing.</param>
    private void AddScore(string name, string balance, string totalTime)
    {
        StartCoroutine(GetRequest($"http://dreamlo.com/lb/REyGVjWkkkqwY5J0ITSyWA4CP2veOrbU-1yqU3OD6r4A/add/{name}/{balance}/{totalTime}"));
    }

    /// <summary>
    /// Uses BubbleSort to sort an array of the downloaded record scores into a LinkedList.
    /// </summary>
    private void SortAscendingScore()
    {
        int[] scores = new int[data.dreamlo.leaderboard.entry.Count];

        for (int i = 0; i < scores.Length; i++)
        {
            scores[i] = (int)data.dreamlo.leaderboard.entry[i].score;
        }

        BubbleSort(scores, scores.Length);

        string scoresOutput = "Sorted scores: ";
        foreach (int score in scores)
        {
            scoresOutput += score.ToString() + ", ";
        }
        Debug.Log(scoresOutput);
        ascendingScores = PairWithData(scores, true);

        string sortedScoreRecords = "Sorted score records: ";
        foreach(DreamloData.Dreamlo.Leaderboard.HighScoreRecord record in ascendingScores)
        {
            sortedScoreRecords += $"[Name: {record.name}, Score: {record.score}] ";
        }
        Debug.Log(sortedScoreRecords);

        StoreDataInBinaryTree(ascendingScores, dataTree);
        //dataTree.TraverseInOrder(dataTree.root);
    }

    /// <summary>
    /// Uses QuickSort to sort an array of the downloaded record times into a LinkedList.
    /// </summary>
    private void SortAscendingTime()
    {
        int[] times = new int[data.dreamlo.leaderboard.entry.Count];

        for (int i = 0; i < times.Length; i++)
        {
            times[i] = (int)data.dreamlo.leaderboard.entry[i].seconds;
        }

        BubbleSort(times, times.Length);
        //QuickSort(times, 0, times.Length - 1);

        string timesOutput = "Sorted times: ";
        foreach(int time in times)
        {
            timesOutput += time + ", "; 
        }
        Debug.Log(timesOutput);
        ascendingTimes = PairWithData(times, false);

        string sortedTimeRecords = "Sorted time records: ";
        foreach (DreamloData.Dreamlo.Leaderboard.HighScoreRecord record in ascendingTimes)
        {
            Debug.Log(record.name);
            sortedTimeRecords += $"[Name: {record.name}, Time: {record.seconds}] ";
        }

        //Debug.Log($"Sorted final time: {ascendingTimes.Last.Value.name}");
    }

    /// <summary>
    /// Sorts the high score records into a doubly-linked list by matching them back up with their score, as sorted by the C++ DLL functions. Dirty but gets the job done.
    /// </summary>
    /// <param name="input">The sorted array of either scores or times</param>
    /// <param name="isScore">If this is the score array, this should be true. If this is the times array, this should be false.</param>
    /// <returns></returns>
    LinkedList<DreamloData.Dreamlo.Leaderboard.HighScoreRecord> PairWithData(int[] input, bool isScore)
    {
        List<DreamloData.Dreamlo.Leaderboard.HighScoreRecord> workingList = new List<DreamloData.Dreamlo.Leaderboard.HighScoreRecord>(data.dreamlo.leaderboard.entry);
        DreamloData.Dreamlo.Leaderboard.HighScoreRecord[] sortedRecords = new DreamloData.Dreamlo.Leaderboard.HighScoreRecord[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            for (int w = 0; w < workingList.Count; w++)
            {
                if (isScore)
                {
                    if ((int)workingList[w].score == input[i])
                    {
                        sortedRecords[i] = workingList[w];
                        workingList.RemoveAt(w);
                        break;
                    }
                }
                else
                {
                    if ((int)workingList[w].seconds == input[i])
                    {
                        sortedRecords[i] = workingList[w];
                        workingList.RemoveAt(w);
                        break;
                    }
                }
            }
        }

        return new LinkedList<DreamloData.Dreamlo.Leaderboard.HighScoreRecord>(sortedRecords);
    }

    /// <summary>
    /// Traverses the passed LinkedList (in reverse order) and converts this into rankings. The index at which a record appears is its ranking.
    /// </summary>
    /// <param name="data">The LinkedList to be sorted into the binary tree.</param>
    /// <param name="tree">The BinaryTree for the LinkedList to be sorted into.</param>
    public void StoreDataInBinaryTree(LinkedList<DreamloData.Dreamlo.Leaderboard.HighScoreRecord> data, BinaryTree tree)
    {
        LinkedListNode<DreamloData.Dreamlo.Leaderboard.HighScoreRecord> currentNode = data.Last;

        for (int i = 0; i < data.Count; i++)
        {
            BinaryTree.BinaryTreeNode node = new BinaryTree.BinaryTreeNode();
            node.index = i;
            node.data = currentNode.Value;
            //Debug.Log($"Index: {node.index}, Name: {node.data.name}");
            tree.CreateNode(node);
            currentNode = currentNode.Previous;
        }
    }

    /// <summary>
    /// Searches the binary tree for a given rank.
    /// </summary>
    /// <param name="rank">The rank number to searched for.</param>
    public void GetRankFromBinaryTree(int rank)
    {
        BinaryTree.BinaryTreeNode result = dataTree.Find(rank - 1);
        if(result != null)
        {
            Debug.Log($"Result returned for rank #{rank}: [Name: {result.data.name}, Score: {result.data.score}, Total Time: {result.data.seconds} seconds, Record Last Updated: {result.data.date}]");
        }
    }

    public void SearchForName(string name)
    {

    }

    /// <summary>
    /// Nested classes implementation
    /// </summary>
    [Serializable]
    public class DreamloData
    {
        public Dreamlo dreamlo;

        [Serializable]
        public class Dreamlo
        {
            public Leaderboard leaderboard;

            [Serializable]
            public class Leaderboard
            {
                public List<HighScoreRecord> entry;

                [Serializable]
                public class HighScoreRecord
                {
                    public string name;
                    public float score;
                    public float seconds;
                    public string text;
                    public string date;
                }
            }
        }
    }
}
