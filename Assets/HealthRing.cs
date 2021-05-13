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

    [SerializeField]
    private UnityAction shieldEventDelegate;

    private void RegisterListeners()
    {
        shieldEventDelegate = ShieldUpdate;
        EventManager.StartListening("ShieldsHit", shieldEventDelegate);
    }

    void ShieldUpdate()
    {
        shieldFill.fillAmount = playerScript.stats.currentShields / playerScript.stats.currentMaxShields;
        Debug.Log("Current shield Calc = " + shieldFill.fillAmount);
    }
    
    // Start is called before the first frame update
    void Start()
    {
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
        shieldFill.fillAmount = playerScript.stats.currentShields / playerScript.stats.currentMaxShields;
    }
}
