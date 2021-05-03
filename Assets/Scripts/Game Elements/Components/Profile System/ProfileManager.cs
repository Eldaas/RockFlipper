using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance;

    public PlayerProfile currentProfile = null;

    private string dataPath;
    private FileInfo[] files;
    public List<string> fileNames = new List<string>();

    [Header("Events")]
    private UnityAction loadProfiles;

    private void Awake()
    {
        #region Singleton
        ProfileManager[] list = FindObjectsOfType<ProfileManager>();
        if (list.Length > 1)
        {
            Destroy(this);
            Debug.Log("Multiple instances of the Profile Manager component detected. Destroying an instance.");
        }
        else
        {
            instance = this;
        }
        #endregion

        #region File System
        SetDirectory();
        #endregion

        #region Events
        loadProfiles = LoadProfiles;
        EventManager.StartListening("LoadProfiles", loadProfiles);
        #endregion
    }

    #region Public Methods
    public bool CreateNewProfile(string name)
    {
        // TO DO: Add validation to ensure no illegal characters entered
        if(File.Exists(dataPath + name + ".json"))
        {
            Debug.LogError("Cannot create a profile with name " + name + " because it already exists.");
            return false;
        }
        else
        {
            PlayerProfile newProfile = new PlayerProfile();
            newProfile.profileName = name;
            newProfile.balance = 0;
            newProfile.reputation = 0f;

            currentProfile = newProfile;
            SaveProfile();
            EventManager.TriggerEvent("LoadProfiles");
            EventManager.TriggerEvent("UpdateProfileSelection");
            return true;
        }
        
    }

    public void SaveProfile()
    {
        Debug.Log("SaveProfile called.");
        string saveData = JsonUtility.ToJson(currentProfile);
        string filePath = dataPath + currentProfile.profileName + ".json";
        File.WriteAllText(filePath, saveData);
    }

    public void LoadProfiles()
    {
        Debug.Log("LoadProfiles called.");
        DirectoryInfo dir = new DirectoryInfo(dataPath);
        files = dir.GetFiles();
        fileNames.Clear();

        foreach (FileInfo file in files)
        {    
            fileNames.Add(Path.GetFileNameWithoutExtension(dataPath + file.Name));
        }

        EventManager.TriggerEvent("UpdateProfileSelection");
    }

    public void LoadProfile(string name)
    {
        Debug.Log("LoadProfile called with name " + name);
        if(currentProfile.profileName != string.Empty)
        {
            Debug.Log("Current profile is not null.");
            SaveProfile();
        }

        bool found = false;

        foreach(FileInfo file in files)
        {
            if(file.Name == name + ".json")
            {
                found = true;
                break;
            }
        }

        if(found)
        {
            string fileContents = File.ReadAllText(dataPath + name + ".json");
            PlayerProfile loadedProfile = JsonUtility.FromJson<PlayerProfile>(fileContents);
            currentProfile = loadedProfile;
            EventManager.TriggerEvent("UISuccess");

            Debug.Log("Successfully loaded player profile named " + name);
        }
        else
        {
            Debug.LogError("Could not load profile, as the passed name did not match any file names.");
        }
    }

    #endregion

    #region Private Methods
    private void SetDirectory()
    {
        dataPath = Application.persistentDataPath + "/PlayerProfiles/";

        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
    }
    #endregion
}
