/*
this script controls the opening/closeing of the settings
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{

    public Animator thisAnim;
    public Toggle thisToggle;


	
	// Update is called once per frame
	public void onPress()
    {
        thisAnim.SetBool("On", thisToggle.isOn);
	}
}
