using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
public class MsgListener_spoon : MonoBehaviour
{



    float[] lastRot = { 0, 0, 0 }; //Need the last rotation to tell how far to spin the camera
    
    [SerializeField]
    public float ori_x, ori_y, ori_z, acc_x, acc_y, acc_z;
    [SerializeField]
    public int button;
    
    [Header("<Reference>")]
    //각각 b_r_thumb/ b_l_thumb3 넣는다.
    public GameObject rightHand;
    public GameObject leftHand;
    private GameObject hand;
    //private OVRSkeleton skeleton;
    // Start is called before the first frame update
    //private void Start()
    //{
    //    skeleton = GetComponent<OVRSkeleton>();
    //}
    void OnMessageArrived(string msg)
    {

        //Debug.Log(msg);
        string[] vec3 = msg.Split(','); //My arduino script returns a 3 part value (IE: 12,30,18)

        if (vec3[0] == "1" && vec3[1] != "" && vec3[2] != "" && vec3[3] != "" && vec3[4] != "") //Check if all values are recieved
        {

            ori_x = float.Parse(vec3[3]); //VR상에서의 roll
            //동서남북 방향에 따라서 값 달라짐
            ori_y = float.Parse(vec3[1]) + SettingController.direction;
            ori_z = float.Parse(vec3[2]); //VR상에서의 pitch
            button = int.Parse(vec3[4]);
            acc_x = float.Parse(vec3[5]);
            acc_y = float.Parse(vec3[6]);
            acc_z = float.Parse(vec3[7]);

            //yaw >> roll
            //pitch >> yaw
            //roll >> pitch
            //transform.localEulerAngles = new Vector3(float.Parse(vec3[2]), float.Parse(vec3[0]), float.Parse(vec3[1]));
            transform.localEulerAngles = new Vector3(ori_x, ori_y, ori_z);
            //transform.Rotate(           //Rotate the camera based on the new values
            //                    z,
            //                    x,
            //                    y,
            //                    Space.Self
            //                );

            //로그 표시 부분 
            //Debug.Log("Angles: " + x.ToString() + ", " + y.ToString() + ", " + z.ToString());

            if(button == 1)
            {
                if(SettingController.WhichHand == SettingController.TYPE.RIGHTHAND)
                {
                    hand = rightHand;
                }
                else if(SettingController.WhichHand == SettingController.TYPE.LEFTHAND)
                {
                    hand = leftHand;
                }
                var spoonPosition = hand.transform.position;
                //var spoonPosition = FingerPosTraker.ThumbTipTransform.position;
                transform.position = spoonPosition;
                
            }
        }
    }

    // Update is called once per frame
    void OnConnectionEvent(bool success)
    {
        if (success)
        {
            Debug.Log("Success");
        }
    }
}
