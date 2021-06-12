using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

/// <summary>
/// The Audio Manager is intended to be used as a decoupled game module that operates on Unity Events.
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Fields
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource collectionPops;
    [SerializeField]
    private AudioSource shipSounds;
    [SerializeField]
    private AudioSource environmentalSounds;
    [SerializeField]
    private AudioSource uiSounds;
    [SerializeField]
    private AudioSource ambientSounds;
    [SerializeField]
    private AudioSource bgMusic;

    #region Audio Clips

    [Header("Audio Clips (General)")]
    public AudioClip resourceCollect;
    public AudioClip projectileShot;
    public AudioClip projectileHit;
    public AudioClip spaceAmbience;
    public AudioClip largeAsteroidExplosion;
    public AudioClip mediumAsteroidExplosion;
    public AudioClip returningToBase;
    public AudioClip shieldsHit;
    public AudioClip shieldsDestroyed;
    public AudioClip shieldsOnline;
    public AudioClip armourHit;
    public AudioClip armourDestroyed;
    public AudioClip hullHit;
    public AudioClip healthLow;
    public AudioClip struckLucky;
    public AudioClip powerupActivated;
    public AudioClip powerupDeactivated;
    public AudioClip resourcesDropped;

    [Header("Audio Clips (UI Sounds)")]
    public AudioClip uiClick;
    public AudioClip uiRelease;
    public AudioClip uiSelect;
    public AudioClip uiSuccess;
    public AudioClip uiError;
    public AudioClip uiNotification;
    public AudioClip uiEquip;
    public AudioClip uiUnequip;
    public AudioClip uiDestroy;
    public AudioClip uiPause;
    public AudioClip uiResume;
    public AudioClip uiItemPurchased;

    [Header("Music Track Clips")]
    public AudioClip[] asteroidFieldMusicTracks;
    public AudioClip[] hangarMusicTracks;
    public AudioClip[] introMenuMusicTracks;
    #endregion

    #region Mixer Groups

    [Header("Mixer Groups")] // Not all groups need to be added here; just the ones needed for runtime manipulation
    public AudioMixerGroup ambientMixer;

    #endregion

    #region Events

    #region Events (General)

    [Header("Events (General)")]
    private UnityAction resourceCollectDelegate;
    private UnityAction projectileShotDelegate;
    private UnityAction projectileHitDelegate;
    private UnityAction largeAsteroidExplosionDelegate;
    private UnityAction mediumAsteroidExplosionDelegate;
    private UnityAction spaceSceneLoadedDelegate;
    private UnityAction asteroidFieldSceneLoadedDelegate;
    private UnityAction hangarSceneLoadedDelegate;
    private UnityAction introMenuSceneLoadedDelegate;
    private UnityAction returningToBaseDelegate;
    private UnityAction struckLuckyDelegate;
    private UnityAction shieldsHitDelegate;
    private UnityAction shieldsDestroyedDelegate;
    private UnityAction shieldsOnlineDelegate;
    private UnityAction armourHitDelegate;
    private UnityAction armourDestroyedDelegate;
    private UnityAction hullHitDelegate;
    private UnityAction healthLowDelegate;
    private UnityAction powerupActivatedDelegate;
    private UnityAction powerupDeactivatedDelegate;
    private UnityAction resourcesDroppedDelegate;

    #endregion

    #region Events (UI)
    [Header("Events (UI)")]
    private UnityAction uiClickDelegate;
    private UnityAction uiReleaseDelegate;
    private UnityAction uiSelectDelegate;
    private UnityAction uiSuccessDelegate;
    private UnityAction uiErrorDelegate;
    private UnityAction uiNotificationDelegate;
    private UnityAction uiEquipDelegate;
    private UnityAction uiUnequipDelegate;
    private UnityAction uiDestroyDelegate;
    private UnityAction uiPauseDelegate;
    private UnityAction uiResumeDelegate;
    private UnityAction uiItemPurchasedDelegate;
    #endregion

    #endregion

    #endregion

    #region Unity Methods
    private void Awake()
    {
        #region Singleton
        AudioManager[] list = FindObjectsOfType<AudioManager>();
        if (list.Length > 1)
        {
            Destroy(this);
            Debug.Log("Multiple instances of the Audio Manager component detected. Destroying an instance.");
        }
        else
        {
            instance = this;
        }
        #endregion

        RegisterEventListeners();
    }

    private void Start()
    {
        
    }

    #endregion

    #region Private Methods

    #region Main Methods

    private void PlayOneShot(AudioSource mixerGroup, AudioClip clip)
    {
        mixerGroup.PlayOneShot(clip);
    }

    /// <summary>
    /// Registers the event listeners required by the Audio Manager and assigns the relevant method to the Unity Action referenced.
    /// Ensure you assign the UnityAction to its relevant method BEFORE you start listening for the event, otherwise you'll get exception messages in the console.
    /// </summary>
    private void RegisterEventListeners()
    {
        #region General Events
        resourceCollectDelegate = PlayResourceCollect;
        EventManager.StartListening("ResourceCollected", resourceCollectDelegate);

        projectileShotDelegate = PlayProjectileShot;
        EventManager.StartListening("ProjectileShot", projectileShotDelegate);

        projectileHitDelegate = PlayProjectileHit;
        EventManager.StartListening("ProjectileHit", projectileHitDelegate);

        largeAsteroidExplosionDelegate = PlayLargeAsteroidExplosion;
        EventManager.StartListening("LargeAsteroidExplosion", largeAsteroidExplosionDelegate);

        mediumAsteroidExplosionDelegate = PlayMediumAsteroidExplosion;
        EventManager.StartListening("MediumAsteroidExplosion", mediumAsteroidExplosionDelegate);

        spaceSceneLoadedDelegate = PlaySpaceAmbience;
        EventManager.StartListening("SpaceSceneLoaded", spaceSceneLoadedDelegate);

        asteroidFieldSceneLoadedDelegate = PlayAsteroidFieldMusic;
        EventManager.StartListening("AsteroidFieldSceneLoaded", asteroidFieldSceneLoadedDelegate);

        introMenuSceneLoadedDelegate = PlayIntroMenuMusic;
        EventManager.StartListening("IntroSceneLoaded", introMenuSceneLoadedDelegate);

        hangarSceneLoadedDelegate = PlayHangarSceneMusic;
        EventManager.StartListening("HangarSceneLoaded", hangarSceneLoadedDelegate);

        returningToBaseDelegate = PlayReturningToBase;
        EventManager.StartListening("ReturningToBase", returningToBaseDelegate);
        #endregion

        #region UI Events

        uiClickDelegate = delegate { PlayOneShot(uiSounds, uiClick); };
        EventManager.StartListening("UIClick", uiClickDelegate);

        uiReleaseDelegate = delegate { PlayOneShot(uiSounds, uiRelease); };
        EventManager.StartListening("UIRelease", uiReleaseDelegate);

        uiSelectDelegate = delegate { PlayOneShot(uiSounds, uiSelect); };
        EventManager.StartListening("UISelect", uiSelectDelegate);

        uiSuccessDelegate = delegate { PlayOneShot(uiSounds, uiSuccess); };
        EventManager.StartListening("UISuccess", uiSuccessDelegate);

        uiErrorDelegate = delegate { PlayOneShot(uiSounds, uiError); };
        EventManager.StartListening("UIError", uiErrorDelegate);

        uiNotificationDelegate = delegate { PlayOneShot(uiSounds, uiNotification); };
        EventManager.StartListening("UINotification", uiNotificationDelegate);

        uiEquipDelegate = delegate { PlayOneShot(uiSounds, uiEquip); };
        EventManager.StartListening("ItemEquipped", uiEquipDelegate);

        uiUnequipDelegate = delegate { PlayOneShot(uiSounds, uiUnequip); };
        EventManager.StartListening("ItemUnequipped", uiUnequipDelegate);

        uiDestroyDelegate = delegate { PlayOneShot(uiSounds, uiDestroy); };
        EventManager.StartListening("ItemDestroyed", uiDestroyDelegate);

        uiPauseDelegate = delegate { PlayOneShot(uiSounds, uiPause); };
        EventManager.StartListening("UIPause", uiPauseDelegate);

        uiResumeDelegate = delegate { PlayOneShot(uiSounds, uiResume); };
        EventManager.StartListening("UIResume", uiResumeDelegate);

        uiItemPurchasedDelegate = delegate { PlayOneShot(uiSounds, uiItemPurchased); };
        EventManager.StartListening("ItemPurchased", uiItemPurchasedDelegate);

        struckLuckyDelegate = delegate { PlayOneShot(uiSounds, struckLucky); };
        EventManager.StartListening("StruckLucky", struckLuckyDelegate);

        shieldsHitDelegate = delegate { PlayOneShot(shipSounds, shieldsHit); };
        EventManager.StartListening("ShieldsHit", shieldsHitDelegate);

        shieldsDestroyedDelegate = delegate { PlayOneShot(shipSounds, shieldsDestroyed); };
        EventManager.StartListening("ShieldsDestroyed", shieldsDestroyedDelegate);

        armourHitDelegate = delegate { PlayOneShot(shipSounds, armourHit); };
        EventManager.StartListening("ArmourHit", armourHitDelegate);

        armourDestroyedDelegate = delegate { PlayOneShot(shipSounds, armourDestroyed); };
        EventManager.StartListening("ArmourDestroyed", armourDestroyedDelegate);

        hullHitDelegate = delegate { PlayOneShot(shipSounds, hullHit); };
        EventManager.StartListening("HullHit", hullHitDelegate);

        healthLowDelegate = delegate { PlayOneShot(shipSounds, healthLow); };
        EventManager.StartListening("HealthLow", healthLowDelegate);

        resourcesDroppedDelegate = delegate { PlayOneShot(uiSounds, resourcesDropped); };
        EventManager.StartListening("ResourcesDropped", resourcesDroppedDelegate);

        #endregion
    }

    private void PlayMusicTrack(AudioClip track)
    {
        bgMusic.Stop();
        bgMusic.clip = track;
        bgMusic.Play();
    }

    private AudioClip SelectRandomClip(AudioClip[] clip)
    {
        int randomIndex = Utility.GenerateRandomInt(0, clip.Length - 1);
        return clip[randomIndex];
    }
    #endregion

    #region Play Sound Methods
    // Implement here the various PRIVATE methods which cause a particular sound to play.
    // Remember to register the appropriate event listener in the RegisterEventListeners method, create a UnityAction and assign it a method to call.

    /// <summary>
    /// Plays the resource collection sound.
    /// </summary>
    private void PlayResourceCollect()
    {
        if(!collectionPops.isPlaying)
        {
            PlayOneShot(collectionPops, resourceCollect);
        }
        
    }

    /// <summary>
    ///  Plays the projectile shot sound.
    /// </summary>
    private void PlayProjectileShot()
    {
        PlayOneShot(shipSounds, projectileShot);
    }

    /// <summary>
    /// Plays the projectile hit sound.
    /// </summary>
    private void PlayProjectileHit()
    {
        PlayOneShot(shipSounds, projectileHit);
    }

    /// <summary>
    /// Plays the large asteroid explosion sound.
    /// </summary>
    private void PlayLargeAsteroidExplosion()
    {
        PlayOneShot(environmentalSounds, largeAsteroidExplosion);
    }

    /// <summary>
    /// Plays the medium asteroid explosion sound.
    /// </summary>
    private void PlayMediumAsteroidExplosion()
    {
        PlayOneShot(environmentalSounds, mediumAsteroidExplosion);
    }

    /// <summary>
    /// Plays the looping space ambience track.
    /// </summary>
    private void PlaySpaceAmbience()
    {
        ambientSounds.clip = spaceAmbience;
        ambientSounds.Play();
    }

    /// <summary>
    /// Selects a random track from the inspector-assigned list and plays as background music for the asteroid field scene.
    /// </summary>
    private void PlayAsteroidFieldMusic()
    {
        // Currently the asteroid scene
        AudioClip clip = SelectRandomClip(asteroidFieldMusicTracks);
        PlayMusicTrack(clip);
    }

    /// <summary>
    /// Selects a random track from the inspector-assigned list and plays as background music for the intro menu scene.
    /// </summary>
    private void PlayIntroMenuMusic()
    {
        AudioClip clip = SelectRandomClip(introMenuMusicTracks);
        PlayMusicTrack(clip);
    }

    /// <summary>
    /// Selects a random track from the inspector-assigned list and plays as background music for the hangar scene.
    /// </summary>
    private void PlayHangarSceneMusic()
    {
        AudioClip clip = SelectRandomClip(hangarMusicTracks);
        PlayMusicTrack(clip);
    }

    /// <summary>
    /// Plays an audio clip when the 'returning to base' event is triggered.
    /// </summary>
    private void PlayReturningToBase()
    {
        PlayOneShot(shipSounds, returningToBase);
    }
    #endregion

    #endregion

    #region Public Methods


    #endregion
}

