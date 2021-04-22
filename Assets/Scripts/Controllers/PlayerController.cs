using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Core Components")]
    private Player player;
    private Rigidbody rb;
    private Animator anim;

    [Header("Horizontal Movement")]
    public Joystick joystick;
    public float horizontalSpeed;
    public float minX;
    public float maxX;
    private float horizontalMove = 0f;

    [Header("Forwards Movement")]
    public float currentVelocity;
    public float maximumVelocity;
    public float initialThrustValue;
    public float thrustValue;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();

    }

    private void Start()
    {
        rb.AddForce(Vector3.forward * initialThrustValue);
        InvokeRepeating("RaiseMaximumVelocity", 1f, 1f);
    }

    void Update()
    {
        horizontalSpeed = player.stats.currentManeuveringSpeed;

        HandleVelocity();
        anim.SetFloat("Roll", joystick.Horizontal);

        if (joystick.Horizontal < Mathf.Epsilon || joystick.Horizontal > Mathf.Epsilon)
        {
            if(transform.position.x >= minX && transform.position.x <= maxX)
            {
                
                horizontalMove = joystick.Horizontal * horizontalSpeed * Time.deltaTime;
                transform.position += Vector3.right * horizontalMove;
            } 

            if(transform.position.x < minX)
            {
                transform.position = new Vector3(minX, transform.position.y, transform.position.z);
            }

            if (transform.position.x > maxX)
            {
                transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
            }
        }

        if(Input.GetButtonDown("Shoot"))
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
                        projectileParticle.gameObject.SetActive(true);
                        parentGo.SetActive(true);
                        IEnumerator coroutine = ObjectPooler.instance.ReturnParticleToPool(parentGo, projectileParticle.main.startLifetimeMultiplier);
                        StartCoroutine(coroutine);
                    }
                }
            }
        }
         
    }

    private void HandleVelocity()
    {
        // Speed up
        if(rb.velocity.magnitude < player.stats.currentMaximumVelocity)
        {
            rb.AddForce(Vector3.forward * player.stats.currentForwardThrust);
        }
        // Slow down
        else if(rb.velocity.magnitude > player.stats.currentMaximumVelocity)
        {
            rb.AddForce(Vector3.forward * -0.5f, ForceMode.VelocityChange);
        }
    }

    private void RaiseMaximumVelocity()
    {
        if(player.stats.currentMaximumVelocity < player.stats.velocityCap)
        {
            player.stats.currentMaximumVelocity++;
        }
        
    }

}
