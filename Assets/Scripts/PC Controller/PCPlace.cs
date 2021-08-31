using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPlace : MonoBehaviour
{
    private Ray path;
    public float MaxRange = 2.5f;
    public byte[,,] Build;
    public uint[] Center;
    public RaycastHit hit;
    private Mapvoxel Chunk;
    private ApplyCollider Colliderscript;

    public void Main()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Build != null)
            {
                if (hit.transform.GetComponent<Mapvoxel>() != null)
                {
                    hit.transform.parent.gameObject.GetComponent<GenSimpleMap>().BuildBlocks(Build, hit.point + hit.normal * .01f);
                }
            }
        }
    }



    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetButtonDown("Fire1"))
    //    {
    //        if (Build != null)
    //        {
    //            path = new Ray(gameObject.transform.position, gameObject.transform.rotation * (Vector3.forward * MaxRange));

    //            if (Physics.Raycast(path.origin, path.direction, out hit, MaxRange) && hit.transform.GetComponent<Mapvoxel>() != null)
    //            {
    //                print(hit.point);
    //                hit.transform.parent.gameObject.GetComponent<GenSimpleMap>().BuildBlocks(Build, hit.point + hit.normal * .01f);
    //            }
    //        }
    //    }
    //}
}
