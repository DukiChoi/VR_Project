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
    //���� Draw Camera�� MarkerTip �׸��� CameraCheckingText �ִ´�.
   
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
        
        ////������ ���� �ʱ�ȭ
        //data.Clear();
        ////������ �������� ����
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

        //���ϰ�� ����
        filepath = SystemPath.GetPath();

        if (!Directory.Exists(filepath))
        {
            Directory.CreateDirectory(filepath);
        }

        //���ϰ���� csv���� ����
        //outStream = System.IO.File.CreateText(filepath + fileName);
        //Debug.Log("CSV will be saved on:" + filepath + fileName);

        //// �� ���� ����
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

        //F1:: ù��° ���� ����
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
            //������ ���� �ʱ�ȭ
            data.Clear();
            //������ ����
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
            //����1 ���� �ٲٱ� D4FF00����� >> FF5C25����Ȳ��
            game1Text.color = new Color32(0xFF, 0x5C, 0x25, 0xFF);
            //����2 ���� �ٲٱ� FF5C25����Ȳ�� >> D4FF00�����
            game2Text.color = new Color32(0xD4, 0xFF, 0x00, 0xFF);
            //����ǥ ���� ����(Ȱ��ȭ)�� �ٲٱ�  
            game1ScoreText.color = new Color32(0x00, 0xFF, 0x76, 0xFF);
            //������ ����
            string[][] output = new string[data.Count][];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = data[i];
            }
            int length = output.GetLength(0);
            string delimiter = ",";
            sb.AppendLine(string.Join(delimiter, output[length - 1]));

        }
        //F2:: �ι�° ���� ����
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
            //������ ���� �ʱ�ȭ
            data.Clear();
            //������ ����
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

            //����1 ���� �ٲٱ�  FF5C25����Ȳ�� >> D4FF00�����
            game1Text.color = new Color32(0xD4, 0xFF, 0x00, 0xFF);
            //����2 ���� �ٲٱ�   D4FF00����� >> FF5C25����Ȳ��
            game2Text.color = new Color32(0xFF, 0x5C, 0x25, 0xFF);
            //����ǥ ���� �������(����) �ٲٱ�
            game1ScoreText.color = new Color32(0x67, 0x00, 0xFF, 0xFF);

            //������ ����
            string[][] output = new string[data.Count][];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = data[i];
            }
            int length = output.GetLength(0);
            string delimiter = ",";
            sb.AppendLine(string.Join(delimiter, output[length - 1]));
        }

        //F3:: ���� ������
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
                    //���� ���� ������� �ٲٱ�
                    game1Text.color = new Color32(0xD4, 0xFF, 0x00, 0xFF);
                    game2Text.color = new Color32(0xD4, 0xFF, 0x00, 0xFF);
                    //����ǥ ���� �������(����) �ٲٱ�
                    game1ScoreText.color = new Color32(0x67, 0x00, 0xFF, 0xFF);
                }
                IsInit = true;
                currentkey = CURRENT_KEY.F3;
                UnityEngine.Debug.Log("Having a breaktime");
                
            }
        } 
        /*
         * ���⼭���ʹ� ���� Ű�� F1, F2�� �� ������ ����ǰԲ� �ϴ� �ڵ�.
         * ������ CURRENT_KEY.none�� �ƴϸ� �� ����ǰԲ� ������ F3 �߰����ָ鼭 ���� �ٲ�.
         */
        if (currentkey ==CURRENT_KEY.F1)
            WriteLinesToCSV();
        //�ι�° ���ӿ����� �ݵ�� �׸� ������ ���� �������ش�.
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
        //������ �������� ����

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
            //�̰� ��Ŀ�� ��ġ
            tempData[8] = gameObj.transform.position.x.ToString();
            tempData[9] = gameObj.transform.position.y.ToString();
            tempData[10] = gameObj.transform.position.z.ToString();
            //��� ��Ŀ ���� ��ġ
            tempData[11] = markertip.transform.position.x.ToString();
            tempData[12] = markertip.transform.position.y.ToString();
            tempData[13] = markertip.transform.position.z.ToString();
            tempData[14] = drawing_check.ToString();
        }


        data.Add(tempData);

        //������ ����
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
