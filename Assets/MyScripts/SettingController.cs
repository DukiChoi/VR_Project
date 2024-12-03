using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingController : MonoBehaviour
{

    public enum TYPE
    {
        RIGHTHAND = 0,
        LEFTHAND = 1
    }

    public enum DIRECTION_TYPE
    {
        HOME = -70,
        SCHOOL = 175
    }

    [Header("<<Please Select Which Hand To Use>>")]
    public TYPE HandToUse;
    public static TYPE WhichHand;

    [Header("<<Which direction are you heading at>>")]
    [Tooltip("East:0, South:-90, North: 90, West: 180(Áý:-70,ÇÐ±³: 175)")]
    public DIRECTION_TYPE FrontDirection;
    public static int direction;


    // Start is called before the first frame update
    private void Start()
    {
        WhichHand = HandToUse;
        direction = (int)FrontDirection;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
