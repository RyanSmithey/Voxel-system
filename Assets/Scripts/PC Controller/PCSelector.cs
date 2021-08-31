using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCSelector : MonoBehaviour
{
    public byte[,,] Selected;

    public MeshFilter Filter;

    public uint[] Origin;
    public uint[] End;

    public RaycastHit hit;
    public Vector3 Endpoint;

    public bool Replace;
    public bool Didhit;

    public GenSimpleMap Map;
    
    private Mesh mesh;
    private Vector3[] Verticies;
    private int[] Triangles;
    private float Scale = 0.2f;

    void Start()
    {
        Origin = new uint[3];
        End = new uint[3];
        
        mesh = new Mesh();

        Filter.mesh = mesh;
        DrawBox();
        //Updatemesh();
    }

    public void Main()
    {
        DrawBox();
    }

    void DrawBox()
    {
        float Offset = 0.001f;

        //Fix indicies if any part is lower than the other
        if (Origin[0] < End[0])
        {
            uint temp = End[0];
            End[0] = Origin[0];
            Origin[0] = End[0];
        }
        if (Origin[1] < End[1])
        {
            uint temp = End[1];
            End[1] = Origin[1];
            Origin[1] = End[1];
        }
        if (Origin[2] < End[2])
        {
            uint temp = End[2];
            End[2] = Origin[2];
            Origin[2] = End[2];
        }

        Verticies = new Vector3[24];

        Triangles = new int[72];
        Vector3 holder;
        //Build Outisde Sides (for loop only used to Organize)
        for (int i = 0, U = 0, T = 0, j = 0, k = 0; i < 1; i++)
        {
            //Build Zneg
            holder = new Vector3(i, j, k) * Scale;

            Verticies[T] = holder + new Vector3(Origin[0], Origin[1], Origin[2]) * Scale - (Vector3.one * Offset);
            Verticies[T + 1] = holder + (Vector3.up + new Vector3(Origin[0], End[1], Origin[2])) * Scale + (-Vector3.right + Vector3.up - Vector3.forward) * Offset;
            Verticies[T + 2] = holder + (Vector3.right + new Vector3(End[0], Origin[1], Origin[2])) * Scale + (Vector3.right - Vector3.up - Vector3.forward) * Offset;
            Verticies[T + 3] = holder + (Vector3.right + Vector3.up + new Vector3(End[0], End[1], Origin[2])) * Scale + (Vector3.right + Vector3.up - Vector3.forward) * Offset;

            Triangles[U + 2] = T;
            Triangles[U + 1] = T + 2;
            Triangles[U] = T + 1;

            Triangles[U + 5] = T + 3;
            Triangles[U + 4] = T + 1;
            Triangles[U + 3] = T + 2;
            U += 6;
            T += 4;
            //Build Zpos
            holder = new Vector3(i, j, k + 1) * Scale;

            Verticies[T] = holder + new Vector3(Origin[0], Origin[1], End[2]) * Scale + (-Vector3.right - Vector3.up + Vector3.forward) * Offset;
            Verticies[T + 1] = holder + (Vector3.up + new Vector3(Origin[0], End[1], End[2])) * Scale + (-Vector3.right + Vector3.up + Vector3.forward) * Offset;
            Verticies[T + 2] = holder + (Vector3.right + new Vector3(End[0], Origin[1], End[2])) * Scale + (Vector3.right - Vector3.up + Vector3.forward) * Offset;
            Verticies[T + 3] = holder + (Vector3.right + Vector3.up + new Vector3(End[0], End[1], End[2])) * Scale + (Vector3.right + Vector3.up + Vector3.forward) * Offset;

            Triangles[U] = T;
            Triangles[U + 1] = T + 2;
            Triangles[U + 2] = T + 1;

            Triangles[U + 3] = T + 3;
            Triangles[U + 4] = T + 1;
            Triangles[U + 5] = T + 2;
            U += 6;
            T += 4;
            //Build Yneg
            holder = new Vector3(i, j, k) * Scale;
            Verticies[T] = holder + new Vector3(Origin[0], Origin[1], Origin[2]) *Scale + (-Vector3.right - Vector3.up - Vector3.forward) * Offset;
            Verticies[T + 1] = holder + (Vector3.forward + new Vector3(Origin[0], Origin[1], End[2])) * Scale + (-Vector3.right - Vector3.up + Vector3.forward) * Offset;
            Verticies[T + 2] = holder + (Vector3.right + new Vector3(End[0], Origin[1], Origin[2])) * Scale + (Vector3.right - Vector3.up - Vector3.forward) * Offset;
            Verticies[T + 3] = holder + (Vector3.right + Vector3.forward + new Vector3(End[0], Origin[1], End[2])) * Scale + (Vector3.right - Vector3.up + Vector3.forward) * Offset;

            Triangles[U + 2] = T;
            Triangles[U + 1] = T + 1;
            Triangles[U + 0] = T + 2;

            Triangles[U + 5] = T + 1;
            Triangles[U + 4] = T + 3;
            Triangles[U + 3] = T + 2;

            U = U + 6;
            T += 4;
            //Build Ypos
            holder = new Vector3(i, j + 1, k) * Scale;
            Verticies[T] = holder + new Vector3(Origin[0], End[1], Origin[2]) *Scale + (-Vector3.right + Vector3.up - Vector3.forward) * Offset;
            Verticies[T + 1] = holder + (Vector3.forward + new Vector3(Origin[0], End[1], End[2])) * Scale + (-Vector3.right + Vector3.up + Vector3.forward) * Offset;
            Verticies[T + 2] = holder + (Vector3.right + new Vector3(End[0], End[1], Origin[2])) * Scale + (Vector3.right + Vector3.up - Vector3.forward) * Offset;
            Verticies[T + 3] = holder + (Vector3.right + Vector3.forward + new Vector3(End[0], End[1], End[2])) * Scale + (Vector3.right + Vector3.up + Vector3.forward) * Offset;

            Triangles[U] = T;
            Triangles[U + 1] = T + 1;
            Triangles[U + 2] = T + 2;

            Triangles[U + 3] = T + 1;
            Triangles[U + 4] = T + 3;
            Triangles[U + 5] = T + 2;

            U = U + 6;
            T += 4;
            //Build Xneg
            holder = new Vector3(i, j, k) * Scale;
            Verticies[T] = holder + new Vector3(Origin[0], Origin[1], Origin[2]) *Scale + (-Vector3.right - Vector3.up - Vector3.forward) * Offset;
            Verticies[T + 1] = holder + (Vector3.forward + new Vector3(Origin[0], Origin[1], End[2])) * Scale + (-Vector3.right - Vector3.up + Vector3.forward) * Offset;
            Verticies[T + 2] = holder + (Vector3.up + new Vector3(Origin[0], End[1], Origin[2])) * Scale + (-Vector3.right + Vector3.up - Vector3.forward) * Offset;
            Verticies[T + 3] = holder + (Vector3.forward + Vector3.up + new Vector3(Origin[0], End[1], End[2])) * Scale + (-Vector3.right + Vector3.up + Vector3.forward) * Offset;

            Triangles[U] = T;
            Triangles[U + 1] = T + 1;
            Triangles[U + 2] = T + 2;

            Triangles[U + 3] = T + 1;
            Triangles[U + 4] = T + 3;
            Triangles[U + 5] = T + 2;

            U = U + 6;
            T += 4;
            //Build Xpos
            holder = new Vector3(i + 1, j, k) * Scale;
            Verticies[T] = holder + new Vector3(End[0], Origin[1], Origin[2]) *Scale + (Vector3.right - Vector3.up - Vector3.forward) * Offset;
            Verticies[T + 1] = holder + (Vector3.forward + new Vector3(End[0], Origin[1], End[2])) * Scale + (Vector3.right - Vector3.up + Vector3.forward) * Offset;
            Verticies[T + 2] = holder + (Vector3.up + new Vector3(End[0], End[1], Origin[2])) * Scale + (Vector3.right + Vector3.up - Vector3.forward) * Offset;
            Verticies[T + 3] = holder + (Vector3.forward + Vector3.up + new Vector3(End[0], End[1], End[2])) * Scale + (Vector3.right + Vector3.up + Vector3.forward) * Offset;

            Triangles[U + 2] = T;
            Triangles[U + 1] = T + 1;
            Triangles[U] = T + 2;

            Triangles[U + 5] = T + 1;
            Triangles[U + 4] = T + 3;
            Triangles[U + 3] = T + 2;

            U = U + 6;
            T += 4;
        }
        //Build Inside sides (Create more triangles inverted from original set)
        for (int i = 36; i < 72; i += 3)
        {
            Triangles[i] = Triangles[(i + 1) - 36];
            Triangles[i + 1] = Triangles[i - 36];
            Triangles[i + 2] = Triangles[(i + 2) - 36];
        }
        mesh.Clear();

        mesh.vertices = Verticies;
        mesh.triangles = Triangles;
    }

    void Updatemesh()
    {
        mesh.Clear();

        mesh.vertices = Verticies;
        mesh.triangles = Triangles;
    }
}
