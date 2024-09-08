using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraChecker : MonoBehaviour
{
    private Transform parent;
    [Header("Reference")]
    public new Camera camera;
    public Transform target;
    public TextMeshPro tmpT;
    //[Header("Settings")]
    //private float screenBorder = 10;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(target.transform.position);
        bool onScreen = screenPoint.z > 0f && screenPoint.z < 1f && screenPoint.x > 0f && screenPoint.x < 1f && screenPoint.y > 0f && screenPoint.y < 1f;       
        if(onScreen){
            tmpT.text = "Drawing";
            tmpT.color = Color.red;
        }
        else
        {
            tmpT.text = "Not Drawing";
        }
    }
}
