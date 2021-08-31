using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCDig : MonoBehaviour
{

    private Ray path;
    public float MaxRange = 2.5f;
    public float DigRadius = 2f;
    public RaycastHit hit;
    private Mapvoxel Chunk;
    private ApplyCollider Colliderscript;
    //private 


    public void Main()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (hit.transform.GetComponent<Mapvoxel>() != null)
            {
                hit.transform.parent.gameObject.GetComponent<GenSimpleMap>().DamageBlocks(hit.point, DigRadius, Vector3.one);
            }
        }
    }




    //void Update()
    //{
    //    if (Input.GetButtonDown("Fire2"))
    //    {
    //        //print("RightClickPressed");
    //        path = new Ray(gameObject.transform.position, gameObject.transform.rotation * (Vector3.forward * MaxRange));
            
    //        if (Physics.Raycast(path.origin, path.direction, out hit, MaxRange) && hit.transform.GetComponent<Mapvoxel>() != null)
    //        {
    //            hit.transform.parent.gameObject.GetComponent<GenSimpleMap>().DamageBlocks(hit.point, DigRadius, Vector3.one);
    //        }
    //    }
    //}

    bool ArrayContains(List<Transform> array, Transform g)
    {
        for (int i = 0; i < array.Count; i++)
        {
            if (array[i] == g) return true;
        }
        return false;
    }
}
