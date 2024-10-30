using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SizingCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    public static int whichlevel2;
    public GameObject canvas;
    public TextMeshPro levelText2;
    private Vector3 size_default;
    private float cam_size_defualt;
    public Camera m_OrthographicCamera;

    public enum CURRENT_KEY2
    {
        None = 0,
        n6 = 6,
        n7 = 7,
        n8 = 8,
        n9 = 9,
        n0 = 10
    }

    CURRENT_KEY2 currentkey = CURRENT_KEY2.None;
    void Start()
    {
        canvas = GameObject.Find("Draw Canvas");
        size_default = canvas.transform.localScale;
        cam_size_defualt = m_OrthographicCamera.orthographicSize;
        Debug.Log("Default size saved");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6) && currentkey != CURRENT_KEY2.n6)
        {
            currentkey = CURRENT_KEY2.n6;
            canvas.transform.localScale = size_default;
            m_OrthographicCamera.orthographicSize = cam_size_defualt;
            levelText2.text = "<Spiral Quest>\r\n        Level 1";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && currentkey != CURRENT_KEY2.n7)
        {
            currentkey = CURRENT_KEY2.n7;
            canvas.transform.localScale = size_default;
            m_OrthographicCamera.orthographicSize = cam_size_defualt;
            levelText2.text = "<Spiral Quest>\r\n        Level 2";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) && currentkey != CURRENT_KEY2.n8)
        {
            currentkey = CURRENT_KEY2.n8;
            canvas.transform.localScale = size_default;
            m_OrthographicCamera.orthographicSize = cam_size_defualt;
            levelText2.text = "<Spiral Quest>\r\n        Level 3";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) && currentkey != CURRENT_KEY2.n9)
        {
            currentkey = CURRENT_KEY2.n9;
            canvas.transform.localScale = size_default;
            m_OrthographicCamera.orthographicSize = cam_size_defualt;
            levelText2.text = "<Spiral Quest>\r\n        Level 4";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0) && currentkey != CURRENT_KEY2.n0)
        {
            currentkey = CURRENT_KEY2.n0;
            canvas.transform.localScale = size_default;
            m_OrthographicCamera.orthographicSize = cam_size_defualt;
            levelText2.text = "<Spiral Quest>\r\n        Level 5";
        }
    }
}
