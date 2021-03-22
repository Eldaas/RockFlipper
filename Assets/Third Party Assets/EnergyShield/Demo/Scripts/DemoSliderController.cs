using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoSliderController : MonoBehaviour
{


    public Slider cutOffSlider;

    public Gradient colors;

    public Slider mainColorSlider;
    public Slider mainAlphaSlider;
    public Image mainColorFill;
    public Image mainColorHandle;
    
    public Slider fresnelSlider;
    
    public Slider fresnelWithSlider;
    
    public Slider distortSlider;
    
    public Slider normalIncrease;
    
    public Material energyShieldMat;

    public GameObject kyleRobot;
    public GameObject zomBunny;
    public GameObject sphereCube;
    public GameObject zomBear;
    public GameObject emptyShield;

    public Material material;

    public Texture[] Textures;
    public int iTexture = 0;
    
    // Update is called once per frame
    void Update()
    {
        Color newColor = colors.Evaluate(mainColorSlider.value);
        
        mainColorFill.color = newColor;
        mainColorHandle.color = newColor;

        newColor.a = mainAlphaSlider.value;


        energyShieldMat.SetFloat("_CutOff", cutOffSlider.value);
        energyShieldMat.SetColor("_MainColor", newColor);
        energyShieldMat.SetFloat("_Fresnel", fresnelSlider.value);
        energyShieldMat.SetFloat("_FresnelWidth", fresnelWithSlider.value);
        energyShieldMat.SetFloat("_Distort", distortSlider.value);
        energyShieldMat.SetFloat("_NormalIncrease", normalIncrease.value);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            kyleRobot.SetActive(true);
            zomBunny.SetActive(false);
            sphereCube.SetActive(false);
            zomBear.SetActive(false);
            emptyShield.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            kyleRobot.SetActive(false);
            zomBunny.SetActive(true);
            sphereCube.SetActive(false);
            zomBear.SetActive(false);
            emptyShield.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            kyleRobot.SetActive(false);
            zomBunny.SetActive(false);
            sphereCube.SetActive(true);
            zomBear.SetActive(false);
            emptyShield.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            kyleRobot.SetActive(false);
            zomBunny.SetActive(false);
            sphereCube.SetActive(false);
            zomBear.SetActive(true);
            emptyShield.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            kyleRobot.SetActive(false);
            zomBunny.SetActive(false);
            sphereCube.SetActive(false);
            zomBear.SetActive(false);
            emptyShield.SetActive(true);
        }
    }

    public void nextPreset()
    {
        EnergyShieldManager[] ESM = GameObject.FindObjectsOfType<EnergyShieldManager>();

        for (int i = 0; i < ESM.Length; i++)
        {
            ESM[i].nextPreset();
        }
    }

    public void prevPreset()
    {
        EnergyShieldManager[] ESM = GameObject.FindObjectsOfType<EnergyShieldManager>();

        for (int i = 0; i < ESM.Length; i++)
        {
            ESM[i].prevPreset();
        }
    }

    public void nextTexture()
    {
        iTexture++;
        material.SetTexture("_MainTex", Textures[iTexture % Textures.Length]);
    }

    public void prevTexture()
    {
        iTexture--;
        material.SetTexture("_MainTex", Textures[iTexture % Textures.Length]);
    }
}
