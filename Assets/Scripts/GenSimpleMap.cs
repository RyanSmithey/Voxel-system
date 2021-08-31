using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenSimpleMap : MonoBehaviour
{
    public byte[,,] TotalMapData;
    public byte[,,] LightMap;

    public int Xlen = 1;
    public int Ylen = 1;
    public int Zlen = 1;

    public float perlinscale = .02f;
    public GameObject chunk;
    public float Scale = .2f;

    private int Xsize;
    private int Ysize;
    private int Zsize;

    private Mapvoxel[,,] Chunklist;
    private ApplyCollider[,,] Colliderlist;

    void setlengths()
    {
        Xsize = Xlen * 16;
        Ysize = Ylen * 16;
        Zsize = Zlen * 16;
    }


    public int type = 1;


    // Start is called before the first frame update
    void Start()
    {
        setlengths();
        if (type == 0)
        {
            GenFlatmap();
        }
        else if (type == 1)
        {
            GenPerlinmap();
        }
        else
        {
            GenFlatmap();
        }
        createchunks();
    }

    void createchunks()
    {
        GameObject currentobj;

        Chunklist = new Mapvoxel[Xlen, Ylen, Zlen];
        Colliderlist = new ApplyCollider[Xlen, Ylen, Zlen];


        for (int i = 0; i < Xlen; i++)
        {
            for (int j = 0; j < Ylen; j++)
            {
                for (int k = 0; k < Zlen; k++)
                {
                    currentobj = Instantiate(chunk, gameObject.GetComponent<Transform>());

                    Chunklist[i, j, k] = currentobj.GetComponent<Mapvoxel>();
                    Colliderlist[i, j, k] = currentobj.GetComponent<ApplyCollider>();

                    Chunklist[i, j, k].TotalMap = gameObject.GetComponent<GenSimpleMap>();
                    Chunklist[i, j, k].Scale = Scale;
                    Colliderlist[i, j, k].TotalMap = gameObject.GetComponent<GenSimpleMap>();

                    currentobj.transform.position = new Vector3(i * 16, j * 16, k * 16) * Scale;

                    Chunklist[i, j, k].Xindex = i * 16;
                    Chunklist[i, j, k].Yindex = j * 16;
                    Chunklist[i, j, k].Zindex = k * 16;

                    Colliderlist[i, j, k].Xindex = i * 16;
                    Colliderlist[i, j, k].Yindex = j * 16;
                    Colliderlist[i, j, k].Zindex = k * 16;
                }
            }
        }
    }

    void GenPerlinmap()
    {
        TotalMapData = new byte[Xsize, Ysize, Zsize];

        float offset = 2.654f;

        for (int i = 0; i < Xsize; i++)
        {
            for (int j = 0; j < Ysize; j++)
            {
                for (int k = 0; k < Zsize; k++)
                {
                    if (j < Mathf.PerlinNoise(i*perlinscale + offset, k * perlinscale + offset) * Ysize)
                    {
                        TotalMapData[i, j, k] = (byte)(Random.Range(1, 12));
                    }
                    else
                    {
                        TotalMapData[i, j, k] = 0;
                    }
                }
            }
        }
    }

    void GenFlatmap()
    {
        TotalMapData = new byte[Xsize, Ysize, Zsize];

        for (int i = 0; i < Xsize; i++)
        {
            for (int j = 0; j < Ysize; j++)
            {
                for (int k = 0; k < Zsize; k++)
                {
                    if (j <= 14)
                    {
                        TotalMapData[i, j, k] = 2;
                    }
                    else
                    {
                        TotalMapData[i, j, k] = 0;
                    }
                }
            }
        }
    }

    public void DamageBlocks(Vector3 Position,float Radius, Vector3 Scaling, byte Type = 0)
    {
        //print(Position);
        uint[] minmax = new uint[6];
        uint[] ChangedChunks = new uint[6];

        float distance;

        minmax[0] = (uint)(Mathf.Clamp(Mathf.Ceil((Position.x - (Radius * Scaling.x)) / Scale) - 1, 0, Xlen * 16 - 1));
        minmax[1] = (uint)(Mathf.Clamp(Mathf.Ceil((Position.x + (Radius * Scaling.x)) / Scale), 0, Xlen * 16 - 1));
        minmax[2] = (uint)(Mathf.Clamp(Mathf.Ceil((Position.y - (Radius * Scaling.y)) / Scale) - 1, 0, Ylen * 16 - 1));
        minmax[3] = (uint)(Mathf.Clamp(Mathf.Ceil((Position.y + (Radius * Scaling.y)) / Scale), 0, Ylen * 16 - 1));
        minmax[4] = (uint)(Mathf.Clamp(Mathf.Ceil((Position.z - (Radius * Scaling.z)) / Scale) - 1, 0, Zlen * 16 - 1));
        minmax[5] = (uint)(Mathf.Clamp(Mathf.Ceil((Position.z + (Radius * Scaling.z)) / Scale), 0, Zlen * 16 - 1));
        

        for (uint i = minmax[0]; i <= minmax[1]; i++)
        {
            for (uint j = minmax[2]; j <= minmax[3]; j++)
            {
                for (uint k = minmax[4]; k <= minmax[5]; k++)
                {
                    distance = new Vector3((((float)(i + .5) * Scale) - Position.x) / Scaling.x,
                                           (((float)(j + .5) * Scale) - Position.y) / Scaling.y,
                                           (((float)(k + .5) * Scale) - Position.z) / Scaling.z).sqrMagnitude;

                    if (Radius * Radius > distance)
                    {
                        TotalMapData[i, j, k] = 0;
                    }
                }
            }
        }

        ChangedChunks[0] = (uint)(Mathf.Clamp(Mathf.Ceil(minmax[0] / 16) - 1, 0, Xlen - 1));
        ChangedChunks[1] = (uint)(Mathf.Clamp(Mathf.Ceil(minmax[1] / 16), 0, Xlen - 1));
        ChangedChunks[2] = (uint)(Mathf.Clamp(Mathf.Ceil(minmax[2] / 16) - 1, 0, Ylen - 1));
        ChangedChunks[3] = (uint)(Mathf.Clamp(Mathf.Ceil(minmax[3] / 16), 0, Ylen - 1));
        ChangedChunks[4] = (uint)(Mathf.Clamp(Mathf.Ceil(minmax[4] / 16) - 1, 0, Zlen - 1));
        ChangedChunks[5] = (uint)(Mathf.Clamp(Mathf.Ceil(minmax[5] / 16), 0, Zlen - 1));


        for (uint i = ChangedChunks[0]; i <= ChangedChunks[1]; i++)
        {
            for (uint j = ChangedChunks[2]; j <= ChangedChunks[3]; j++)
            {
                for (uint k = ChangedChunks[4]; k <= ChangedChunks[5]; k++)
                {
                    Chunklist[i, j, k].Createtris();
                    Chunklist[i, j, k].updatemesh();

                    Colliderlist[i, j, k].Reset();
                    Colliderlist[i, j, k].GetMap();
                    Colliderlist[i, j, k].generatecollider();
                }
            }
        }
    }

    public void BuildBlocks(byte[,,] Build, Vector3 WorldLocation, bool replace = false)
    {
        //Establish bounds of the build array
        uint Build_Xsize = (uint)(Build.GetUpperBound(0) + 1);
        uint Build_Ysize = (uint)(Build.GetUpperBound(1) + 1);
        uint Build_Zsize = (uint)(Build.GetUpperBound(2) + 1);

        
        //Apply build to voxel space
        uint[] Position = FindBlock(WorldLocation);
        
        for (uint i = Position[0]; i < Mathf.Clamp(Position[0] + Build_Xsize, 0, Xsize); i++)
        {
            for (uint j = Position[1]; j < Mathf.Clamp(Position[1] + Build_Ysize, 0, Ysize); j++)
            {
                for (uint k = Position[2]; k < Mathf.Clamp(Position[2] + Build_Zsize, 0, Zsize); k++)
                {
                    //If replace is enabled replace the existing blocks
                    if (replace)
                    {
                        TotalMapData[i, j, k] = Build[i - Position[0], j - Position[1], k - Position[2]];
                    }
                    //Otherwise only place the block if the space is empty
                    else if (TotalMapData[i,j,k] == 0)
                    {
                        TotalMapData[i, j, k] = Build[i - Position[0], j - Position[1], k - Position[2]];
                    }
                }
            }
        }

        uint[] ChangedChunks = new uint[6];

        //Find Effected chunks
        ChangedChunks[0] = (uint)(Mathf.Clamp(Mathf.Ceil((Position[0] - 1) / 16), 0, Xlen));
        ChangedChunks[1] = (uint)(Mathf.Clamp(Mathf.Ceil((Position[0] + Build_Xsize + 1) / 16), 0, Xlen));
        ChangedChunks[2] = (uint)(Mathf.Clamp(Mathf.Ceil((Position[1] - 1) / 16), 0, Ylen));
        ChangedChunks[3] = (uint)(Mathf.Clamp(Mathf.Ceil((Position[1] + Build_Ysize + 1) / 16), 0, Ylen));
        ChangedChunks[4] = (uint)(Mathf.Clamp(Mathf.Ceil((Position[2] - 1) / 16), 0, Zlen));
        ChangedChunks[5] = (uint)(Mathf.Clamp(Mathf.Ceil((Position[2] + Build_Zsize + 1) / 16), 0, Zlen));


        //Update relevent chunks
        for (uint i = ChangedChunks[0]; i <= ChangedChunks[1]; i++)
        {
            for (uint j = ChangedChunks[2]; j <= ChangedChunks[3]; j++)
            {
                for (uint k = ChangedChunks[4]; k <= ChangedChunks[5]; k++)
                {
                    Chunklist[i, j, k].Createtris();
                    Chunklist[i, j, k].updatemesh();

                    Colliderlist[i, j, k].Reset();
                    Colliderlist[i, j, k].GetMap();
                    Colliderlist[i, j, k].generatecollider();
                }
            }
        }
    }

    public uint[] FindBlock(Vector3 hitpoint)
    {
        uint[] Final = new uint[3];

        hitpoint /= Scale;

        for (int i = 0; i < 3; i++)
        {
            hitpoint[i] = Mathf.Ceil(hitpoint[i]) - 1;
        }

        Final[0] = (uint)(Mathf.Clamp(hitpoint[0], 0, Xsize));
        Final[1] = (uint)(Mathf.Clamp(hitpoint[1], 0, Ysize));
        Final[2] = (uint)(Mathf.Clamp(hitpoint[2], 0, Zsize));
        return Final;
    }

    //Start by assuming that it is fixed above
    private void BakeLighting()
    {
        LightMap = new byte[Xsize, Ysize, Zsize];

        for (int j = Ysize - 1; j >= 0; j--) { for (int i = 0; i < Xsize; i++) { for (int k = 0; k < Zsize; k++) { LightMap[i, j, k] = 255; } } }
        

        bool[,] DirectLit = new bool[Xsize, Zsize];
        for (int i = 0; i < Xsize; i++) { for (int k = 0; k < Zsize; k++) { DirectLit[i, k] = true; } }


        //Sets direct lighting
        for (int j = Ysize - 1; j >= 0; j++)
        {
            for (int i = 0; i < Xsize; i++)
            {
                for (int k = 0; k < Zsize; k++)
                {
                    if (TotalMapData[i, j, k] > 0 && DirectLit[i, k])
                    {
                        DirectLit[i,k] = false;
                    }
                    if (DirectLit[i,k] && TotalMapData[i, j, k] == 0)
                    {
                        LightMap[i, j, k] = 0;
                    }
                }
            }
        }

    }

}
