using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class HandTrackScript : MonoBehaviour
{
    public enum HandPoses { Ok, Finger, Thumb, OpenHandBack, Fist, NoPose };
    public HandPoses pose = HandPoses.NoPose;
    public Vector3[] pos;
    public GameObject sphereThumb, sphereIndex, sphereWrist;

    private MLHandKeyPose[] _gestures;

    private void Awake()
    {
        MLHands.Start();
        _gestures = new MLHandKeyPose[5];
        _gestures[0] = MLHandKeyPose.Ok;
        _gestures[1] = MLHandKeyPose.Finger;
        _gestures[2] = MLHandKeyPose.OpenHandBack;
        _gestures[3] = MLHandKeyPose.Fist;
        _gestures[4] = MLHandKeyPose.Thumb;
        MLHands.KeyPoseManager.EnableKeyPoses(_gestures, true, false);
        pos = new Vector3[3];
    }
    private void OnDestroy()
    {
        MLHands.Stop();
    }

    private void Update()
    {
        if (GetGesture(MLHands.Left, MLHandKeyPose.Ok))
        {
            pose = HandPoses.Ok;
        }
        else if (GetGesture(MLHands.Left, MLHandKeyPose.Finger))
        {
            pose = HandPoses.Finger;
        }
        else if (GetGesture(MLHands.Left, MLHandKeyPose.OpenHandBack))
        {
            pose = HandPoses.OpenHandBack;
        }
        else if (GetGesture(MLHands.Left, MLHandKeyPose.Fist))
        {
            pose = HandPoses.Fist;
        }
        else if (GetGesture(MLHands.Left, MLHandKeyPose.Thumb))
        {
            pose = HandPoses.Thumb;
        }
        else
        {
            pose = HandPoses.NoPose;
        }

        if (pose != HandPoses.NoPose) ShowPoints();
    }

    private void ShowPoints()
    {
        // Left Hand Thumb tip
        pos[0] = MLHands.Left.Thumb.KeyPoints[0].Position;
        // Left Hand Index finger tip 
        pos[1] = MLHands.Left.Index.KeyPoints[0].Position;
        // Left Hand Wrist 
        pos[2] = MLHands.Left.Wrist.KeyPoints[0].Position;
        sphereThumb.transform.position = pos[0];
        sphereIndex.transform.position = pos[1];
        sphereWrist.transform.position = pos[2];
    }

    private bool GetGesture(MLHand hand, MLHandKeyPose type)
    {
        if (hand != null)
        {
            if (hand.KeyPose == type)
            {
                if (hand.KeyPoseConfidence > 0.9f)
                {
                    return true;
                }
            }
        }
        return false;
    }

}