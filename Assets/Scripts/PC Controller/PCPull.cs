using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPull : MonoBehaviour
{
    private bool FirstFrame = true;
    private Ray path;

    private GameObject HitTransform;
    private float HitDistance;
    private Rigidbody rb;
    private Vector3 force;
    public RaycastHit hit;

    public float MaxRange = 2f;
    public float Strength = 5f;
    public float Damping = 2f;


    public void Main()
    {
        if (Input.GetButtonDown("Fire1") && HitTransform != null)
        {
            FirstFrame = false;
            rb.AddForce(gameObject.transform.rotation * Vector3.forward * Strength, ForceMode.VelocityChange);
            Destroy(HitTransform);
        }

        if (Input.GetButton("PullObjects"))
        {
            if (FirstFrame)
            {
                if (hit.transform.GetComponent<Rigidbody>() != null)
                {
                    HitTransform = new GameObject("Force Location");
                    HitTransform.transform.position = hit.point;
                    HitTransform.transform.SetParent(hit.transform);
                    HitDistance = hit.distance;

                    rb = hit.transform.GetComponent<Rigidbody>();

                    FirstFrame = false;
                }
            }

            else if (HitTransform != null)
            {
                force = (gameObject.transform.position + gameObject.transform.rotation * (Vector3.forward * HitDistance) - HitTransform.transform.position) * Strength;
                rb.AddForce(-rb.velocity / Damping, ForceMode.VelocityChange);
                rb.AddForceAtPosition(force, HitTransform.transform.position);
            }
        }

        else
        {
            FirstFrame = true;

            if (HitTransform != null)
            {
                Destroy(HitTransform);
            }
        }
    }



    //// Update is called once per frame
    //void Update()
    //{

    //    //Debug.DrawLine(gameObject.transform.position, gameObject.transform.position + gameObject.transform.rotation * (Vector3.forward * MaxRange), Color.white, 0.5f, true);
        
    //    if (Input.GetButtonDown("Fire1") && HitTransform != null)
    //    {
    //        FirstFrame = false;
    //        rb.AddForce(gameObject.transform.rotation * Vector3.forward * Strength, ForceMode.VelocityChange);
    //        Destroy(HitTransform);
    //    }

    //    if (Input.GetButton("PullObjects"))
    //    {
    //        if (FirstFrame)
    //        {
    //            path = new Ray(gameObject.transform.position, gameObject.transform.rotation * Vector3.forward);

    //            if (Physics.Raycast(path.origin, path.direction, out hit, MaxRange) && hit.transform.GetComponent<Rigidbody>() != null)
    //            {
    //                HitTransform = new GameObject("Force Location");
    //                HitTransform.transform.position = hit.point;
    //                HitTransform.transform.SetParent(hit.transform);
    //                HitDistance = hit.distance;

    //                FirstFrame = false;
    //            }
    //        }
            
    //        else if (HitTransform != null)
    //        {
    //            rb = hit.transform.GetComponent<Rigidbody>();
    //            force = (gameObject.transform.position + gameObject.transform.rotation * (Vector3.forward * HitDistance) - HitTransform.transform.position) * Strength;
    //            rb.AddForce(-rb.velocity / Damping, ForceMode.VelocityChange);
    //            rb.AddForceAtPosition(force, HitTransform.transform.position);
    //        }
    //    }

    //    else
    //    {
    //        FirstFrame = true;
            
    //        if (HitTransform != null)
    //        {
    //            Destroy(HitTransform);
    //        }
    //    }
    //}
}
