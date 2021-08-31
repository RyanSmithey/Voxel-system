using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyCollider : MonoBehaviour
{
    
    void Start()
    {
        GetMap();
        generatecollider();
    }

    public int Xindex = 0;
    public int Yindex = 0;
    public int Zindex = 0;

    public GenSimpleMap TotalMap;

    private int Numwidth;
    private bool[,,] Collidermap;

    private BoxCollider[] BoxArray;
    //each index represents a new colldier

    public void GetMap()
    {
        Numwidth = gameObject.GetComponent<Mapvoxel>().Numwidth;

        Collidermap = new bool[Numwidth, Numwidth, Numwidth];
        for (int i = 0; i < Numwidth; i++)
        {
            for (int j = 0; j < Numwidth; j++)
            {
                for (int k = 0; k < Numwidth; k++)
                {
                    if (TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex] != 0)
                    {
                        Collidermap[i, j, k] = true;
                    }
                    else
                    {
                        Collidermap[i, j, k] = false;
                    }
                }
            }
        }
    }


    // optimize collider scale and position to use minimun number of box colliders
    public void generatecollider()
    {
        BoxCollider CurrentCollider;

        bool[,,] CheckedMap;

        CheckedMap = new bool[Numwidth, Numwidth, Numwidth];
        bool[,,] PrevCheckMap = new bool[Numwidth, Numwidth, Numwidth];

        for (int i = 0; i < Numwidth; i++)
        {
            for (int j = 0; j < Numwidth; j++)
            {
                for (int k = 0; k < Numwidth; k++)
                {
                    CheckedMap[i, j, k] = false;
                }
            }
        }


        Vector3[] Scaling = new Vector3[2048];
        Vector3[] Position = new Vector3[2048];

        bool failed = false;


        int I = 1;
        int J = 0;
        int K = 0;


        for (int i = 0, numcolliders = 0; i < Numwidth; i++)
        {
            for (int j = 0; j < Numwidth; j++)
            {
                for (int k = 0; k < Numwidth; k++)
                {
                    if (Collidermap[i, j, k] && !CheckedMap[i, j, k])
                    {
                        Scaling[numcolliders] = Vector3.one;
                        Position[numcolliders] = (Vector3.one / 2) + new Vector3(i, j, k);
                        I = 1 + i;
                        J = j;
                        K = k;

                        int Xlen = 1;
                        int Ylen = 1;

                        while (I < Numwidth)
                        {
                            if (Collidermap[I, J, K] && !CheckedMap[I, J, K])
                            {
                                Scaling[numcolliders] += Vector3.right;
                                Position[numcolliders] += Vector3.right / 2;
                                Xlen++;
                                CheckedMap[I, J, K] = true;
                            }
                            else
                            {
                                I = Numwidth;
                            }

                            I++;
                        }

                        K = k;
                        J = 1 + j;
                        I = i;

                        failed = false;

                        while (J < Numwidth && !failed)
                        {
                            for (int x = i; x < Xlen + i; x++)
                            {
                                if (!Collidermap[x, J, K] || CheckedMap[x, J, K])
                                {
                                    failed = true;
                                }
                            }
                            if (J < Numwidth && !failed)
                            {
                                Scaling[numcolliders] += Vector3.up;
                                Position[numcolliders] += Vector3.up / 2;

                                for (int x = i; x < Xlen + i; x++)
                                {
                                    CheckedMap[x, J, K] = true;
                                }

                                Ylen++;
                            }
                            
                            J++;
                        }
                        failed = false;

                        K = 1 + k;
                        J = j;
                        I = i;


                        while (K < Numwidth && !failed)
                        {
                            for (int y = j; y < Ylen + j; y++)
                            {
                                for (int x = i; x < Xlen + i; x++)
                                {
                                    if (!Collidermap[x, y, K] || CheckedMap[x, y, K])
                                    {
                                        failed = true;
                                    }
                                }
                            }
                            
                            if (J < Numwidth && !failed)
                            {
                                Scaling[numcolliders] += Vector3.forward;
                                Position[numcolliders] += Vector3.forward / 2;

                                for (int y = j; y < Ylen + j; y++)
                                {
                                    for (int x = i; x < Xlen + i; x++)
                                    {
                                        CheckedMap[x, y, K] = true;
                                    }
                                }
                            }

                            K++;
                        }

                        numcolliders++;
                    }
                }
            }
        }


        int numberofcolliders = 0;
        for (int i = 0; i < 2048; i++)
        {
            if (Scaling[i] != Vector3.zero)
            {
                numberofcolliders++;
            }
        }


        BoxArray = new BoxCollider[numberofcolliders];

        for (int i = 0; i < 2048; i++)
        {
            if (Scaling[i] != Vector3.zero)
            {
                CurrentCollider = gameObject.AddComponent<BoxCollider>();
                CurrentCollider.size = Scaling[i] * gameObject.GetComponent<Mapvoxel>().Scale;
                CurrentCollider.center = Position[i] * gameObject.GetComponent<Mapvoxel>().Scale;
                BoxArray[i] = CurrentCollider;
            }
            else
            {
                i = 2048;
            }
        }
    }

    public void Reset()
    {
        foreach (BoxCollider i in BoxArray)
        {
            Destroy(i);
        }
    }
}
