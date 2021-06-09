using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance;

    public PlayerProfile currentProfile;
    public List<PlayerProfile> profileList;

    private string dataPath;
    private FileInfo[] files;

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

        #endregion
    }

    private void Start()
    {
        LoadProfiles();
    }

    #region Public Methods
    public bool CreateNewProfile(string name)
    {
        Debug.Log("Attempting to create a new profile with name " + name);
        // TO DO: Add validation to ensure no illegal characters entered
        if(File.Exists(dataPath + name + ".json"))
        {
            Debug.Log("Cannot create a profile with name " + name + " because it already exists.");
            return false;
        }
        else
        {
            PlayerProfile newProfile = new PlayerProfile();
            newProfile.profileName = name;
            newProfile.balance = 0;
            newProfile.reputation = 0f;
            newProfile.totalPlayTime = 0f;
            newProfile.lastSaved = DateTime.Now.ToString();

            currentProfile = newProfile;
            SaveProfile();
            LoadProfiles();
            Debug.Log("Successfully created a profile with name " + name);
            return true;
        }
    }

    public void SaveProfile()
    {
        currentProfile.currentEquipment = EquipmentManager.instance.playerEquipment;
        currentProfile.currentInventory = EquipmentManager.instance.playerInventory;
        currentProfile.lastSaved = DateTime.Now.ToString();

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
        profileList.Clear();

        foreach (FileInfo file in files)
        {
            string fileContents = File.ReadAllText(file.FullName);
            PlayerProfile profileData = JsonUtility.FromJson<PlayerProfile>(fileContents);
            profileList.Add(profileData);
        }
    }

    public bool LoadProfile(string name)
    {
        Debug.Log("LoadProfile called with name " + name);
        if(currentProfile.profileName != string.Empty)
        {
            SaveProfile();
        }

        bool found = false;
        PlayerProfile loadedProfile = null;

        if(profileList.Count != 0)
        {
            foreach (PlayerProfile profile in profileList)
            {
                if (name == profile.profileName)
                {
                    found = true;
                    loadedProfile = profile;
                    break;
                }
            }
        }
        
        if(found)
        {
            currentProfile = loadedProfile;
            EventManager.TriggerEvent("UISuccess");
            EventManager.TriggerEvent("ProfileLoaded");

            Debug.Log("Successfully loaded player profile named " + name);
            return true;
        }
        else
        {
            Debug.Log("Could not load profile, as the passed name did not match any file names.");
            return false;
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
