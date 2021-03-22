using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offsetFromPlayer;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        transform.position = new Vector3(offsetFromPlayer.x, offsetFromPlayer.y, offsetFromPlayer.z + player.transform.position.z);
    }
}
