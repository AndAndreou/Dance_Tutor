using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// joint data structures
/// inspired from BVH data structures
/// </summary>
[Serializable]
public class Joint
{
    /// <summary>
    /// 
    /// </summary>
    public enum Channel
    {
        Xposition = 1,
        Yposition = 2,
        Zposition = 4,
        Zrotation = 10,
        Xrotation = 20,
        Yrotation = 40
    };

    /// <summary>
    /// BVH channel string
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    public static string ChannelString(Channel c)
    {
        return c.ToString();
    }

    public int hierarchyID;

    /// <summary>
    /// 
    /// </summary>
    public string name;

    /// <summary>
    /// 
    /// </summary>
    public Joint parent = null;

    /// <summary>
    /// 
    /// </summary>
    public Vector3 offset;

    /// <summary>
    /// 
    /// </summary>
    public int channelsNumber;

    /// <summary>
    /// 
    /// </summary>
    public ArrayList children = new ArrayList();

    /// <summary>
    /// 
    /// </summary>
    public Matrix4x4 matrix;

    /// <summary>
    /// 
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// 
    /// </summary>
    public Quaternion rotation;

    /// <summary>
    /// 
    /// </summary>
    public Vector3 velocity;

    /// <summary>
    /// 
    /// </summary>
    public int channelStart;

    /// <summary>
    /// 
    /// </summary>
    public Channel[] channelsOrder;
        
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Joint Clone()
    {
        Joint joint = new Joint();

        joint.hierarchyID = hierarchyID;
        joint.name = name;
        joint.parent = parent;  // keep by references
        joint.offset = offset;
        joint.channelsNumber = channelsNumber;
        joint.matrix = matrix;
        joint.position = position;
        joint.rotation = rotation;
        joint.velocity = velocity;
        joint.channelStart = channelStart;
        if (channelsOrder == null)
            joint.channelsOrder = null;
        else
        {
            joint.channelsOrder = new Channel[channelsOrder.Length];
            for (int i = 0; i < channelsOrder.Length; i++)
            {
                joint.channelsOrder[i] = channelsOrder[i];
            }
        }
        joint.children = new ArrayList();
        foreach(Joint child in children)
        {
            Joint j = child.Clone();
            j.parent = joint;   // override the reference to a new object
            joint.children.Add(j);
        }

        return joint;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="positions"></param>
    public void ExtractJointPositionRecursive(ref List<Vector3> positions)
    {
        positions.Add(position);

        foreach (Joint child in children)
            child.ExtractJointPositionRecursive(ref positions);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rotations"></param>
    public void ExtractJointRotationRecursive(ref List<Quaternion> rotations)
    {
        rotations.Add(rotation);

        foreach (Joint child in children)
            child.ExtractJointRotationRecursive(ref rotations);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="velocities"></param>
    public void ExtractJointVelocityRecursive(ref List<Vector3> velocities)
    {
        velocities.Add(velocity);

        foreach (Joint child in children)
            child.ExtractJointVelocityRecursive(ref velocities);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="positions"></param>
    /// <param name="rotations"></param>
    /// <param name="velocities"></param>
    /// <param name="offsets"></param>
    public void ExtractJointRecursive(ref List<Vector3> positions, ref List<Quaternion> rotations, ref List<Vector3> velocities, ref List<Vector3> offsets)
    {
        positions.Add(position);

        rotations.Add(rotation);

        velocities.Add(velocity);

        offsets.Add(offset);

        foreach (Joint child in children)
            child.ExtractJointRecursive(ref positions, ref rotations, ref velocities, ref offsets);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="lengths"></param>
    public void ExtractBoneLengthRecursive(ref List<float> lengths)
    {
        float l = 0;
        if (parent != null)
            l = (parent.position - position).magnitude;

        lengths.Add(l);

        foreach (Joint child in children)
            child.ExtractBoneLengthRecursive(ref lengths);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="positions"></param>
    /// <param name="rotations"></param>
    public void SetValuesRecursive(ref int index, List<Vector3> positions, List<Quaternion> rotations)
    {
        position = positions[index];
        rotation = rotations[index];
        index++;

        foreach (Joint child in children)
        {
            child.SetValuesRecursive(ref index, positions, rotations);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="jointIndex"></param>
    /// <param name="rotation"></param>
    public void SetRotationRecursive(ref int jointIndex, Quaternion rotation)
    {
        if (jointIndex == 0)
        {
            this.rotation = rotation;
        }
        else
        {
            foreach (Joint child in children)
            {
                jointIndex--;
                child.SetRotationRecursive(ref jointIndex, rotation);
            }
        }
    }

}

