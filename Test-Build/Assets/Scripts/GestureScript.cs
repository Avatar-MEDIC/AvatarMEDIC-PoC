using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class GestureScript : MonoBehaviour
{

    private bool OKHandPose = false; // bool used to check if the activation has happened
    private float speed = 30.0f;  // Speed of our cube
    private float distance = 2.0f; // Distance between Main Camera and Cube
    private GameObject cube; // Reference to our Cube
    private MLHandKeyPose[] gestures; // Array to store different gestures we will look for

    void Awake()
    {
        MLHands.Start();

        gestures = new MLHandKeyPose[4];
        gestures[0] = MLHandKeyPose.Ok;
        gestures[1] = MLHandKeyPose.Fist;
        gestures[2] = MLHandKeyPose.OpenHandBack;
        gestures[3] = MLHandKeyPose.Finger;
        MLHands.KeyPoseManager.EnableKeyPoses(gestures, true, false);

        // Cube is deactivated at first
        cube = GameObject.Find("Cube");
        cube.SetActive(false);
    }

    void OnDestroy()
    {
        MLHands.Stop();
    }

    void Update()
    {
        if (OKHandPose)
        {
            if (GetGesture(MLHands.Left, MLHandKeyPose.OpenHandBack)
            || GetGesture(MLHands.Right, MLHandKeyPose.OpenHandBack))
                cube.transform.Rotate(Vector3.up, +speed * Time.deltaTime);

            if (GetGesture(MLHands.Left, MLHandKeyPose.Fist)
            || GetGesture(MLHands.Right, MLHandKeyPose.Fist))
                cube.transform.Rotate(Vector3.up, -speed * Time.deltaTime);

            if (GetGesture(MLHands.Left, MLHandKeyPose.Finger))
                cube.transform.Rotate(Vector3.right, +speed * Time.deltaTime);

            if (GetGesture(MLHands.Right, MLHandKeyPose.Finger))
                cube.transform.Rotate(Vector3.right, -speed * Time.deltaTime);
        }
        else
        {
            if (GetGesture(MLHands.Left, MLHandKeyPose.Ok)
            || GetGesture(MLHands.Right, MLHandKeyPose.Ok))
            {
                OKHandPose = true;

                // Cube is re-activated
                cube.SetActive(true);
                cube.transform.position = transform.position + transform.forward * distance;
                cube.transform.rotation = transform.rotation;
            }
        }
    }

    bool GetGesture(MLHand hand, MLHandKeyPose type)
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