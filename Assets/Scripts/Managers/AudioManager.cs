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

    [Header("Audio Clips")]
    public AudioClip resourceCollect;
    public AudioClip projectileShot;
    public AudioClip projectileHit;
    public AudioClip spaceAmbience;
    public AudioClip largeAsteroidExplosion;
    public AudioClip mediumAsteroidExplosion;
    public AudioClip uiSelect;
    public AudioClip uiSuccess;
    public AudioClip returningToBase;

    [Header("Music Track Clips")]
    public AudioClip[] asteroidFieldMusicTracks;
    public AudioClip[] hangarMusicTracks;
    public AudioClip[] introMenuMusicTracks;

    [Header("Mixer Groups")] // Not all groups need to be added here; just the ones needed for runtime manipulation
    public AudioMixerGroup ambientMixer;

    [Header("Unity Actions (Event Delegates)")]
    private UnityAction resourceCollectDelegate;
    private UnityAction projectileShotDelegate;
    private UnityAction projectileHitDelegate;
    private UnityAction largeAsteroidExplosionDelegate;
    private UnityAction mediumAsteroidExplosionDelegate;
    private UnityAction spaceSceneLoadedDelegate;
    private UnityAction asteroidFieldSceneLoadedDelegate;
    private UnityAction hangarSceneLoadedDelegate;
    private UnityAction introMenuSceneLoadedDelegate;
    private UnityAction uiButtonOptionSelectDelegate;
    private UnityAction uiSuccessDelegate;
    private UnityAction returningToBaseDelegate;
    

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

        uiButtonOptionSelectDelegate = PlayUiButtonOptionSelect;
        EventManager.StartListening("UIButtonOptionSelected", uiButtonOptionSelectDelegate);

        uiSuccessDelegate = PlayUiSuccess;
        EventManager.StartListening("UISuccess", uiSuccessDelegate);

        returningToBaseDelegate = PlayReturningToBase;
        EventManager.StartListening("ReturningToBase", returningToBaseDelegate);
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
    /// Plays the UI success sound - for example, when you successfully load a profile
    /// </summary>
    private void PlayUiSuccess()
    {
        PlayOneShot(uiSounds, uiSuccess);
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

    public void PlayUiButtonOptionSelect()
    {
        PlayOneShot(uiSounds, uiSelect);
    }


    #endregion
}
