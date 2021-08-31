using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mapvoxel : MonoBehaviour
{
    public int Xindex = 0;
    public int Yindex = 0;
    public int Zindex = 0;

    public float Scale;
    public byte Numwidth = 16;
    public GenSimpleMap TotalMap;

    
    private Mesh mesh;
    private Vector3[] Verticies;
    private int[] Triangles;
    private Color[] Colors;
    private ushort vertlen = 0;

    public byte[,,] Mapinfo;

    private Vector2[] UVS;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;


        //convert voxel boundries into quads/tris
        Createtris();

        //update mesh to display voxel boundries
        updatemesh();
    }


    private void definebounds()
    {

        vertlen = 0;
        byte[,,] zpos = new byte[Numwidth, Numwidth, Numwidth];
        byte[,,] zneg = new byte[Numwidth, Numwidth, Numwidth];
        byte[,,] ypos = new byte[Numwidth, Numwidth, Numwidth];
        byte[,,] yneg = new byte[Numwidth, Numwidth, Numwidth];
        byte[,,] xpos = new byte[Numwidth, Numwidth, Numwidth];
        byte[,,] xneg = new byte[Numwidth, Numwidth, Numwidth];

        //Small Section to evaluate if the current chunk is an edge chunk
        bool[] neighbors = new bool[6];
        for (byte i = 0; i < 6; i++) { neighbors[i] = true; }

        if (Xindex == 0) { neighbors[0] = false; }
        if (Xindex == (TotalMap.Xlen - 1) * 16) { neighbors[1] = false; }
        if (Yindex == 0) { neighbors[2] = false; }
        if (Yindex == (TotalMap.Ylen - 1) * 16) { neighbors[3] = false; }
        if (Zindex == 0) { neighbors[4] = false; }
        if (Zindex == (TotalMap.Zlen - 1) * 16) { neighbors[5] = false; }

        //Set Default values for final result
        for (byte i = 0; i < Numwidth; i++)
        {
            for (byte j = 0; j < Numwidth; j++)
            {
                for (byte k = 0; k < Numwidth; k++)
                {
                    zpos[i, j, k] = 0;
                    zneg[i, j, k] = 0;
                    ypos[i, j, k] = 0;
                    yneg[i, j, k] = 0;
                    xpos[i, j, k] = 0;
                    xneg[i, j, k] = 0;
                }
            }
        }


        byte xprev = 0;
        byte yprev = 0;
        byte zprev = 0;

        byte xnext = 0;
        byte ynext = 0;
        byte znext = 0;



        for (byte i = 0; i < Numwidth; i++)
        {
            for (byte j = 0; j < Numwidth; j++)
            {
                for (byte k = 0; k < Numwidth; k++)
                {
                    //Take Side info convert to bounds
                    if (TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex] != 0)
                    {
                        //X boundries
                        if (i == 0 && !neighbors[0])
                        {
                            xprev = 0;
                            xnext = TotalMap.TotalMapData[i + Xindex + 1, j + Yindex, k + Zindex];
                        }
                        else if (i == Numwidth - 1 && !neighbors[1])
                        {
                            xnext = 0;
                            xprev = TotalMap.TotalMapData[i + Xindex - 1, j + Yindex, k + Zindex];
                        }
                        else
                        {
                            xprev = TotalMap.TotalMapData[i + Xindex - 1, j + Yindex, k + Zindex];
                            xnext = TotalMap.TotalMapData[i + Xindex + 1, j + Yindex, k + Zindex];
                        }
                        //Y boundries
                        if (j == 0 && !neighbors[2])
                        {
                            yprev = 0;
                            ynext = TotalMap.TotalMapData[i + Xindex, j + Yindex + 1, k + Zindex];
                        }
                        else if (j == Numwidth - 1 && !neighbors[3])
                        {
                            ynext = 0;
                            yprev = TotalMap.TotalMapData[i + Xindex, j + Yindex - 1, k + Zindex];
                        }
                        else
                        {
                            yprev = TotalMap.TotalMapData[i + Xindex, j + Yindex - 1, k + Zindex];
                            ynext = TotalMap.TotalMapData[i + Xindex, j + Yindex + 1, k + Zindex];
                        }

                        //Z boundries
                        if (k == 0 && !neighbors[4])
                        {
                            zprev = 0;
                            znext = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex + 1];
                        }
                        else if (k == Numwidth - 1 && !neighbors[5])
                        {
                            znext = 0;
                            zprev = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex - 1];
                        }
                        else
                        {
                            zprev = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex - 1];
                            znext = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex + 1];
                        }

                        //Assing values to sets able to be rendered in the next function
                        if (zprev == 0)
                        {
                            zneg[i, j, k] = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex];
                            vertlen += 4;
                        }
                        if (znext == 0)
                        {
                            zpos[i, j, k] = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex];
                            vertlen += 4;
                        }
                        if (yprev == 0)
                        {
                            yneg[i, j, k] = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex];
                            vertlen += 4;
                        }
                        if (ynext == 0)
                        {
                            ypos[i, j, k] = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex];
                            vertlen += 4;
                        }
                        if (xprev == 0)
                        {
                            xneg[i, j, k] = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex];
                            vertlen += 4;
                        }
                        if (xnext == 0)
                        {
                            xpos[i, j, k] = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex];
                            vertlen += 4;
                        }
                    }
                }
            }
        }
    }

    byte[,,,] GreedyMesh(byte[,,] Array, byte Type)
    {
        byte[,,,] FinalArray = new byte[Numwidth, Numwidth, Numwidth, 2];

        bool[,] Checkmap = new bool[Numwidth, Numwidth];

        for (byte i = 0; i < Numwidth; i++)
        {
            for (byte j = 0; j < Numwidth; j++)
            {
                Checkmap[i, j] = false;
                for (byte k = 0; k < Numwidth; k++)
                {
                    FinalArray[i, j, k, 0] = 0;
                    FinalArray[i, j, k, 1] = 0;
                }
            }
        }

        bool Failed = false;

        // Optimize along X direction
        if (Type == 0)
        {
            byte J = 0;
            byte K = 0;


            for (byte i = 0; i < Numwidth; i++)
            {
                for (byte j = 0; j < Numwidth; j++)
                {
                    for (byte k = 0; k < Numwidth; k++)
                    {
                        if (!Checkmap[j, k] && Array[i, j, k] != 0)
                        {
                            FinalArray[i, j, k, 0] = 1;
                            FinalArray[i, j, k, 1] = 1;

                            byte Ylen = 1;
                            byte Zlen = 1;
                            J = (byte)(1 + j);
                            while (J < Numwidth && !Failed)
                            {
                                if (Array[i, J, k] != Array[i, j, k])
                                {
                                    Failed = true;
                                }
                                else
                                {
                                    Ylen++;
                                }

                                J++;
                            }

                            vertlen -= (ushort)(2 * Ylen);
                            for (byte B = j; B < Ylen + j; B++)
                            {
                                Checkmap[B, k] = true;
                            }
                            FinalArray[i, j, k, 0] = Ylen;

                            Failed = false;
                            J = j;
                            K = (byte)(k + 1);

                            while (K < Numwidth && !Failed)
                            {
                                // Check the entirety of next set for if it can grow 
                                while (J < j + Ylen && !Failed)
                                {
                                    if (Checkmap[J, K] || Array[i, J, K] != Array[i, j, k])
                                    {
                                        Failed = true;
                                    }
                                    J++;
                                }

                                J = j;
                                // if it can grow change appropriate elements
                                if (!Failed)
                                {
                                    for (byte Y = J; Y < J + Ylen; Y++)
                                    {
                                        Checkmap[Y, K] = true;
                                    }
                                    FinalArray[i, j, k, 1] += 1;
                                    vertlen -= (ushort)(Ylen + 2);
                                }
                                K++;
                            }
                        }
                    }
                }
                //reset Checked map here
                for (byte j = 0; j < Numwidth; j++)
                {
                    for (byte k = 0; k < Numwidth; k++)
                    {
                        Checkmap[j, k] = false;
                    }
                }
            }
        }
        // Optimize along Y direction
        if (Type == 1)
        {
            byte I = 0;
            byte K = 0;


            for (byte j = 0; j < Numwidth; j++)
            {
                for (byte i = 0; i < Numwidth; i++)
                {
                    for (byte k = 0; k < Numwidth; k++)
                    {
                        if (!Checkmap[i, k] && Array[i, j, k] != 0)
                        {
                            FinalArray[i, j, k, 0] = 1;
                            FinalArray[i, j, k, 1] = 1;

                            byte Xlen = 1;
                            byte Zlen = 1;
                            I = (byte)(1 + i);
                            while (I < Numwidth && !Failed)
                            {
                                if (Array[I, j, k] != Array[i, j, k])
                                {
                                    Failed = true;
                                }
                                else
                                {
                                    Xlen++;
                                }

                                I++;
                            }

                            vertlen -= (ushort)(2 * Xlen);
                            for (byte A = i; A < Xlen + i; A++)
                            {
                                Checkmap[A, k] = true;
                            }
                            FinalArray[i, j, k, 0] = Xlen;

                            Failed = false;
                            I = i;
                            K = (byte)(k + 1);

                            while (K < Numwidth && !Failed)
                            {
                                // Check the entirety of next set for if it can grow 
                                while (I < i + Xlen && !Failed)
                                {
                                    if (Checkmap[I, K] || Array[I, j, K] != Array[i, j, k])
                                    {
                                        Failed = true;
                                    }
                                    I++;
                                }

                                I = i;
                                // if it can grow change appropriate elements
                                if (!Failed)
                                {
                                    for (byte X = I; X < I + Xlen; X++)
                                    {
                                        Checkmap[X, K] = true;
                                    }
                                    FinalArray[i, j, k, 1] += 1;
                                    vertlen -= (ushort)(Xlen + 2);
                                }
                                K++;
                            }
                        }
                    }
                }
                //reset Checked map here
                for (byte i = 0; i < Numwidth; i++)
                {
                    for (byte k = 0; k < Numwidth; k++)
                    {
                        Checkmap[i, k] = false;
                    }
                }
            }
        }
        // Optimize along Z direction
        if (Type == 2)
        {
            byte I = 0;
            byte J = 0;


            for (byte k = 0; k < Numwidth; k++)
            {
                for (byte i = 0; i < Numwidth; i++)
                {
                    for (byte j = 0; j < Numwidth; j++)
                    {
                        if (!Checkmap[i, j] && Array[i, j, k] != 0)
                        {
                            FinalArray[i, j, k, 0] = 1;
                            FinalArray[i, j, k, 1] = 1;

                            byte Xlen = 1;
                            byte Ylen = 1;
                            I = (byte)(1 + i);
                            J = j;

                            while (I < Numwidth && !Failed)
                            {
                                if (Array[I, j, k] != Array[i, j, k])
                                {
                                    Failed = true;
                                }
                                else
                                {
                                    Xlen++;
                                }

                                I++;
                            }

                            vertlen -= (ushort)(2 * Xlen);
                            for (byte A = i; A < Xlen + i; A++)
                            {
                                Checkmap[A, j] = true;
                            }
                            FinalArray[i, j, k, 0] = Xlen;

                            Failed = false;
                            I = i;
                            J = (byte)(j + 1);

                            while (J < Numwidth && !Failed)
                            {
                                // Check the entirety of next set for if it can grow 
                                while (I < i + Xlen && !Failed)
                                {
                                    if (Checkmap[I, J] || Array[I, J, k] != Array[i, j, k])
                                    {
                                        Failed = true;
                                    }
                                    I++;
                                }
                                I = i;
                                // if it can grow change appropriate elements
                                if (!Failed)
                                {
                                    for (byte X = I; X < I + Xlen; X++)
                                    {
                                        Checkmap[X, J] = true;
                                    }
                                    FinalArray[i, j, k, 1] += 1;
                                    vertlen -= (ushort)(Xlen + 2);
                                }
                                J++;
                            }
                        }
                    }
                }
                //reset Checked map here
                for (byte i = 0; i < Numwidth; i++)
                {
                    for (byte j = 0; j < Numwidth; j++)
                    {
                        Checkmap[i, j] = false;
                    }
                }
            }
        }

        return FinalArray;
    }

    public void Createtris()
    {
        byte[,,,] XposTest = new byte[Numwidth, Numwidth, Numwidth, 2];
        byte[,,,] XnegTest = new byte[Numwidth, Numwidth, Numwidth, 2];
        byte[,,,] YposTest = new byte[Numwidth, Numwidth, Numwidth, 2];
        byte[,,,] YnegTest = new byte[Numwidth, Numwidth, Numwidth, 2];
        byte[,,,] ZposTest = new byte[Numwidth, Numwidth, Numwidth, 2];
        byte[,,,] ZnegTest = new byte[Numwidth, Numwidth, Numwidth, 2];

        //ZposTest = GreedyMesh(zpos, 2);
        //ZnegTest = GreedyMesh(zneg, 2);
        //YposTest = GreedyMesh(ypos, 1);
        //YnegTest = GreedyMesh(yneg, 1);
        //XposTest = GreedyMesh(xpos, 0);
        //XnegTest = GreedyMesh(xneg, 0);


        //Define Bounds Code copied{
        vertlen = 0;
        byte[,,] zpos = new byte[Numwidth, Numwidth, Numwidth];
        byte[,,] zneg = new byte[Numwidth, Numwidth, Numwidth];
        byte[,,] ypos = new byte[Numwidth, Numwidth, Numwidth];
        byte[,,] yneg = new byte[Numwidth, Numwidth, Numwidth];
        byte[,,] xpos = new byte[Numwidth, Numwidth, Numwidth];
        byte[,,] xneg = new byte[Numwidth, Numwidth, Numwidth];

        //Small Section to evaluate if the current chunk is an edge chunk
        bool[] neighbors = new bool[6];
        for (byte i = 0; i < 6; i++) { neighbors[i] = true; }

        if (Xindex == 0) { neighbors[0] = false; }
        if (Xindex == (TotalMap.Xlen - 1) * 16) { neighbors[1] = false; }
        if (Yindex == 0) { neighbors[2] = false; }
        if (Yindex == (TotalMap.Ylen - 1) * 16) { neighbors[3] = false; }
        if (Zindex == 0) { neighbors[4] = false; }
        if (Zindex == (TotalMap.Zlen - 1) * 16) { neighbors[5] = false; }

        //Set Default values for final result
        for (byte i = 0; i < Numwidth; i++)
        {
            for (byte j = 0; j < Numwidth; j++)
            {
                for (byte k = 0; k < Numwidth; k++)
                {
                    zpos[i, j, k] = 0;
                    zneg[i, j, k] = 0;
                    ypos[i, j, k] = 0;
                    yneg[i, j, k] = 0;
                    xpos[i, j, k] = 0;
                    xneg[i, j, k] = 0;
                }
            }
        }
        
        for (byte i = 0; i < Numwidth; i++)
        {
            for (byte j = 0; j < Numwidth; j++)
            {
                for (byte k = 0; k < Numwidth; k++)
                {
                    byte xprev = 0;
                    byte yprev = 0;
                    byte zprev = 0;

                    byte xnext = 0;
                    byte ynext = 0;
                    byte znext = 0;

                    //Take Side info convert to bounds
                    if (TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex] != 0)
                    {
                        //X boundries
                        if (i == 0 && !neighbors[0])
                        {
                            xprev = 0;
                            xnext = TotalMap.TotalMapData[i + Xindex + 1, j + Yindex, k + Zindex];
                        }
                        else if (i == Numwidth - 1 && !neighbors[1])
                        {
                            xnext = 0;
                            xprev = TotalMap.TotalMapData[i + Xindex - 1, j + Yindex, k + Zindex];
                        }
                        else
                        {
                            xprev = TotalMap.TotalMapData[i + Xindex - 1, j + Yindex, k + Zindex];
                            xnext = TotalMap.TotalMapData[i + Xindex + 1, j + Yindex, k + Zindex];
                        }
                        //Y boundries
                        if (j == 0 && !neighbors[2])
                        {
                            yprev = 0;
                            ynext = TotalMap.TotalMapData[i + Xindex, j + Yindex + 1, k + Zindex];
                        }
                        else if (j == Numwidth - 1 && !neighbors[3])
                        {
                            ynext = 0;
                            yprev = TotalMap.TotalMapData[i + Xindex, j + Yindex - 1, k + Zindex];
                        }
                        else
                        {
                            yprev = TotalMap.TotalMapData[i + Xindex, j + Yindex - 1, k + Zindex];
                            ynext = TotalMap.TotalMapData[i + Xindex, j + Yindex + 1, k + Zindex];
                        }

                        //Z boundries
                        if (k == 0 && !neighbors[4])
                        {
                            zprev = 0;
                            znext = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex + 1];
                        }
                        else if (k == Numwidth - 1 && !neighbors[5])
                        {
                            znext = 0;
                            zprev = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex - 1];
                        }
                        else
                        {
                            zprev = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex - 1];
                            znext = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex + 1];
                        }

                        //Assing values to sets able to be rendered in the next function
                        if (zprev == 0)
                        {
                            zneg[i, j, k] = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex];
                            vertlen += 4;
                        }
                        if (znext == 0)
                        {
                            zpos[i, j, k] = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex];
                            vertlen += 4;
                        }
                        if (yprev == 0)
                        {
                            yneg[i, j, k] = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex];
                            vertlen += 4;
                        }
                        if (ynext == 0)
                        {
                            ypos[i, j, k] = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex];
                            vertlen += 4;
                        }
                        if (xprev == 0)
                        {
                            xneg[i, j, k] = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex];
                            vertlen += 4;
                        }
                        if (xnext == 0)
                        {
                            xpos[i, j, k] = TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex];
                            vertlen += 4;
                        }
                    }
                }
            }
        }

        //End Define bounds section

        for (byte i = 0; i < Numwidth; i++)
        {
            for (byte j = 0; j < Numwidth; j++)
            {
                for (byte k = 0; k < Numwidth; k++)
                {
                    for (byte w = 0; w < 2; w++)
                    {
                        ZposTest[i, j, k, w] = 1;
                        ZnegTest[i, j, k, w] = 1;
                        XposTest[i, j, k, w] = 1;
                        XnegTest[i, j, k, w] = 1;
                        YposTest[i, j, k, w] = 1;
                        YnegTest[i, j, k, w] = 1;

                    }
                }
            }
        }



        Verticies = new Vector3[vertlen];
        Triangles = new int[vertlen * 6];
        UVS = new Vector2[vertlen];
        Colors = new Color[vertlen];


        Vector3 holder;
        int U = 0;
        for (ushort i = 0, T = 0; i < Numwidth; i++)
        {
            for (byte j = 0; j < Numwidth; j++)
            {
                for (byte k = 0; k < Numwidth; k++)
                {
                    //Build Zneg
                    if (zneg[i, j, k] != 0 && ZnegTest[i, j, k, 0] != 0)
                    {
                        holder = new Vector3(i, j, k) * Scale;
                        Verticies[T] = holder;
                        Verticies[T + 1] = holder + (Vector3.up * ZnegTest[i, j, k, 1] * Scale);
                        Verticies[T + 2] = holder + (Vector3.right * ZnegTest[i, j, k, 0] * Scale);
                        Verticies[T + 3] = holder + ((Vector3.right * ZnegTest[i, j, k, 0] + Vector3.up * ZnegTest[i, j, k, 1]) * Scale);


                        UVS[T] = new Vector2(Numwidth - i, j) / Numwidth;
                        UVS[T + 1] = new Vector2(Numwidth - i, j + ZnegTest[i, j, k, 1]) / Numwidth;
                        UVS[T + 2] = new Vector2(Numwidth - (i + ZnegTest[i, j, k, 0]), j) / Numwidth;
                        UVS[T + 3] = new Vector2(Numwidth - (i + ZnegTest[i, j, k, 0]), j + ZnegTest[i, j, k, 1]) / Numwidth;


                        for (int A = 0; A <= 3; A++)
                        {
                            Colors[A + T] = new Color(TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex] - 1, 255, 0, 0);
                        }


                        Triangles[U + 2] = T;
                        Triangles[U + 1] = T + 2;
                        Triangles[U] = T + 1;

                        Triangles[U + 5] = T + 3;
                        Triangles[U + 4] = T + 1;
                        Triangles[U + 3] = T + 2;


                        U = U + 6;
                        T = (ushort)(T + 4);
                    }
                    //Build Zpos
                    if (zpos[i, j, k] != 0 && ZposTest[i, j, k, 0] != 0)
                    {
                        holder = new Vector3(i, j, k + 1) * Scale;

                        Verticies[T] = holder;
                        Verticies[T + 1] = holder + (Vector3.up * ZposTest[i, j, k, 1] * Scale);
                        Verticies[T + 2] = holder + (Vector3.right * ZposTest[i, j, k, 0] * Scale);
                        Verticies[T + 3] = holder + ((Vector3.right * ZposTest[i, j, k, 0] + Vector3.up * ZposTest[i, j, k, 1]) * Scale);

                        UVS[T] = new Vector2(i, j) / Numwidth;
                        UVS[T + 1] = new Vector2(i, j + ZposTest[i, j, k, 1]) / Numwidth;
                        UVS[T + 2] = new Vector2(i + ZposTest[i, j, k, 0], j) / Numwidth;
                        UVS[T + 3] = new Vector2(i + ZposTest[i, j, k, 0], j + ZposTest[i, j, k, 1]) / Numwidth;


                        for (int A = 0; A <= 3; A++)
                        {
                            Colors[A + T] = new Color(TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex] - 1, 255, 0, 0);
                        }


                        Triangles[U] = T;
                        Triangles[U + 1] = T + 2;
                        Triangles[U + 2] = T + 1;

                        Triangles[U + 3] = T + 3;
                        Triangles[U + 4] = T + 1;
                        Triangles[U + 5] = T + 2;


                        U = U + 6;
                        T = (ushort)(T + 4);
                    }
                    //Build Yneg
                    if (yneg[i, j, k] != 0 && YnegTest[i, j, k, 0] != 0)
                    {
                        holder = new Vector3(i, j, k) * Scale;
                        Verticies[T] = holder;
                        Verticies[T + 1] = holder + (Vector3.forward * YnegTest[i, j, k, 1] * Scale);
                        Verticies[T + 2] = holder + (Vector3.right * YnegTest[i, j, k, 0] * Scale);
                        Verticies[T + 3] = holder + ((Vector3.right * YnegTest[i, j, k, 0] + Vector3.forward * YnegTest[i, j, k, 1]) * Scale);


                        UVS[T] = new Vector2(i, k) / Numwidth;
                        UVS[T + 1] = new Vector2(i, k + YnegTest[i, j, k, 1]) / Numwidth;
                        UVS[T + 2] = new Vector2(i + YnegTest[i, j, k, 0], k) / Numwidth;
                        UVS[T + 3] = new Vector2(i + YnegTest[i, j, k, 0], k + YnegTest[i, j, k, 1]) / Numwidth;


                        for (int A = 0; A <= 3; A++)
                        {
                            Colors[A + T] = new Color(TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex] - 1, 255, 0, 0);
                        }

                        Triangles[U + 2] = T;
                        Triangles[U + 1] = T + 1;
                        Triangles[U + 0] = T + 2;

                        Triangles[U + 5] = T + 1;
                        Triangles[U + 4] = T + 3;
                        Triangles[U + 3] = T + 2;


                        U = U + 6;
                        T = (ushort)(T + 4);
                    }
                    //Build Ypos
                    if (ypos[i, j, k] != 0 && YposTest[i, j, k, 0] != 0)
                    {
                        holder = new Vector3(i, j + 1, k) * Scale;
                        Verticies[T] = holder;
                        Verticies[T + 1] = holder + (Vector3.forward * YposTest[i, j, k, 1] * Scale);
                        Verticies[T + 2] = holder + (Vector3.right * YposTest[i, j, k, 0] * Scale);
                        Verticies[T + 3] = holder + ((Vector3.right * YposTest[i, j, k, 0] + Vector3.forward * YposTest[i, j, k, 1]) * Scale);

                        UVS[T] = new Vector2(i, k) / Numwidth;
                        UVS[T + 1] = new Vector2(i, k + YposTest[i, j, k, 1]) / Numwidth;
                        UVS[T + 2] = new Vector2(i + YposTest[i, j, k, 1], k) / Numwidth;
                        UVS[T + 3] = new Vector2(i + YposTest[i, j, k, 1], k + YposTest[i, j, k, 1]) / Numwidth;


                        for (int A = 0; A <= 3; A++)
                        {
                            Colors[A + T] = new Color(TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex] - 1, 255, 0, 0);
                        }

                        Triangles[U] = T;
                        Triangles[U + 1] = T + 1;
                        Triangles[U + 2] = T + 2;

                        Triangles[U + 3] = T + 1;
                        Triangles[U + 4] = T + 3;
                        Triangles[U + 5] = T + 2;


                        U = U + 6;
                        T = (ushort)(T + 4);
                    }
                    //Build Xneg
                    if (xneg[i, j, k] != 0 && XnegTest[i, j, k, 0] != 0)
                    {
                        holder = new Vector3(i, j, k) * Scale;
                        Verticies[T] = holder;
                        Verticies[T + 1] = holder + (Vector3.forward * XnegTest[i, j, k, 1] * Scale);
                        Verticies[T + 2] = holder + (Vector3.up * XnegTest[i, j, k, 0] * Scale);
                        Verticies[T + 3] = holder + ((Vector3.forward * XnegTest[i, j, k, 1] + Vector3.up * XnegTest[i, j, k, 0]) * Scale);

                        UVS[T] = new Vector2(k, j) / Numwidth;
                        UVS[T + 1] = new Vector2(k + XnegTest[i, j, k, 1], j) / Numwidth;
                        UVS[T + 2] = new Vector2(k, j + XnegTest[i, j, k, 0]) / Numwidth;
                        UVS[T + 3] = new Vector2(k + XnegTest[i, j, k, 1], j + XnegTest[i, j, k, 0]) / Numwidth;


                        for (int A = 0; A <= 3; A++)
                        {
                            Colors[A + T] = new Color(TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex] - 1, 255, 0, 0);
                        }

                        Triangles[U] = T;
                        Triangles[U + 1] = T + 1;
                        Triangles[U + 2] = T + 2;

                        Triangles[U + 3] = T + 1;
                        Triangles[U + 4] = T + 3;
                        Triangles[U + 5] = T + 2;


                        U = U + 6;
                        T = (ushort)(T + 4);
                    }
                    //Build Xpos
                    if (xpos[i, j, k] != 0 && XposTest[i, j, k, 0] != 0)
                    {
                        holder = new Vector3(i + 1, j, k) * Scale;
                        Verticies[T] = holder;
                        Verticies[T + 1] = holder + (Vector3.forward * XposTest[i, j, k, 1] * Scale);
                        Verticies[T + 2] = holder + (Vector3.up * XposTest[i, j, k, 0] * Scale);
                        Verticies[T + 3] = holder + ((Vector3.forward * XposTest[i, j, k, 1] + Vector3.up * XposTest[i, j, k, 0]) * Scale);

                        UVS[T] = new Vector2(Numwidth - k, j) / Numwidth;
                        UVS[T + 1] = new Vector2(Numwidth - (k + XposTest[i, j, k, 1]), j) / Numwidth;
                        UVS[T + 2] = new Vector2(Numwidth - k, j + XposTest[i, j, k, 0]) / Numwidth;
                        UVS[T + 3] = new Vector2(Numwidth - (k + XposTest[i, j, k, 1]), j + XposTest[i, j, k, 0]) / Numwidth;


                        for (int A = 0; A <= 3; A++)
                        {
                            Colors[A + T] = new Color(TotalMap.TotalMapData[i + Xindex, j + Yindex, k + Zindex] - 1, 255, 0, 0);
                        }

                        Triangles[U + 2] = T;
                        Triangles[U + 1] = T + 1;
                        Triangles[U] = T + 2;

                        Triangles[U + 5] = T + 1;
                        Triangles[U + 4] = T + 3;
                        Triangles[U + 3] = T + 2;


                        U = U + 6;
                        T = (ushort)(T + 4);
                    }
                }
            }
        }

    }

    public void updatemesh()
    {
        mesh.Clear();

        mesh.vertices = Verticies;
        mesh.uv = UVS;
        mesh.triangles = Triangles;
        mesh.colors = Colors;

        mesh.RecalculateNormals();
    }

}