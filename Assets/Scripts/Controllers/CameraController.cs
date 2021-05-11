using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public Vector3 offsetFromPlayer;
    public bool following = true;
    private GameObject player;

    private UnityAction stopCameraDelegate;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(following)
        {
            transform.position = new Vector3(offsetFromPlayer.x, offsetFromPlayer.y, offsetFromPlayer.z + player.transform.position.z);
        }
    }

    #region Private Methods
    private void RegisterListeners()
    {
        stopCameraDelegate = StopCameraFollowing;
        EventManager.StartListening("ReturningToBase", stopCameraDelegate);
    }

    private void StopCameraFollowing()
    {
        following = false;
    }
    #endregion
}
