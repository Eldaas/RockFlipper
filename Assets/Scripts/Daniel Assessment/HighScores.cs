using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class HighScores : MonoBehaviour
{
    [SerializeField]
    public DreamloData data;

    private void Awake()
    {
        PlayerProfile profile = ProfileManager.instance.currentProfile;
        AddScore(profile.profileName, profile.balance.ToString(), profile.totalPlayTime.ToString());
    }

    private void Start()
    {
        AddDummyScores();
        StartCoroutine(GetRequest("http://dreamlo.com/lb/60b5ed888f40bb64eca5f56d/json"));
    }

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
                if(webRequest.downloadHandler.text != "OK")
                {
                    data = JsonUtility.FromJson<DreamloData>(webRequest.downloadHandler.text);
                }
            }
        }
    }

    private void AddDummyScores()
    {
        AddScore("Bob", "12345", "678");
        AddScore("Lisa", "54321", "876");
        AddScore("Andrew", "123", "321");
    }

    private void AddScore(string name, string balance, string totalTime)
    {
        StartCoroutine(GetRequest($"http://dreamlo.com/lb/REyGVjWkkkqwY5J0ITSyWA4CP2veOrbU-1yqU3OD6r4A/add/{name}/{balance}/{totalTime}"));
    }


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
                public List<Entry> entry;

                [Serializable]
                public class Entry
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

    

    

    





    // Upload Dreamlo data

    // Load Dreamlo data

    // Split data into list

    // Sort data ascending score into first doubly-linked list

    // Sort data ascending play time into second doubly-linked list

    // Split list into two doubly-linked lists (score, total play time)

}
