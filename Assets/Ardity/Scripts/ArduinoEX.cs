using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
public class ArduinoEX : MonoBehaviour
{

    float[] lastRot = { 0, 0, 0 }; //Need the last rotation to tell how far to spin the camera
    string msg;
    float x, y, z;
    SerialPort m_SerialPort = new SerialPort("/dev/cu.usbmodem1201", 115200, Parity.None, 8, StopBits.One);

    void Start()
    {
        m_SerialPort.Open();
        InvokeRepeating("Readline", 0.0f, 1f);
    }

    void Readline()
    {

        try
        {
            if (m_SerialPort.IsOpen)
            {
                msg = m_SerialPort.ReadLine();
                m_SerialPort.ReadTimeout = 100;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        //if (msg != null)
        //{
        //    Debug.Log(msg);
        //    string[] vec3 = msg.Split(','); //My arduino script returns a 3 part value (IE: 12,30,18)
        //    if (vec3[0] != "" && vec3[1] != "" && vec3[2] != "") //Check if all values are recieved
        //    {

        //        x = float.Parse(vec3[0]) - lastRot[0];
        //        y = float.Parse(vec3[1]) - lastRot[1];
        //        z = float.Parse(vec3[2]) - lastRot[2];


        //        //yaw >> roll
        //        //pitch >> yaw
        //        //roll >> pitch
        //        transform.localEulerAngles = new Vector3(float.Parse(vec3[2]), float.Parse(vec3[0]), float.Parse(vec3[1]));
        //        //transform.Rotate(z,x,y,Space.Self);           //Rotate the camera based on the new values

        //        Debug.Log("Angles: " + x.ToString() + ", " + y.ToString() + ", " + z.ToString());
        //        lastRot[0] = float.Parse(vec3[0]);  //Set new values to last time values for the next loop
        //        lastRot[1] = float.Parse(vec3[1]);
        //        lastRot[2] = float.Parse(vec3[2]);
        //    }
        //}
    }

    void OnApplicationQuit()
    {
        m_SerialPort.Close();
    }

}