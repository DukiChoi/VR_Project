using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Xml.Schema;
using Meta.WitAi.Events;
public class ScoreForSpoonGame : MonoBehaviour
{
    public TMP_Text tmpT;
    private GameObject[] balls;
    public static int bluebowl, redbowl, outside;
    public int ball_max_num = 128;
    private GameObject BlueBowl;
    private GameObject RedBowl;
    private int total;
    // Start is called before the first frame update
    void Start()
    {
        balls = GameObject.FindGameObjectsWithTag("Tomato");
        BlueBowl = GameObject.Find("BlueBowl");
        RedBowl = GameObject.Find("RedBowl");
    }
    // Update is called once per frame
    void Update()
    {
        bluebowl = 0;
        redbowl = 0;
        outside = 0;
        for(int i = 0; i < balls.Length; i++) {
            
            if (Vector3.Distance(balls[i].transform.position, BlueBowl.transform.position) <= .14f)
            {
                bluebowl += 1;
            }
            else if (Vector3.Distance(balls[i].transform.position, RedBowl.transform.position) <= .14f)
            {
                redbowl += 1;
            }
            else
            {
                outside += 1;
            }
        }
        tmpT.text = "Your Score: " + redbowl +"\r\n      Out: " + outside;
        
        //��Ż�� �޶����� ��Ż �������ְ� �α� ���.
        if (total != redbowl + outside && redbowl < 10)
        {
            total = redbowl + outside;
            Debug.Log("Score: " + redbowl + "  Out: " + outside + "  Total: " + total + "\r\n");
        }
        //score �� 10�� �̻��� �Ǹ� ǥ��
        else if (redbowl >= 10)
        {

            Debug.Log("CUT\n");
        }
        
    }
}

public static class Extensions
{
    public static T[] Append<T>(this T[] array, T item)
    {
        if (array == null)
        {
            return new T[] { item };
        }
        T[] result = new T[array.Length + 1];
        array.CopyTo(result, 0);
        result[array.Length] = item;
        return result;
    }
}
