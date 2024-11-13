using UnityEngine;

public class SaveRenderTextureWithKey : MonoBehaviour
{
    public RenderTexture renderTexture; // ������ RenderTexture�� Unity �����Ϳ��� ����

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SaveRenderTextureToFile();
        }
    }

    private void SaveRenderTextureToFile()
    {
        if (renderTexture == null)
        {
            Debug.LogWarning("RenderTexture�� �������� �ʾҽ��ϴ�.");
            return;
        }

        RenderTexture.active = renderTexture;
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes = tex.EncodeToPNG();
        string path = Application.dataPath + "/SavedRenderTexture.png";
        System.IO.File.WriteAllBytes(path, bytes);

        Debug.Log("Saved RenderTexture to " + path);
    }
}