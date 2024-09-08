using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus;

public class FingerPosTraker : MonoBehaviour
{
    private OVRSkeleton skeleton;
    public static Transform ThumbTipTransform;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        skeleton = GetComponent<OVRSkeleton>();

        while (skeleton.Bones.Count == 0)
        {
            Debug.Log("CANNOT COUNT");
            yield return null;
        }

        foreach (var bone in skeleton.Bones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_Thumb3)
            {
                ThumbTipTransform = bone.Transform;
                Debug.Log("position:" + ThumbTipTransform.position);
            }
            else
            {
                Debug.Log("CANNOT FIND");
            }
        }

        //if (skeleton.Bones.Count != 0)
        //{
        //    foreach (var bone in skeleton.Bones)
        //    {
        //        if (bone.Id == OVRSkeleton.BoneId.Hand_Thumb3)
        //        {
        //            ThumbTipTransform = bone.Transform;
        //        }
        //    }
        //}
    }
}
