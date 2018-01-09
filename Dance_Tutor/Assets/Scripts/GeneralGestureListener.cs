using UnityEngine;
//using Windows.Kinect;
using System.Collections;
using System;

public class GeneralGestureListener : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    [Tooltip("Index of the player, tracked by this component. 0 means the 1st player, 1 - the 2nd one, 2 - the 3rd one, etc.")]
    public int playerIndex = 0;

    [Tooltip("UI-Text to display gesture-listener messages and gesture information.")]
    public UnityEngine.UI.Text gestureInfo;

    // private bool to track if progress message has been displayed
    private bool progressDisplayed;
    private float progressGestureTime;


    public void UserDetected(long userId, int userIndex)
    {
        if (userIndex != playerIndex)
            return;

        // as an example - detect these user specific gestures
        KinectManager manager = KinectManager.Instance;
        manager.DetectGesture(userId, KinectGestures.Gestures.Tpose);

        //manager.DetectGesture(userId, KinectGestures.Gestures.Run);

        if (gestureInfo != null)
        {
            gestureInfo.text = "No gesture find";
        }
    }

    public void UserLost(long userId, int userIndex)
    {
        if (userIndex != playerIndex)
            return;

        if (gestureInfo != null)
        {
            gestureInfo.text = string.Empty;
        }
    }

    public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  float progress, KinectInterop.JointType joint, Vector3 screenPos)
    {
        if (userIndex != playerIndex)
            return;

        if (gesture == KinectGestures.Gestures.Tpose && progress > 0.5f)
        {
            if (gestureInfo != null)
            {
                string sGestureText = "T-Pose Detect .....";
                gestureInfo.text = sGestureText;

                progressDisplayed = true;
                progressGestureTime = Time.realtimeSinceStartup;

                GameManager.instance.TPoseDetectionEvent.Invoke();
            }
        }
    }

    public bool GestureCompleted(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint, Vector3 screenPos)
    {
        if (userIndex != playerIndex)
            return false;

        if (progressDisplayed)
            return true;

        string sGestureText = gesture + " detected";
        if (gestureInfo != null)
        {
            gestureInfo.text = sGestureText;
        }

        return true;
    }

    public bool GestureCancelled(long userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectInterop.JointType joint)
    {
        if (userIndex != playerIndex)
            return false;

        if (progressDisplayed)
        {
            progressDisplayed = false;

            if (gestureInfo != null)
            {
                gestureInfo.text = String.Empty;
            }
        }

        return true;
    }

    public void Update()
    {
        if (progressDisplayed && ((Time.realtimeSinceStartup - progressGestureTime) > 2f))
        {
            progressDisplayed = false;

            if (gestureInfo != null)
            {
                gestureInfo.text = String.Empty;
            }

            Debug.Log("Forced progress to end.");
        }
    }

}
