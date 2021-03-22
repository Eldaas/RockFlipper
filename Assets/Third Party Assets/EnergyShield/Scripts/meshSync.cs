using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshSync : MonoBehaviour 
{

	public MeshFilter splineMesh;
	

	private MeshFilter thisMeshFilter;
	public MeshRenderer thisMeshRenderer;


	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Start()
	{
        print(splineMesh.sharedMesh);
        print(splineMesh.mesh);

        thisMeshFilter =  GetComponent<MeshFilter>();
        thisMeshFilter.mesh = (Mesh) Instantiate( splineMesh.sharedMesh );
	}



	
	// Update is called once per frame
	void Update () {

        thisMeshFilter.mesh = (Mesh) Instantiate( splineMesh.sharedMesh );
		
	}
}



