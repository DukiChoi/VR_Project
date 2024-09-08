using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positiontracking : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public GameObject head;
    public GameObject handR;
    public GameObject handL;

    void Update()
    {
        var headPosition = head.transform.position;
        var handRPosition = handR.transform.position;
        var handLPosition = handL.transform.position;

        //Debug.Log("head: " + headPosition + "/Right: " + handRPosition + "/Left: " + handLPosition);
    }
}
