using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Joint
{
    public JointName name;
    public GameObject jointGO;
    public List<Frame> frames;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="go"></param>
    public Joint(JointName name,GameObject go)
    {
        this.name = name;
        this.jointGO = go;
        this.frames = new List<Frame>();
    }

    public JointName GetJointName()
    {
        return name;
    }

    public GameObject GetJointGameObject()
    {
        return jointGO;
    }

    /// <summary>
    /// add new frame position
    /// </summary>
    /// <param name="position"></param>
    public void SetFramePosition(Vector3 position,int frameIndex)
    {
        Frame temp = frames[frameIndex];
        temp.position = position;
        frames[frameIndex] = temp;
    }

    /// <summary>
    /// add new frame rotation
    /// </summary>
    /// <param name="rotation"></param>
    public void SetFrameRotation(Vector3 rotation, int frameIndex)
    {
        Frame temp = frames[frameIndex];
        temp.rotation = rotation;
        frames[frameIndex] = temp;
    }

    /// <summary>
    /// Return the position of joint for frame with some index
    /// </summary>
    /// <param name="frameIndex"></param>
    /// <returns></returns>
    public Vector3 GetPositionForFrame(int frameIndex)
    {
        return frames[frameIndex].position;
    }

    /// <summary>
    /// Return the rotation of joint for frame with some index
    /// </summary>
    /// <param name="frameIndex"></param>
    /// <returns></returns>
    public Vector3 GetRotationForFrame(int frameIndex)
    {
        return frames[frameIndex].rotation;
    }

    /// <summary>
    /// Add frame by current values of joitn game object
    /// </summary>
    public Frame AddFrame(Transform callerTransform)
    {
        if (frames == null)
        {
            frames = new List<Frame>();
        }
        //Get position and direction relative to the caller transform
        //Frame newFrame = new Frame(callerTransform.InverseTransformPoint(jointGO.transform.position), callerTransform.InverseTransformDirection(jointGO.transform.eulerAngles), Time.time);
        
        //Get position relative to the caller transform and local euler angles
        Frame newFrame = new Frame(callerTransform.InverseTransformPoint(jointGO.transform.position), (jointGO.transform.localEulerAngles), Time.time);
        frames.Add(newFrame);
        return newFrame;
    }

    /// <summary>
    /// add new frame (position and rotation)
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotetion"></param>
    public void AddFrame(Vector3 position,Vector3 rotetion, float time)
    {
        frames.Add(new Frame(position, rotetion,time));
    }

    /// <summary>
    /// Get the number of total frames
    /// </summary>
    /// <returns></returns>
    public int GetTotalFrames()
    {
        return frames.Count;
    }

    /// <summary>
    /// Get th frame with index
    /// </summary>
    /// <param name="frameIndex"></param>
    /// <returns></returns>
    public Frame GetFrame(int frameIndex)
    {
        return frames[frameIndex];
    }

    /// <summary>
    /// Return the last x frames
    /// With out param rerurn the last frame
    /// </summary>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public List<Frame> GetLastFrames(int quantity = 1)
    {
        List<Frame> returnList = new List<Frame>();
        int lastFrameIndex = GetTotalFrames()-1;
        for(int i = 0; i < quantity; i++)
        {
            returnList.Add(frames[lastFrameIndex - i]);
        }

        return returnList;
    }
}

public struct Frame
{
    public Vector3 position;
    public Vector3 rotation;
    public float time;

    public Frame(Vector3 pos, Vector3 rot, float t)
    {
        position = pos;
        rotation = rot;
        time = t;
    }
}

public enum JointName
{
    Root = 0,
    LeftUpLeg = 1,
    LeftLeg = 2,
    LeftFoot = 3,
    LeftToeBase = 4,
    EndSiteLeftToeBase = 5,
    RightUpLeg = 6,
    RightLeg = 7,
    RightFoot = 8,
    RightToeBase = 9,
    EndSiteRightToeBase = 10,
    Spine = 11,
    Spine1 = 12,
    Spine2 = 13,
    Spine3 = 14,
    Spine4 = 15,
    RightShoulder = 16,
    RightArm = 17,
    RightForeArm = 18,
    RightHand = 19,
    RightHandMiddle1 = 20,
    RightHandMiddle2 = 21,
    RightHandMiddle3 = 22,
    EndSiteRightHandMiddle = 23,
    RightHandRing1 = 24,
    RightHandRing2 = 25,
    RightHandRing3 = 26,
    EndSiteRightHandRing = 27,
    RightHandPinky1 = 28,
    RightHandPinky2 = 29,
    RightHandPinky3 = 30,
    EndSiteRightHandPinky = 31,
    RightHandIndex1 = 32,
    RightHandIndex2 = 33,
    RightHandIndex3 = 34,
    EndSiteRightHandIndex = 35,
    RightHandThumb1 = 36,
    RightHandThumb2 = 37,
    RightHandThumb3 = 38,
    EndSiteRightHandThumb = 39,
    LeftShoulder = 40,
    LeftArm = 41,
    LeftForeArm = 42,
    LeftHand = 43,
    LeftHandMiddle1 = 44,
    LeftHandMiddle2 = 45,
    LeftHandMiddle3 = 46,
    EndSiteLeftHandMiddle = 47,
    LeftHandRing1 = 48,
    LeftHandRing2 = 49,
    LeftHandRing3 = 50,
    EndSiteLeftHandRing = 51,
    LeftHandPinky1 = 52,
    LeftHandPinky2 = 53,
    LeftHandPinky3 = 54,
    EndSiteLeftHandPinky = 55,
    LeftHandIndex1 = 56,
    LeftHandIndex2 = 57,
    LeftHandIndex3 = 58,
    EndSiteLeftHandIndex = 59,
    LeftHandThumb1 = 60,
    LeftHandThumb2 = 61,
    LeftHandThumb3 = 62,
    EndSiteLeftHandThumb = 63,
    Neck = 64,
    Head = 65,
    EndSiteHead = 66
}