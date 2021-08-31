using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCmove : MonoBehaviour
{
    public Rigidbody Character;
    private Vector3 force = Vector3.zero;
    public float gravity = -9.81f;
    public float acceleration = 100f;
    public float friction = 10f;

    public float jumpspeed = 10f;

    private bool isGrounded;

    public float groundDistance = 0.4f;
    
    private Vector3 frictionforce;

    private bool CollisionStayCalled = false;
    public float ContactBuffer = .3f;
    private float PreviousGroundedTime = 0;
    
    void Start()
    {
        Character.drag = 0;
    }

    void OnCollisionStay(Collision collision)
    {
        bool notgrounded = true;
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.point.y - gameObject.transform.position.y < .69f)
            {
                isGrounded = true;
                notgrounded = false;
            }
        }
        if (notgrounded)
        {
            isGrounded = false;
        }
        CollisionStayCalled = true;
    }

    //Update is called once per frame
    void Update()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Manage Contact buffer to allow grounded movement while not grounded
        if (!(isGrounded && CollisionStayCalled))
        {
            PreviousGroundedTime += Time.deltaTime;
        }
        else
        {
            PreviousGroundedTime = 0f;
        }

        //Manage movement while grounded
        if ((isGrounded && CollisionStayCalled) || PreviousGroundedTime < ContactBuffer)
        {

            //Get inputs
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            //Normalize inputs
            if (x != 0 || z != 0)
            {
                x = x / (Mathf.Sqrt(x * x + z * z));
                z = z / (Mathf.Sqrt(x * x + z * z));
                x *= acceleration;
                z *= acceleration;
            }

            //Consolidate X variable and Z variable to force
            force.x = x;
            force.z = z;
            force.y = 0f;

            //jump
            if (Input.GetButtonDown("Jump"))
            {
                Character.AddForce(0, jumpspeed, 0, ForceMode.VelocityChange);
            }
            Character.AddRelativeForce(force, ForceMode.Acceleration);


            //Calculate friction Force
            frictionforce = Character.velocity * -friction;
            if (frictionforce.y > 0)
            {
                frictionforce.y = -1;
            }

            //Apply friction force
            Character.AddForce(frictionforce, ForceMode.Acceleration);

        }
        //handle air movement
        //else
        //{
        //    placeholder.x = 0f;
        //    placeholder.y = 0f;
        //    placeholder.z = 0f;
        //    Character.AddForce(placeholder, ForceMode.Acceleration);
        //}

        //Reset Collision called set
        CollisionStayCalled = false;

        Character.angularVelocity = Vector3.zero;
    }



    




    //void Update()
    //{
    //    if (!isGrounded)
    //    {

    //    }





    //    Character.addforce(HorizontalVelocity * Time.deltaTime, ForceMode.Acceleration);
    //    Character.rotation += RotationVelocity * Time.deltaTime;
    //}
}
