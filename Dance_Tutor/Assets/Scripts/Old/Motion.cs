using System;
using System.Collections.Generic;
using UnityEngine;

public class Motion
{
    public string name;
    public int framesNumber;
    public float frameTime;
    public int channelsNumber;
    public int channelStart = 0;
    //public Attitude attitude = Attitude.Neutral;
    //public GesturePhase phase = GesturePhase.None;
    //public float attitudeIntensity = 1;
    public float[] data;
    public Joint rootJoint;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="motion"></param>
    /// <param name="joint"></param>
    public delegate void OnJointUpdated(Motion motion, Joint joint);

    /// <summary>
    /// 
    /// </summary>
    public OnJointUpdated onJointUpdated = null;

    //public enum Attitude
    //{
    //    Neutral = 0,
    //    Friendly = 1,
    //    Aggresive = 2,
    //}
        
    //public enum GesturePhase
    //{
    //    None = 0,
    //    Preparation = 1,
    //    Hold = 2,
    //    Stroke = 3
    //}

    public Motion()
    {
        this.name = "";
        //this.attitude = Attitude.Neutral;
        //this.attitudeIntensity = 1;
        //this.phase = GesturePhase.None;
    }

    public Motion(string name)
    {
        this.name = name;
        //this.attitude = Attitude.Neutral;
        //this.attitudeIntensity = 1;
        //this.phase = GesturePhase.None;
    }

    public Motion(string name, /*Attitude attitude,*/ float attitudeIntensity)
    {
        this.name = name;
        //this.attitude = attitude;
        //this.attitudeIntensity = attitudeIntensity;
        //this.phase = GesturePhase.None;
    }

    //public Motion(string name, Attitude attitude, float attitudeIntensity, GesturePhase phase)
    //{
    //    this.name = name;
    //    //this.attitude = attitude;
    //    //this.attitudeIntensity = attitudeIntensity;
    //    //this.phase = phase;
    //}

    /// <summary>
    /// copy the sturecture of this motion (without data)
    /// </summary>
    /// <returns></returns>
    public Motion CopyStructure()
    {
        Motion motion = new Motion();
        motion.name = name;
        //motion.attitude = attitude;
        motion.channelStart = channelStart;
        motion.rootJoint = rootJoint.Clone();
        motion.framesNumber = framesNumber;
        motion.channelsNumber = channelsNumber;
        motion.frameTime = frameTime;

        return motion;
    }

    /// <summary>
    /// update joint, need to be called after calling MoveTo function to update all related joints
    /// </summary>
    /// <param name="joint"></param>
    public void UpdateJointObject(Joint joint = null)
    {
        if (joint == null)
            joint = rootJoint;

        if (onJointUpdated != null)
        {
            onJointUpdated(this, joint);
        }

        foreach (Joint child in joint.children)
            UpdateJointObject(child);
    }
        
    /// <summary>
    /// 
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="data"></param>
    public void RecordTo(int frame, float[] data)
    {
        int startIndex = frame * channelsNumber;
        if (this.data == null)
        {
            this.data = new float[startIndex + data.Length];
        }
        else if (this.data.Length < startIndex + data.Length)
        {
            Array.Resize(ref this.data, startIndex + data.Length);
        }
        framesNumber = this.data.Length / channelsNumber - 1;

        Array.Copy(data, 0, this.data, startIndex, data.Length);
    }

    /// <summary>
    /// get joint data from this motion
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="joint"></param>
    //public void RecordTo(int frame, Joint joint)
    //{
    //    List<float> values = new List<float>();

    //    RecordJoint(joint, ref values);

    //    int startIndex = frame * channelsNumber;

    //    if (data == null)
    //    {
    //        data = new float[startIndex + values.Count];
    //        framesNumber = 1;
    //    }
    //    else
    //    {
    //        if (data.Length < startIndex + values.Count)
    //            Array.Resize(ref data, startIndex + values.Count);
    //    }
    //    framesNumber = this.data.Length / channelsNumber - 1;

    //    for (int i = 0; i < values.Count; i++)
    //    {
    //        data[startIndex + i] = values[i];

    //    }
    //}

    /// <summary>
    /// get motion values data from this motion
    /// </summary>
    /// <param name="joint"></param>
    /// <param name="values"></param>
    //private void RecordJoint(Joint joint, ref List<float> values)
    //{
    //    joint.matrix = Matrix4x4.TRS(joint.position, joint.rotation, Vector3.one);

    //    if (joint.parent != null)
    //    {
    //        joint.parent.matrix = Matrix4x4.TRS(joint.parent.position, joint.parent.rotation, Vector3.one);

    //        joint.matrix = joint.parent.matrix.inverse * joint.matrix;
    //    }

    //    Vector3 position = joint.matrix.GetColumn(3);
    //    Quaternion rotation = Quaternion.FromMatrix(joint.matrix);

    //    Vector3 axis;
    //    float angle;
    //    rotation.ToAxisAngle(out axis, out angle);
    //    Vector3 angleRotation = axis * angle;

    //    for (int i = 0; i < joint.channelsNumber; i++)
    //    {
    //        // channel alias
    //        Joint.Channel channel = joint.channelsOrder[i];
    //        float value = 0.0f;

    //        if (channel == Joint.Channel.Xposition)
    //        {
    //            value = position.x;
    //        }
    //        if (channel == Joint.Channel.Yposition)
    //        {
    //            value = position.y;
    //        }
    //        if (channel == Joint.Channel.Zposition)
    //        {
    //            value = position.z;
    //        }

    //        if (channel == Joint.Channel.Xrotation)
    //        {
    //            value = angleRotation.x;
    //        }
    //        if (channel == Joint.Channel.Yrotation)
    //        {
    //            value = angleRotation.y;
    //        }
    //        if (channel == Joint.Channel.Zrotation)
    //        {
    //            value = angleRotation.z;
    //        }

    //        values.Add(value);
    //    }

    //    foreach (Joint child in joint.children)
    //        RecordJoint(child, ref values);
    //}

    /// <summary>
    /// move root joint to specific frame
    /// </summary>
    /// <param name="frame"></param>
    //public void MoveTo(int frame)
    //{
    //    MoveTo(frame, out rootJoint);
    //}

    /// <summary>
    /// move joint to specific frame
    /// </summary>
    /// <param name="frame"></param>
    /// <param name="joint"></param>
    //public void MoveTo(int frame, out Joint joint)
    //{
    //    if (frame >= framesNumber)
    //        frame = framesNumber - 1;
    //    if (frame < 0)
    //        frame = 0;

    //    // we calculate motion data's array start index for a frame
    //    int startIndex = frame * channelsNumber;

    //    // recursively transform skeleton
    //    MoveJoint(ref rootJoint, startIndex);

    //    joint = rootJoint;
    //}

    /// <summary>
    /// recursive move joint function
    /// </summary>
    /// <param name="joint"></param>
    /// <param name="frameStartsIndex"></param>
    //private void MoveJoint(ref Joint joint, int frameStartsIndex)
    //{
    //    // we'll need index of motion data's array with start of this specific joint
    //    int startIndex = frameStartsIndex + joint.channelStart;

    //    // apply offset 
    //    joint.matrix = Matrix4x4.identity;

    //    Vector3 translation = joint.offset;
    //    Quaternion rotation = Quaternion.identity;

    //    // here we transform joint's local matrix with each specified channel's values
    //    // which are read from motion data

    //    for (int i = 0; i < joint.channelsNumber; i++)
    //    {
    //        // channel alias
    //        Joint.Channel channel = joint.channelsOrder[i];

    //        // extract value from motion data
    //        float value = data[startIndex + i];

    //        if (channel == Joint.Channel.Xposition)
    //        {
    //            translation += new Vector3(value, 0, 0);
    //        }
    //        if (channel == Joint.Channel.Yposition)
    //        {
    //            translation += new Vector3(0, value, 0);
    //        }
    //        if (channel == Joint.Channel.Zposition)
    //        {
    //            translation += new Vector3(0, 0, value);
    //        }

    //        if (channel == Joint.Channel.Xrotation)
    //        {
    //            rotation *= Quaternion.AngleAxis(value, Vector3.right);
    //        }
    //        if (channel == Joint.Channel.Yrotation)
    //        {
    //            rotation *= Quaternion.AngleAxis(value, Vector3.up);
    //        }
    //        if (channel == Joint.Channel.Zrotation)
    //        {
    //            rotation *= Quaternion.AngleAxis(value, Vector3.forward);
    //        }
    //    }

    //    // apply translation and / or rotation to current matrix
    //    joint.matrix = Matrix4x4.TRS(translation, rotation, Vector3.one);

    //    // then we apply parent's local transfomation matrix to this joint's LTM (local tr. mtx. :)
    //    if (joint.parent != null)
    //    {
    //        // apply parent matrix to current matrix
    //        joint.matrix = joint.parent.matrix * joint.matrix;
    //    }

    //    joint.position = joint.matrix.GetColumn(3);
    //    joint.rotation = Quaternion.FromMatrix(joint.matrix);

    //    // when we have calculated parent's matrix do the same to all children
    //    for (int i = 0; i < joint.children.Count; i++)
    //    {
    //        Joint j = (Joint)joint.children[i];

    //        MoveJoint(ref j, frameStartsIndex);

    //        joint.children[i] = j;
    //    }
    //}

    public override string ToString()
    {
        return name;
    }
}

