using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using static TreeEditor.TextureAtlas;

public class ChangingCanvas : MonoBehaviour
{

    public enum CURRENT_KEY2
    {
        None = 0,
        n8 = 1,
        n9 = 2,
        n0 = 3
    }
    public CURRENT_KEY2 currentkey = CURRENT_KEY2.None;

    //public void SetMaterial(Material newMaterial)
    //{
    //    Renderer renderer = GetComponent<Renderer>();
    //    if (GetComponent<Renderer>() == null)
    //    {
    //        return;
    //    }
    //}

    // Start is called before the first frame update
    public static int whichlevel2 = 0;
    public TextMeshPro levelText2;
    void Start()
    {
    }

    public void SetTexture(string textureName)
    {
        Texture tex = Resources.Load(textureName, typeof(Texture)) as Texture;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.SetTexture("_Image", tex);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8) && currentkey != CURRENT_KEY2.n8)
        {
            whichlevel2 = 1;
            SetTexture("Figure01");
            currentkey = CURRENT_KEY2.n8;
            levelText2.text = "<Spiral Quest>\r\n      Level 1";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) && currentkey != CURRENT_KEY2.n9)
        {
            whichlevel2 = 2;
            SetTexture("Figure02");
            currentkey = CURRENT_KEY2.n9;
            levelText2.text = "<Spiral Quest>\r\n      Level 2";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0) && currentkey != CURRENT_KEY2.n0)
        {
            whichlevel2 = 3;
            SetTexture("Figure03");
            currentkey = CURRENT_KEY2.n0;
            levelText2.text = "<Spiral Quest>\r\n      Level 3";
        }
        whichlevel2 = Convert.ToInt32(currentkey);
    }
}
