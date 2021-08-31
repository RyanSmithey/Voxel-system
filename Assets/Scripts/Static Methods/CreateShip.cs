using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public static class CreateShip
{
    [MenuItem("GameObject/Create Sample Ship")]
    static void CreateSample()
    {
        uint Xsize = 10;
        uint Ysize = 10;
        uint Zsize = 10;

        byte[,,] Shipinfo = new byte[Xsize, Ysize, Zsize];
        byte[] Final = new byte[Xsize*Ysize*Zsize + 3];

        for(uint i = 0, Z = 0; i < Xsize; i++)
        {
            for (uint j = 0; j < Ysize; j++)
            {
                for (uint k = 0; k < Zsize; k++)
                {
                    if (i == 0 || i == Xsize - 1 || j == 0 || k == 0 || k == Zsize - 1)
                    {
                        Final[Z] = 11;
                    }
                    else
                    {
                        Final[Z] = 0;
                    }
                    Z++;
                }
            }
        }

        Final[Xsize * Ysize * Zsize] = (byte)(Xsize);
        Final[Xsize * Ysize * Zsize + 1] = (byte)(Ysize);
        Final[Xsize * Ysize * Zsize + 2] = (byte)(Zsize);


        File.WriteAllBytes("GameObjects/Shipinfo", Final);
    }
}
