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
using static TreeEditor.TextureAtlas;

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
    private String figure_name = "";
    ChangingCanvas.CURRENT_KEY2 key2;
    private GameObject CanvasObj;
    private RenderTexture renderTexture_canvas; // 저장할 RenderTexture를 Unity 에디터에서 설정
    //TextureCreationFlags flags;
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
        CanvasObj = GameObject.Find("Draw Canvas");
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
        filepath = SystemPath.GetPath() + "/experiment/";

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
        if (Input.GetKeyDown(KeyCode.F1) && currentkey != CURRENT_KEY.F1 && currentkey != CURRENT_KEY.F2)
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
        else if (Input.GetKeyDown(KeyCode.F2) && currentkey != CURRENT_KEY.F2 && currentkey != CURRENT_KEY.F1)
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
            fileName = init_time + "_drawing" + game2_count.ToString() + "_level" + ChangingCanvas.whichlevel2;
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
            //그림 바꾸기
            key2 = CanvasObj.GetComponent<ChangingCanvas>().currentkey;
            UnityEngine.Debug.Log("찾았습니다.");
            if (key2 == ChangingCanvas.CURRENT_KEY2.n8)
            {
                figure_name = "Figure1";
            }
            else if (key2 == ChangingCanvas.CURRENT_KEY2.n9)
            {
                figure_name = "Figure2";
            }
            else if (key2 == ChangingCanvas.CURRENT_KEY2.n0)
            {
                figure_name = "Figure3";
            }
            else if (key2 == ChangingCanvas.CURRENT_KEY2.None)
            {
                UnityEngine.Debug.Log("Level is not Set");
                return;
            }

            if (key2 != ChangingCanvas.CURRENT_KEY2.None)
            {
                Texture tex = Resources.Load(figure_name, typeof(Texture)) as Texture;
                Renderer renderer = CanvasObj.GetComponent<Renderer>();
                renderer.material.SetTexture("_Image", tex);
                UnityEngine.Debug.Log(figure_name + "으로 바꿉니다.");
            }


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
                    //두번째 겜에서만 사진 png로 저장하기!!!!
                    if (currentkey == CURRENT_KEY.F2)
                    {
                        SaveRenderTextureToFile();
                    }
                }
                IsInit = true;
                currentkey = CURRENT_KEY.F3;
                UnityEngine.Debug.Log("Having a breaktime");

            }
            ClearOutRenderTexture(renderTexture_canvas);
        }
        /*
         * 여기서부터는 현재 키가 F1, F2일 때 엑셀에 저장되게끔 하는 코드.
         * 원래는 CURRENT_KEY.none이 아니면 다 저장되게끔 했으나 F3 추가해주면서 로직 바꿈.
         */
        if (currentkey == CURRENT_KEY.F1)
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
        //if (renderTexture == null)
        //{
        //    UnityEngine.Debug.LogWarning("RenderTexture가 null입니다.");
        //    return;
        //}

        //이거 해야 처음에 지워지나봄??
        renderTexture_canvas = camera.targetTexture;
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;

        renderTexture.Release();
        renderTexture.Create(); // RenderTexture를 다시 초기화
    }

    private void SaveRenderTextureToFile()
    {

        if (camera == null)
        {
            UnityEngine.Debug.LogWarning("카메라가 설정되지 않았습니다.");
            return;
        }

        renderTexture_canvas = camera.targetTexture;
        if (renderTexture_canvas == null)
        {
            UnityEngine.Debug.LogWarning("카메라의 RenderTexture가 설정되지 않았습니다.");
            return;
        }

        RenderTexture.active = renderTexture_canvas;
        camera.Render(); // 명시적으로 카메라 렌더링

        //if (renderTexture_canvas == null)
        //{
        //    UnityEngine.Debug.LogWarning("RenderTexture가 지정되지 않았습니다.");
        //    return;
        //}

        //R8G8B8A8_UNorm
        //var texture = new Texture2D(128, 128, GraphicsFormat.R8G8B8A8_SRGB, flags);
        //texture = CanvasObj.GetComponent<Renderer>().material.mainTexture as Texture2D;
        //camera.Render();

        //renderTexture_canvas = camera.targetTexture;
        //RenderTexture.active = renderTexture_canvas;

        Texture2D tex = new Texture2D(renderTexture_canvas.width, renderTexture_canvas.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, renderTexture_canvas.width, renderTexture_canvas.height), 0, 0);
        UnityEngine.Debug.Log("Width: " + renderTexture_canvas.width + ", Height: " + renderTexture_canvas.height);
        tex.Apply();
        RenderTexture.active = null;
        tex = ResizeTexture(tex, 800, 800);
        //tex = RotateTexture180(tex);
        //byte[] bytes = tex.EncodeToPNG();

        string background_path = Application.dataPath + "/Resources/" + figure_name + ".png";
        SaveAndBlendTextures(tex, background_path);



    }

    private Texture2D BlendTextures(Texture2D baseTexture, Texture2D overlayTexture)
    {
        if (baseTexture.width != overlayTexture.width || baseTexture.height != overlayTexture.height)
        {
            UnityEngine.Debug.LogError("텍스처 크기가 일치하지 않습니다. 크기를 맞춰주세요.\n" +
                "(" + baseTexture.width + "," + baseTexture.height + ") | (" + overlayTexture.width + "," + overlayTexture.height + ")");
            return null;
        }

        Texture2D result = new Texture2D(baseTexture.width, baseTexture.height, TextureFormat.RGBA32, false);

        // 픽셀 데이터 읽기
        Color[] basePixels = baseTexture.GetPixels();
        Color[] overlayPixels = overlayTexture.GetPixels();
        Color[] resultPixels = new Color[basePixels.Length];

        // 알파 블렌딩
        for (int i = 0; i < basePixels.Length; i++)
        {
            Color baseColor = basePixels[i];
            Color overlayColor = overlayPixels[i];

            // 알파 블렌딩 공식: C = A1 * (1 - A2) + A2 * C2
            float alpha = overlayColor.a;
            resultPixels[i] = Color.Lerp(baseColor, overlayColor, alpha);
        }

        // 결과 픽셀 적용
        result.SetPixels(resultPixels);
        result.Apply();

        return result;
    }

    private void SaveAndBlendTextures(Texture2D renderTextureData, string overlayFilePath)
    {
        // 외부 PNG 파일 로드
        byte[] overlayBytes = System.IO.File.ReadAllBytes(overlayFilePath);
        Texture2D overlayTexture = new Texture2D(2, 2);
        overlayTexture.LoadImage(overlayBytes);

        // 텍스처 합성
        Texture2D blendedTexture = BlendTextures(renderTextureData, overlayTexture);
        blendedTexture = RotateTexture180(blendedTexture);
        // 합성 결과 저장
        byte[] bytes = blendedTexture.EncodeToPNG();
        string image_path = filepath + fileName + ".png";
        System.IO.File.WriteAllBytes(image_path, bytes);
        UnityEngine.Debug.Log("Saved RenderTexture to " + image_path);
    }

    // 180도 회전 메서드
    private Texture2D RotateTexture180(Texture2D originalTex)
    {
        int width = originalTex.width;
        int height = originalTex.height;
        Texture2D rotatedTex = new Texture2D(width, height);

        Color[] originalPixels = originalTex.GetPixels();
        Color[] rotatedPixels = new Color[originalPixels.Length];

        // 픽셀 데이터를 뒤집어서 회전
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                rotatedPixels[(height - y - 1) * width + (width - x - 1)] = originalPixels[y * width + x];
            }
        }

        rotatedTex.SetPixels(rotatedPixels);
        rotatedTex.Apply();

        return rotatedTex;
    }
    private Texture2D ResizeTexture(Texture2D sourceTexture, int targetWidth, int targetHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight, 0);
        rt.filterMode = FilterMode.Bilinear;

        RenderTexture.active = rt;
        Graphics.Blit(sourceTexture, rt);

        Texture2D resizedTexture = new Texture2D(targetWidth, targetHeight, sourceTexture.format, false);
        resizedTexture.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        resizedTexture.Apply();

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);

        return resizedTexture;
    }

}