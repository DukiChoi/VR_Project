using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.CameraUI;
using static UnityEngine.GraphicsBuffer;
using TMPro;
using Unity.VisualScripting;
using Meta.WitAi.Events;
using System.Diagnostics;

public class CSVManager : MonoBehaviour
{

    static string init_time = DateTime.Now.ToString("yy-MM-dd_HH-mm-ss");
    string fileName = init_time + ".csv";
    //List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();
    bool IsInit = true;
    StreamWriter outStream;
    StringBuilder sb;
    //StringBuilder sb1 = new StringBuilder();
    //StringBuilder sb2 = new StringBuilder();
    List<string[]> data = new List<string[]>();
    string[] tempData;
    string filepath;
    private GameObject gameObj;
    private GameObject spoon;
    private GameObject marker;
    private bool onScreen;
    private Vector3 screenPoint;
    private int game1_count;
    private int game2_count;
    [Header("<Reference>")]
    //각각 Draw Camera와 MarkerTip 그리고 CameraCheckingText 넣는다.
   
    public new Camera camera;
    public GameObject markertip;
    public TextMeshPro CameraText;
    public TextMeshPro game1Text;
    public TextMeshPro game2Text;
    public TextMeshPro game1ScoreText;
    public RenderTexture renderTexture1;
    private Stopwatch watch;
    private int drawing_check = 0;
    public enum CURRENT_KEY
    {
        None = 0,
        F1 = 1,
        F2 = 2,
        F3 = 3
    }
    public CURRENT_KEY currentkey = CURRENT_KEY.None;

    void Awake()
    {


        spoon = GameObject.Find("Table Spoon Object");
        marker = GameObject.Find("Marker Red Object");
        //spoon = GameObject.FindGameObjectWithTag("Spoon");
        //marker = GameObject.FindGameObjectWithTag("Marker");
        
        ////데이터 변수 초기화
        //data.Clear();
        ////데이터 가져오는 과정
        //tempData = new string[11];
        //tempData[0] = "Time";
        //tempData[1] = "ms";
        //tempData[2] = "Acc_x";
        //tempData[3] = "Acc_y";
        //tempData[4] = "Acc_z";
        //tempData[5] = "Ori_x";
        //tempData[6] = "Ori_y";
        //tempData[7] = "Ori_z";
        //tempData[8] = "Loc_x";
        //tempData[9] = "Loc_y";
        //tempData[10] = "Loc_z";
        //data.Add(tempData);

        //파일경로 지정
        filepath = SystemPath.GetPath();

        if (!Directory.Exists(filepath))
        {
            Directory.CreateDirectory(filepath);
        }

        //파일경로의 csv파일 열기
        //outStream = System.IO.File.CreateText(filepath + fileName);
        //Debug.Log("CSV will be saved on:" + filepath + fileName);

        //// 맨 앞줄 저장
        //string[][] output = new string[data.Count][];

        //for (int i = 0; i < output.Length; i++)
        //{
        //    output[i] = data[i];
        //}
        //int length = output.GetLength(0);
        //string delimiter = ",";
        //sb.AppendLine(string.Join(delimiter, output[length - 1]));
        ////    
    }

    private void Update()
    {

        //F1:: 첫번째 게임 시작
        if (Input.GetKeyDown(KeyCode.F1) && currentkey != CURRENT_KEY.F1)
        {
            if (!IsInit)
            {
                outStream.Write(sb);
                outStream.Close();
                watch.Stop();
                UnityEngine.Debug.Log("The game ends");
                UnityEngine.Debug.Log("CSV Saved completely on: " + filepath + fileName + ".csv");
            }

            watch = new Stopwatch();
            watch.Start();
            IsInit = false;
            //데이터 변수 초기화
            data.Clear();
            //데이터 목차
            tempData = new string[15];
            tempData[0] = "Time";
            tempData[1] = "ms";
            tempData[2] = "Acc_x";
            tempData[3] = "Acc_y";
            tempData[4] = "Acc_z";
            tempData[5] = "Ori_x";
            tempData[6] = "Ori_y";
            tempData[7] = "Ori_z";
            tempData[8] = "Spoon_loc_x";
            tempData[9] = "Spoon_loc_y";
            tempData[10] = "Spoon_loc_z";
            tempData[11] = "Score";
            tempData[12] = "Out";
            tempData[13] = "Trials";
            tempData[14] = "Remains";


            data.Add(tempData);
            gameObj = spoon;
            currentkey = CURRENT_KEY.F1;
            fileName = init_time + "_spoon" + game1_count.ToString() + "_level" + MovingDishes.whichlevel;
            game1_count++;
            outStream = System.IO.File.CreateText(filepath + fileName + ".csv");
            sb = new StringBuilder();
            UnityEngine.Debug.Log("Spoon game starts");
            UnityEngine.Debug.Log("CSV will be saved on:" + filepath + fileName + ".csv");
            //제목1 색깔 바꾸기 D4FF00노란색 >> FF5C25진주황색
            game1Text.color = new Color32(0xFF, 0x5C, 0x25, 0xFF);
            //제목2 색깔 바꾸기 FF5C25진주황색 >> D4FF00노란색
            game2Text.color = new Color32(0xD4, 0xFF, 0x00, 0xFF);
            //점수표 색깔 연두(활성화)로 바꾸기  
            game1ScoreText.color = new Color32(0x00, 0xFF, 0x76, 0xFF);
            //데이터 쓰기
            string[][] output = new string[data.Count][];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = data[i];
            }
            int length = output.GetLength(0);
            string delimiter = ",";
            sb.AppendLine(string.Join(delimiter, output[length - 1]));

        }
        //F2:: 두번째 게임 시작
        else if (Input.GetKeyDown(KeyCode.F2) && currentkey != CURRENT_KEY.F2)
        {
            if (!IsInit)
            {
                outStream.Write(sb);
                outStream.Close();
                watch.Stop();
                UnityEngine.Debug.Log("The game ends");
                UnityEngine.Debug.Log("CSV Saved completely on: " + filepath + fileName + ".csv");
            }

            watch = new Stopwatch();
            watch.Start();
            IsInit = false;
            //데이터 변수 초기화
            data.Clear();
            //데이터 목차
            tempData = new string[15];
            tempData[0] = "Time";
            tempData[1] = "ms";
            tempData[2] = "Acc_x";
            tempData[3] = "Acc_y";
            tempData[4] = "Acc_z";
            tempData[5] = "Ori_x";
            tempData[6] = "Ori_y";
            tempData[7] = "Ori_z";
            tempData[8] = "Marker_loc_x";
            tempData[9] = "Marker_loc_y";
            tempData[10] = "Marker_loc_z";
            tempData[11] = "Tip_loc_x";
            tempData[12] = "Tip_loc_y";
            tempData[13] = "Tip_loc_z";
            tempData[14] = "Drawing_check";
            data.Add(tempData);

            gameObj = marker;
            currentkey = CURRENT_KEY.F2;
            fileName = init_time + "_drawing" + game2_count.ToString();
            game2_count++;
            outStream = System.IO.File.CreateText(filepath + fileName + ".csv");
            sb = new StringBuilder();
            UnityEngine.Debug.Log("Drawing game starts");
            UnityEngine.Debug.Log("CSV will be saved on:" + filepath + fileName + ".csv");

            //제목1 색깔 바꾸기  FF5C25진주황색 >> D4FF00노란색
            game1Text.color = new Color32(0xD4, 0xFF, 0x00, 0xFF);
            //제목2 색깔 바꾸기   D4FF00노란색 >> FF5C25진주황색
            game2Text.color = new Color32(0xFF, 0x5C, 0x25, 0xFF);
            //점수표 색깔 원래대로(보라) 바꾸기
            game1ScoreText.color = new Color32(0x67, 0x00, 0xFF, 0xFF);

            //데이터 쓰기
            string[][] output = new string[data.Count][];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = data[i];
            }
            int length = output.GetLength(0);
            string delimiter = ",";
            sb.AppendLine(string.Join(delimiter, output[length - 1]));
        }

        //F3:: 게임 끝내기
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            ClearOutRenderTexture(renderTexture1);
            if (currentkey != CURRENT_KEY.F3)
            {
                if (!IsInit)
                {
                    outStream.Write(sb);
                    outStream.Close();
                    watch.Stop();
                    UnityEngine.Debug.Log("The game ends");
                    UnityEngine.Debug.Log("CSV Saved completely on: " + filepath + fileName + ".csv");
                    //제목 색깔 원래대로 바꾸기
                    game1Text.color = new Color32(0xD4, 0xFF, 0x00, 0xFF);
                    game2Text.color = new Color32(0xD4, 0xFF, 0x00, 0xFF);
                    //점수표 색깔 원래대로(보라) 바꾸기
                    game1ScoreText.color = new Color32(0x67, 0x00, 0xFF, 0xFF);
                }
                IsInit = true;
                currentkey = CURRENT_KEY.F3;
                UnityEngine.Debug.Log("Having a breaktime");
                
            }
        } 
        /*
         * 여기서부터는 현재 키가 F1, F2일 때 엑셀에 저장되게끔 하는 코드.
         * 원래는 CURRENT_KEY.none이 아니면 다 저장되게끔 했으나 F3 추가해주면서 로직 바꿈.
         */
        if (currentkey ==CURRENT_KEY.F1)
            WriteLinesToCSV();
        //두번째 게임에서는 반드시 그릴 때에만 값을 저장해준다.
        else if (currentkey == CURRENT_KEY.F2)
        {
            screenPoint = camera.WorldToViewportPoint(markertip.transform.position);
            onScreen = screenPoint.z > 0f && screenPoint.z < 1.05f && screenPoint.x > 0f && screenPoint.x < 1f && screenPoint.y > 0f && screenPoint.y < 1f;
            WriteLinesToCSV();
            if (onScreen)
            {
                CameraText.text = "Drawing";
                drawing_check = 1;
                CameraText.color = new Color32(0x00, 0xFF, 0x76, 0xFF);
            }
            else
            {
                CameraText.text = "Not Drawing";
                drawing_check = 0;
                CameraText.color = new Color32(0x55, 0x00, 0xFF, 0xFF);
            }
        }
    }

    private void OnDestroy()
    {
        outStream.Write(sb);
        outStream.Close();
        UnityEngine.Debug.Log("CSV Saved completely on: " + filepath + fileName + ".csv");

    }

    public void WriteLinesToCSV()
    {
        //데이터 가져오는 과정

        tempData = new string[15];
        tempData[0] = DateTime.Now.ToString("HH:mm:ss.fff");
        //tempData[0] = DateTime.Now.ToString("MM-dd HH:mm:ss");
        tempData[1] = watch.ElapsedMilliseconds.ToString();
        if (currentkey == CURRENT_KEY.F1)
        {
            tempData[2] = gameObj.GetComponent<MsgListener_spoon>().acc_x.ToString();
            tempData[3] = gameObj.GetComponent<MsgListener_spoon>().acc_y.ToString();
            tempData[4] = gameObj.GetComponent<MsgListener_spoon>().acc_z.ToString();
            tempData[5] = gameObj.GetComponent<MsgListener_spoon>().ori_x.ToString();
            tempData[6] = gameObj.GetComponent<MsgListener_spoon>().ori_y.ToString();
            tempData[7] = gameObj.GetComponent<MsgListener_spoon>().ori_z.ToString();
            tempData[8] = gameObj.transform.position.x.ToString();
            tempData[9] = gameObj.transform.position.y.ToString();
            tempData[10] = gameObj.transform.position.z.ToString();
            tempData[11] = ScoreForSpoonGame.redbowl.ToString();
            tempData[12] = ScoreForSpoonGame.outside.ToString();
            tempData[13] = (ScoreForSpoonGame.redbowl + ScoreForSpoonGame.outside).ToString();
            tempData[14] = ScoreForSpoonGame.bluebowl.ToString();
        }
        else if (currentkey == CURRENT_KEY.F2)
        {
            tempData[2] = gameObj.GetComponent<MsgListener_marker>().acc_x.ToString();
            tempData[3] = gameObj.GetComponent<MsgListener_marker>().acc_y.ToString();
            tempData[4] = gameObj.GetComponent<MsgListener_marker>().acc_z.ToString();
            tempData[5] = gameObj.GetComponent<MsgListener_marker>().ori_x.ToString();
            tempData[6] = gameObj.GetComponent<MsgListener_marker>().ori_y.ToString();
            tempData[7] = gameObj.GetComponent<MsgListener_marker>().ori_z.ToString();
            //이건 마커의 위치
            tempData[8] = gameObj.transform.position.x.ToString();
            tempData[9] = gameObj.transform.position.y.ToString();
            tempData[10] = gameObj.transform.position.z.ToString();
            //얘는 마커 팁의 위치
            tempData[11] = markertip.transform.position.x.ToString();
            tempData[12] = markertip.transform.position.y.ToString();
            tempData[13] = markertip.transform.position.z.ToString();
            tempData[14] = drawing_check.ToString();
        }


        data.Add(tempData);

        //데이터 쓰기
        string[][] output = new string[data.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = data[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";
        sb.AppendLine(string.Join(delimiter, output[length - 1]));
        //for (int i = 0; i < length; i++)
        //{
        //    sb.AppendLine(string.Join(delimiter, output[i]));
        //}

    }
    public void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }

}
