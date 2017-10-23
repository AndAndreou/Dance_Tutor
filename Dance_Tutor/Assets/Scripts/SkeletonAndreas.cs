using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkeletonAndreas
{
    public List<JointAndreas> skeleton;

    public SkeletonAndreas()
    {
        skeleton = new List<JointAndreas>();
    }

    public SkeletonAndreas(List<JointAndreas> joints)
    {
        skeleton = joints;
    }

    /// <summary>
    /// Return joint with name 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public JointAndreas GetJointByName(JointName name)
    {
        foreach(JointAndreas joint in skeleton)
        {
            if(joint.name == name)
            {
                return joint;
            }
        }
        return null;
    }

    /// <summary>
    /// Add values of each joint into a frame value
    /// </summary>
    public void AutoAddFrameValuesForEachJoint()
    {
        foreach (JointAndreas joint in skeleton)
        {
            joint.AddFrame();
        }
    }
}
