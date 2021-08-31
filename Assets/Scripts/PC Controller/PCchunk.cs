using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCchunk : MonoBehaviour
{
    public GameObject chunk;
    public float width;
    public int Viewdist;

    private float xoffset;
    private float zoffset;

    private Vector3 Position;
    private Vector3 Newpos;
    private GameObject plane;
    private Vector3 temppos;
    private float test;

    void Start()
    {
        Position = gameObject.transform.position;
        xoffset = Position.x % width;
        zoffset = Position.z % width;

        test = Position.x;

        for (float i = (Position.x - (xoffset + Viewdist)); i <= Position.x - (xoffset - (Viewdist+(1*Viewdist))); i++)
        {
            for (float j = (Position.z - (zoffset + Viewdist)); j <= Position.z - (zoffset - (Viewdist + (1 * Viewdist))); j++)
            {
                plane = Instantiate(chunk, null);
                temppos = new Vector3(i, 0, j)*width;
                plane.transform.position = temppos;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
