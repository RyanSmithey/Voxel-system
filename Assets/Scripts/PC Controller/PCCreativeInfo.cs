using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCCreativeInfo : MonoBehaviour
{
    public bool Aim = true;
    public PCaim CAim;
    public bool Pull = false;
    public PCPull CPull;
    public bool Dig = false;
    public PCDig CDig;
    public bool Selector = true;
    public PCSelector CSelector;
    public bool Place = false;
    public PCPlace CPlace;


    public bool SinglePlace;

    public byte[,,] Blueprint1;
    public byte[,,] Blueprint2;
    public byte[,,] Blueprint3;
    public byte[,,] Blueprint4;
    public byte[,,] Blueprint5;
    public byte[,,] Blueprint6;
    public byte[,,] Blueprint7;
    public byte[,,] Blueprint8;
    public byte[,,] Blueprint9;

    public byte[] Blocks;

    public GenSimpleMap MapData;
    
    private RaycastHit MainHit;
    private float MaxRange = 2.5f;

    private byte SingleSelected = 0;
    private byte BlueprintSelected = 0;
    private bool Replace = false;


    void Start()
    {
        Blocks = new byte[9];
        for (byte i = 0; i < 9; i++)
        {
            Blocks[i] = i;
        }
    }

    void Update()
    {
        //Create a raycast for what the player is looking at
        bool Didhit = Physics.Raycast(gameObject.transform.position, gameObject.transform.rotation * Vector3.forward, out MainHit, MaxRange);


        //Allow Tab to switch between Single and blueprint mode
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (SinglePlace) {SinglePlace = false;}
            
            else { SinglePlace = true; }
        }

        //Change the block/blueprint used based on numberpad
        if (GatherInputNumber() != 10)
        {
            if (SinglePlace) { SingleSelected = GatherInputNumber(); }
            else { BlueprintSelected = GatherInputNumber(); }
        }

        //Run Code for single place mode
        if (SinglePlace)
        {
            //offset value to ensure selected block is the required block
            float offset = 0.02f;

            //If control + V is pressed
            if ((Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.V))
            {
                // Flip-flop replace if it is pressed
                if (Replace) { Replace = false; }
                else { Replace = true; }
            }

            //Change offset to negative value if it should replace the block
            if (Replace) { offset *= -1.0f; }

            uint[] Location;

            //Obtain Location based on raycast
            if (Didhit) { Location = MapData.FindBlock(MainHit.point + MainHit.normal * offset); }
            else { Location = MapData.FindBlock(gameObject.transform.rotation * Vector3.forward * MaxRange); }
            
            //If control + C is pressed
            if ((Input.GetKey(KeyCode.RightControl)|| Input.GetKey(KeyCode.LeftControl)) && Input.GetKeyDown(KeyCode.C))
            {
                uint[] AltLocation;

                if (Didhit) { Location = MapData.FindBlock(MainHit.point + MainHit.normal * -0.02f) ; }
                else { Location = MapData.FindBlock(gameObject.transform.rotation * Vector3.forward * MaxRange); }

                Blocks[SingleSelected] = MapData.TotalMapData[Location[0], Location[1], Location[2]];
            }

            byte[,,] Build;

            //If left click is pressed
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Build = new byte[1, 1, 1];
                Build[0, 0, 0] = Blocks[SingleSelected];
                MapData.BuildBlocks(Build, MainHit.point + MainHit.normal * offset, true);
            }

            //If right click is pressed
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Build = new byte[1, 1, 1];
                Build[0, 0, 0] = 0;
                MapData.BuildBlocks(Build, MainHit.point  + MainHit.normal * offset, true);
            }

            //Visualize cursor
            CSelector.Origin = Location;
            CSelector.End = Location;
            CSelector.Endpoint = gameObject.transform.rotation * Vector3.forward * MaxRange;
            CSelector.Main();
        }
        //run code for blueprint mode
        else
        {

        }
        
        if (Pull)
        {
            CPull.hit = MainHit;
            CPull.Main();
        }

        if (Dig)
        {
            CDig.hit = MainHit;
            CDig.Main();
        }

        if (Place)
        {
            CPlace.hit = MainHit;
            CPlace.Main();
        }
    }

    public byte[,,] AccessBlueprint(uint Index)
    {
        if (Index == 1)
        {
            return Blueprint1;
        }
        else if (Index == 2)
        {
            return Blueprint2;
        }
        else if (Index == 3)
        {
            return Blueprint3;
        }
        else if (Index == 4)
        {
            return Blueprint4;
        }
        else if (Index == 5)
        {
            return Blueprint5;
        }
        else if (Index == 6)
        {
            return Blueprint6;
        }
        else if (Index == 7)
        {
            return Blueprint7;
        }
        else if (Index == 8)
        {
            return Blueprint8;
        }
        else if (Index == 9)
        {
            return Blueprint9;
        }
        else
        {
            return null;
        }
    }

    private byte GatherInputNumber()
    {
        if (Input.GetKey(KeyCode.Alpha0)) { return 0; }
        else if (Input.GetKey(KeyCode.Alpha1)) { return 1; }
        else if (Input.GetKey(KeyCode.Alpha2)) { return 2; }
        else if (Input.GetKey(KeyCode.Alpha3)) { return 3; }
        else if (Input.GetKey(KeyCode.Alpha4)) { return 4; }
        else if (Input.GetKey(KeyCode.Alpha5)) { return 5; }
        else if (Input.GetKey(KeyCode.Alpha6)) { return 6; }
        else if (Input.GetKey(KeyCode.Alpha7)) { return 7; }
        else if (Input.GetKey(KeyCode.Alpha8)) { return 8; }
        else if (Input.GetKey(KeyCode.Alpha9)) { return 9; }
        else { return 10; }
    }
}
