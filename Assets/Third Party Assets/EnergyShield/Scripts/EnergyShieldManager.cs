/*
EnergyShieldManager.cs
Version 1.1

1. Creates/Updates the Energy Shield Effects
2. Stores presets for the Energy Shield Effects

*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnergyShieldManager : MonoBehaviour 
{
    
	/// <summary>
	/// List of tags that will cause the energy sheild to display effect
	/// </summary>
	public List<string> CollisionTags = new List<string>();
    

	/// <summary>
	/// The possibile values you would like use while rendering the Energy Shield Effect
	/// </summary>
	public preset[] presets = new preset[1];

	/// <summary>
	/// The index of the preset.
	/// </summary>
	public int presetIndex = 0;


	/// <summary>
	/// The material
	/// </summary>
	public Material mat;

	void Awake()
	{
		//display this stupid error because stupid people exist
		if (presets.Length == 0)
		{
			Debug.LogError("You need at least one preset");
		}
        
        //clear all Energy Shield Effects
        for (int i = 0;i < ese.Length;i++)
		{
			ClearESE(i);
		}
        
        if (!mat.shader.name.Contains("Custom/EnergyShield"))
        {
            mat.shader = Shader.Find("Custom/EnergyShield");

            print("Lets Assign the right shader");
        }
        

		//speed check
		for(int i = 0;i < presets.Length;i++)
		{
			if (presets[i].speed == 0f)
			{
				Debug.LogWarning("i don't think you want the speed to be zero, try 0.02.");
			}
		}
	}

	//On FixedUpdate
	void FixedUpdate()
	{
		//for each active Energy Shield Effect...update the values of the effect based on it's assigned preset.
		for(int i = 0;i < ese.Length;i++)
		{
			if (ese[i].active)
			{
				//localy make variables so we don't need to reference the source all the time.
				float t = 0f;
				int p = ese[i].preset;

				//increase time
				ese[i].time += presets[p].speed;
				t = ese[i].time;

				//set the new values for the Energy Screen Effect
				ese[i].radius = presets[p].radiusOverTime.Evaluate(t);
				ese[i].fadePower = presets[p].fadePowerOverTime.Evaluate(t);
				ese[i].inThickness = presets[p].inThicknessOverTime.Evaluate(t);
				ese[i].outThickness = presets[p].outThicknessOverTime.Evaluate(t);
				ese[i].color = presets[p].colorOverTime.Evaluate(t);

				//pass the values to the shader
				mat.SetVector("_CollisionPoint" + i.ToString("00"),ese[i].collisionPoint + transform.position);
				mat.SetFloat("_Radius" + i.ToString("00"),ese[i].radius);
				mat.SetFloat("_FadePower" + i.ToString("00"),ese[i].fadePower);
				mat.SetFloat("_inThickness" + i.ToString("00"),ese[i].inThickness);
				mat.SetFloat("_outThickness" + i.ToString("00"),ese[i].outThickness);
				mat.SetColor("_Color" + i.ToString("00"),ese[i].color);

				//clear and deactivate effect if done.
				if (t >= 1f)
				{
					ClearESE(i);
				}
			}
		}
	}

	/// <summary>
	/// Use this Method when a projectile/Ray hits the Energy Shield
	/// </summary>
	/// <param name="collisionPoint">Collision point.</param>
	public void Hit(Vector3 collisionPoint)
	{
		//find an empty slot to render this Energy Shield Effect
		int h = -1;
		for(int i =0; i < ese.Length;i++)
		{
			if (!ese[i].active)
			{
				h = i;
				break;
			}
		}

		//no slot is found
		if (h == -1)
		{
			Debug.LogWarning("this asset can only handle 10 collisionPoints at a time");
			return;
		}

		//set the new Energy Shield Effect
		ese[h].collisionPoint = collisionPoint - transform.position;
		ese[h].preset = presetIndex;
		ese[h].active = true;

        //used for debugging
        /*
        print(ese[h].collisionPoint);
        print(ese[h].preset);
        print(ese[h].active);
        */

        print("Please Rate and Comment EnergyShield!");
    }

	/// <summary>
	/// Use this Method when a projectile/Ray hits the Energy Shield
	/// </summary>
	/// <param name="collisionPoint">Collision point.</param>
	/// <param name="presets">Presets.</param>
	public void Hit(Vector3 collisionPoint, int preset)
	{
		//find an empty slot to render this Energy Shield Effect
		int h = -1;
		for(int i =0; i < ese.Length;i++)
		{
			if (!ese[i].active)
			{
				h = i;
				break;
			}
		}

		//no slot is found
		if (h == -1)
		{
			Debug.LogWarning("this asset can only handle 10 collisionPoints at a time");
			return;
		}

		//if preset doesn't exist
		if (preset >= presets.Length)
		{
			Debug.LogWarning("this preset dosen't exists...we'll use zero");
			preset = 0;
		}

		//set the new Energy Shield Effect
		ese[h].collisionPoint = collisionPoint - transform.position;
		ese[h].preset = preset;
		ese[h].active = true;

	}

	//Struct for the preset
	[System.Serializable]
	public struct preset
	{
		public AnimationCurve radiusOverTime;
		public AnimationCurve fadePowerOverTime;
		public AnimationCurve inThicknessOverTime;
		public AnimationCurve outThicknessOverTime;
		public Gradient colorOverTime;
		public float speed;
	}

	//A List of all the Energy Shield Effects.
	[HideInInspector]
	public ESE[] ese = new ESE[10];

	//Energy Shield Effect
	[System.Serializable]
	public struct ESE
	{
		public Vector3 collisionPoint;
		public float radius;
		public float fadePower;
		public float inThickness;
		public float outThickness;
		public Color color;
		public float time;
		public int preset;
		public bool  active;
	}



    //used to clear an Energy Shield Effect so it can be reused.
    private void ClearESE(int index)
	{
		ese[index].collisionPoint =  Vector4.zero;
		ese[index].radius = 0f;
		ese[index].fadePower = 0f;
		ese[index].inThickness = 0f;
		ese[index].outThickness = 0f;
		ese[index].color = new Color(0f,0f,0f,0f);
		ese[index].time = 0f;
		ese[index].active = false;

		mat.SetVector("_CollisionPoint" + index.ToString("00"),ese[index].collisionPoint);
		mat.SetFloat("_Radius" + index.ToString("00"),ese[index].radius);
		mat.SetFloat("_FadePower" + index.ToString("00"),ese[index].fadePower);
		mat.SetFloat("_inThickness" + index.ToString("00"),ese[index].inThickness);
		mat.SetFloat("_outThickness" + index.ToString("00"),ese[index].outThickness);
		mat.SetColor("_Color" + index.ToString("00"),ese[index].color);

	}


    //for Colliion Events

	//on Collision Enter
	void OnCollisionEnter(Collision collision) 
	{

		if (CollisionTags.Contains(collision.gameObject.tag))
		{
			foreach (ContactPoint contact in collision.contacts) 
	        {
	            Hit(contact.point);
	        }
		}
    }

    //on Trigger Enter...if you decide to make this a trigger
	void OnTriggerEnter(Collider other) 
	{
		if (CollisionTags.Contains(other.tag))
        {
        	//note this position might not be very accurate so i suggest using the Collision function.
        	Hit(other.transform.position);
        }
    }




    /// <summary>
    /// this part is used for the demo...it can be commented out
    /// </summary>
    private int _presetindex;
    public void nextPreset()
    {
        _presetindex++;
        presetIndex = Mathf.Abs(_presetindex) % presets.Length;
    }

    public void prevPreset()
    {
        _presetindex--;
        presetIndex = Mathf.Abs(_presetindex) % presets.Length;
    }
}


