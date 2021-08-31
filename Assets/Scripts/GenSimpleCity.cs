using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenSimpleCity : MonoBehaviour
{
    //012 Marble
    //013 WoodFloor
    //014 

    public int Xlen = 1;
    public int Ylen = 1;
    public int Zlen = 1;

    public byte[,,] TotalMapData;

    private int Xsize;
    private int Ysize;
    private int Zsize;

    void setlengths()
    {
        Xsize = Xlen * 16;
        Ysize = Ylen * 16;
        Zsize = Zlen * 16;
    }
    void Start()
    {
        setlengths();
    }




}
