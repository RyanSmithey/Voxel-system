
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public byte[] selected;

    string filepattern = "sprites/{0:000}";

    //Texture2d tex;



    public Sprite samplepicture;

    public GameObject TestObj;

    //Useful for finding the desired locations of sprites (Screen.height/10)
    //width = Screen.width;
    //height = Screen.height;

    void Start()
    {
        string filePattern = "Materials/Ambient/{0:000}";
        string filename = string.Format(filePattern, 2);


        Texture2D tex = Resources.Load<Texture2D>(filename);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 100, 100), new Vector2(50, 50), 1f, 0);
        //This takes 5ms which is fairly long but workable

        TestObj.name = "TestObj";
        TestObj.AddComponent<CanvasRenderer>();
        TestObj.AddComponent<Image>();
        TestObj.GetComponent<Image>().sprite = sprite;
    }
    //void start()
    //{
    //    tex = new texture2d[10];
    //    selected = new byte[10];

    //    for (byte i = 1; i < 11; i++) { selected[i] = i; }

    //    for (byte i = 0; i < 10; i++)
    //    {
    //        string filename = string.format(filepattern, selected[i]);
    //        tex[i] = resources.load<texture2d>(filename);

    //    }

    //}

    //void Start()
    //{
    //    GameObject X = new GameObject("MyGO", typeof(RectTransform));
    //}
}
