using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CSVManager;



public class MovingDishes : MonoBehaviour
{
    public static int whichlevel;
    public GameObject dish;
    private GameObject[] balls;
    private Vector3[] pos_default_balls;
    private Vector3 pos_default;
    public TextMeshPro levelText;

    public enum CURRENT_KEY
    {
        n1 = 1,
        n2 = 2,
        n3 = 3,
        n4 = 4,
        n5 = 5
    }

    CURRENT_KEY currentkey = CURRENT_KEY.n1;

    // Start is called before the first frame update
    void Start()
    {
        
        dish = GameObject.Find("RedBowl");
        balls = GameObject.FindGameObjectsWithTag("Tomato");
        pos_default = dish.transform.position;
        //이렇게 한번도 안써본 array에는 new로 그냥 임의로 초기화를 해놔야 참조 오류가 안난다.
        pos_default_balls = new Vector3[balls.Length];
        Debug.Log("Length is: " + balls.Length);
        for (int i = 0; i < balls.Length; i++)
        {
            pos_default_balls[i] = balls[i].transform.position;
            Debug.Log(pos_default_balls[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentkey != CURRENT_KEY.n1)
        {
            currentkey = CURRENT_KEY.n1;
            dish.transform.position = pos_default;
            levelText.text = "<Tomato Shuffle>\r\n\r\n        Level 1";
            Debug.Log("Level 1 : postion :" + dish.transform.position);

            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].transform.position = pos_default_balls[i];
            }

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && currentkey != CURRENT_KEY.n2)
        {
            currentkey = CURRENT_KEY.n2;
            Debug.Log(dish.transform.position);
            dish.transform.position = pos_default + new Vector3(0, 0, -0.1f);
            levelText.text = "<Tomato Shuffle>\r\n\r\n        Level 2";
            Debug.Log("Level 2 : postion :" + dish.transform.position);
            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].transform.position = pos_default_balls[i];
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && currentkey != CURRENT_KEY.n3)
        {
            currentkey = CURRENT_KEY.n3;
            Debug.Log(dish.transform.position);
            dish.transform.position = pos_default + new Vector3(0, 0, -0.2f);
            levelText.text = "<Tomato Shuffle>\r\n\r\n        Level 3";
            Debug.Log("Level 3 : postion :" + dish.transform.position);
            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].transform.position = pos_default_balls[i];
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && currentkey != CURRENT_KEY.n4)
        {
            currentkey = CURRENT_KEY.n4;
            Debug.Log(dish.transform.position);
            dish.transform.position = pos_default + new Vector3(0, 0, -0.3f);
            levelText.text = "<Tomato Shuffle>\r\n\r\n        Level 4";
            Debug.Log("Level 4 : postion :" + dish.transform.position);
            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].transform.position = pos_default_balls[i];
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5) && currentkey != CURRENT_KEY.n5) 
        {
            currentkey = CURRENT_KEY.n5;
            Debug.Log(dish.transform.position);
            dish.transform.position = pos_default + new Vector3(0, 0, -0.4f);
            levelText.text = "<Tomato Shuffle>\r\n\r\n        Level 5";
            Debug.Log("Level 5 : postion :" + dish.transform.position);
            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].transform.position = pos_default_balls[i];
            }
        }
        whichlevel = Convert.ToInt32(currentkey);
    }
}
