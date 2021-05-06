﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerController : MonoBehaviour
{
    [Header("Core Components")]
    private Player player;
    private Rigidbody rb;
    private Animator anim;

    [Header("Horizontal Movement")]
    private float input;
    public Joystick joystick;
    public float horizontalSpeed;
    public float minX;
    public float maxX;
    public float horizontalMove = 0f;
    public float horizontalDrag = 0f;
    public float maxHorizontalVelocity;
    public float forceMultiplier;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();

    }

    private void Start()
    {
        InvokeRepeating("RaiseMaximumVelocity", 1f, 1f);
    }

    void Update()
    {
        horizontalSpeed = player.stats.currentManeuveringSpeed;

        HandleVelocity();


#if UNITY_ANDROID

        input = joystick.Horizontal;

#endif

#if UNITY_STANDALONE_WIN

        input = Input.GetAxis("Horizontal");

#endif
        HandleHorizontal(input);
        anim.SetFloat("Roll", input);

        if (Input.GetButtonDown("Shoot"))
        {
            GameObject parentGo = ObjectPooler.instance.GetPooledProjectile();
            if(parentGo)
            {
                // Get parent particle system and set the location
                Transform parentObject = parentGo.GetComponentInParent<Transform>();
                parentGo.transform.position = parentObject.position;

                // Get the children particle systems
                ParticleSystem[] children = parentGo.GetComponentsInChildren<ParticleSystem>(true);

                // Find the particle system responsible for the projectile
                for (int i = 0; i < children.Length; i++)
                {
                    if (children[i].CompareTag("Projectile"))
                    {
                        ParticleSystem projectileParticle = children[i];

                        /* FOR DEBUG PURPOSES
                        int randomInt = Utility.GenerateRandomInt(0, 1000);
                        string projectileName = "Projectile " + randomInt.ToString();
                        projectileParticle.gameObject.name = projectileName;
                        Debug.Log("Projectile particle " + projectileParticle.gameObject.name + " is currently active?: " + projectileParticle.gameObject.activeSelf);
                        */

                        parentGo.SetActive(true);
                        ParticleSystem.MainModule main = projectileParticle.main;
                        main.startSpeed = 300 + rb.velocity.z;

                        IEnumerator coroutine = ObjectPooler.instance.ReturnParticleToPool(parentGo, projectileParticle.main.startLifetimeMultiplier);
                        StartCoroutine(coroutine);
                        EventManager.TriggerEvent("ProjectileShot");
                        break;
                    }
                }
            }
        }
    }

    private void HandleVelocity()
    {
        // Speed up
        if(rb.velocity.z < player.stats.currentMaximumVelocity)
        {
            rb.AddForce(Vector3.forward * player.stats.currentForwardThrust);
        }
        // Slow down
        else if(rb.velocity.z > player.stats.currentMaximumVelocity)
        {
            rb.AddForce(Vector3.forward * -0.5f, ForceMode.VelocityChange);
        }
    }

    private void HandleHorizontal(float axis)
    {
        horizontalMove = axis * forceMultiplier;
        Vector3 direction = Vector3.zero;

        if (transform.position.x >= minX && transform.position.x <= maxX)
        {
            direction = horizontalMove * Vector3.right;

            if (horizontalMove < 0f && rb.velocity.x > -maxHorizontalVelocity)
            {
                rb.AddForce(direction, ForceMode.Impulse);
            }
            
            if (horizontalMove > 0f && rb.velocity.x < maxHorizontalVelocity)
            {
                rb.AddForce(direction, ForceMode.Impulse);
            }
        }

        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
            rb.velocity = new Vector3(0f, 0f, rb.velocity.z);
        }
        else if(transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
            rb.velocity = new Vector3(0f, 0f, rb.velocity.z);
        }

        // Calculate and apply horizontal drag
        float dragLerp = Mathf.Abs(rb.velocity.x) / maxHorizontalVelocity;

        if (rb.velocity.x < 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x + (horizontalDrag * dragLerp) * Time.deltaTime, 0f, rb.velocity.z);
        }
        else if (rb.velocity.x > 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x - (horizontalDrag * dragLerp) * Time.deltaTime, 0f, rb.velocity.z);
        }

    }

    private void RaiseMaximumVelocity()
    {
        if(player.stats.currentMaximumVelocity < player.stats.velocityCap)
        {
            player.stats.currentMaximumVelocity++;
        }
    }

    public void AddExternalForce(Vector3 direction, float forceValue)
    {
        rb.AddForce(direction * forceValue);
    }

    public void AddExternalForce(Vector3 direction, float forceValue, ForceMode forceMode)
    {
        rb.AddForce(direction * forceValue, forceMode);
    }

}
