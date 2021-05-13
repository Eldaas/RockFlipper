using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HealthRing : MonoBehaviour
{
    private GameObject player;
    private Vector3 tarPos = new Vector3(0, -5, 0);

    private Player playerScript;

    public Image shieldFill;
    public Image velocityFill;
    public Image batteryFill;
    public Image hullFill;
    public Image armorFill;

    [SerializeField]
    private UnityAction shieldEventDelegate;

    private void SetupFill()
    {
        shieldFill.fillAmount = 1;
        velocityFill.fillAmount = 0;
        batteryFill.fillAmount = 1;
        hullFill.fillAmount = 1;
        armorFill.fillAmount = 1;
    }

    private void RegisterListeners()
    {
        //shieldEventDelegate = ShieldUpdate;
        //EventManager.StartListening("ShieldsHit", shieldEventDelegate);
    }

    void ShieldUpdate()
    {
        shieldFill.fillAmount = playerScript.stats.currentShields / playerScript.stats.currentMaxShields;
        Debug.Log("Current shield Calc = " + shieldFill.fillAmount);
    }

    void UpdateFills()
    {
        shieldFill.fillAmount = playerScript.stats.currentShields / playerScript.stats.currentMaxShields;
        velocityFill.fillAmount = playerScript.Velocity / playerScript.stats.currentMaximumVelocity;
        batteryFill.fillAmount = playerScript.stats.currentHeatSinkLevel / playerScript.stats.currentHeatSinkCapacity;
        hullFill.fillAmount = playerScript.stats.currentHull / playerScript.stats.currentMaxHull;
        armorFill.fillAmount = playerScript.stats.currentArmour / playerScript.stats.currentMaxArmour;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupFill();
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        RegisterListeners();
    }


    // Update is called once per frame
    void Update()
    {
        tarPos.x = player.transform.position.x;
        tarPos.z = player.transform.position.z;
        transform.position = tarPos;
        UpdateFills();
    }
}
