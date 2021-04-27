using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance;

    public PlayerProfile currentProfile = null;

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
    }

    #region Public Methods

    public virtual void CreateNewProfile(string name)
    {
        PlayerProfile newProfile = new PlayerProfile();
        newProfile.profileName = name;
        newProfile.balance = 0;
        newProfile.reputation = 0f;

        currentProfile = newProfile;
    }

    #endregion

    #region Private Methods

    #endregion
}
