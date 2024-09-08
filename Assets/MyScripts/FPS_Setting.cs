using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Setting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_IOS || UNITY_ANDROID
        Application.targetFrameRate = 72;
#else
            QualitySettings.vSyncCount = 1;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
