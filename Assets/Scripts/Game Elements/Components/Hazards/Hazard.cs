using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    #region Fields

    [Header("Inherited Hazard Class References")]
    /// <summary>
    /// Assign this field in the Unity Inspector to assign the 'original' game object which will be switched off and replaced by the children objects when interacted with. May be left empty if there are no interactions with child objects.
    /// </summary>
    public GameObject mainObject;

    /// <summary>
    /// Assign this field in the Unity Inspector to assign the parent object which contains any child hazard objects to which physics may be applied. May be left empty if there are no children.
    /// </summary>
    [SerializeField]
    protected GameObject childrenObjectsParent;

    /// <summary>
    /// Use childrenObjects to store anything like asteroid shards, child objects of gas pockets, etc.
    /// </summary>
    public List<Transform> childrenObjects = new List<Transform>();

    /// <summary>
    /// Use originalPositions to store the position of a childrenObject before physics is called on that object.
    /// </summary>
    protected List<Vector3> originalPositions = new List<Vector3>();

    /// <summary>
    /// Use originalPositions to store the rotation of a childrenObject before physics is called on that object.
    /// </summary>
    protected List<Quaternion> originalRotations = new List<Quaternion>();

    #endregion

    #region Methods

/*    /// <summary>
    /// Gets all of the childrenObjects from the parent game object and puts them in a list.
    /// </summary>
    /// 
    public void PopulateChildrenObjects()
    {
        childrenObjects = new List<Transform>(childrenObjectsParent.GetComponentsInChildren<Transform>(true));
        childrenObjects.Remove(childrenObjectsParent.transform);
    }

    /// <summary>
    /// Stores the original position and rotation of the input childObject so that the childObject can be reset when returned to the pool.
    /// </summary>
    /// <param name="i">The childObject being iterated over.</param>
    public void CachePositionRotation(int i)
    {
        originalPositions.Add(childrenObjects[i].transform.localPosition);
        originalRotations.Add(childrenObjects[i].transform.localRotation);
    }*/

    /// <summary>
    /// Use this to reset the hazard object (ie, return all children to their initial positions) when it exits the game view and is returned to the pool.
    /// </summary>
    public virtual void ResetHazard()
    {
        

    }

    #endregion

    #region Unity Methods

    protected virtual void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.collider.name + " has collided with " + gameObject.name + ".");
    }

    protected virtual void OnParticleCollision(GameObject collider)
    {
        //Debug.Log("A projectile has hit an asteroid.");
    }

    #endregion
}
