using System.IO;
using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;

public class BVHLoader
{
    List<Motion> motions = new List<Motion>();
    List<IEnumerator> tokens = new List<IEnumerator>();
    int currentHierarchyID = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <param name="joint"></param>
    public delegate void OnJointUpdated(int index, Joint joint);

    /// <summary>
    /// 
    /// </summary>
    public OnJointUpdated onJointUpdated = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Motion GetMotion(int index)
    {
        return motions[index];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Joint GetRootJoint(int index)
    {
        return motions[index].rootJoint;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public string GetRootJointName(int index)
    {
        return motions[index].rootJoint.name;
    }

    /// <summary>
    /// save motion to bvh file
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="motion"></param>
    public void Save(string filename, Motion motion)
    {
        StreamWriter writer = new StreamWriter(filename);

        writer.WriteLine("HIERARCHY");

        SaveJoint(writer, motion.rootJoint);

        SaveMotion(writer, motion);

        writer.Close();
    }

    /// <summary>
    /// load bvh file to motion data structure
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public Motion Load(string directory, string filename)
    {
        string filePath = directory + filename;
        char[] delimiterChars = { ' ', '\t', '\n' };

        StreamReader reader = new StreamReader(filePath);
        string ss = reader.ReadToEnd();

        ArrayList s = new ArrayList();
        s.AddRange(ss.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries));

        //Motion.Attitude attitude = Motion.Attitude.Friendly;//(Motion.Attitude)UnityEngine.Random.Range(0, 3); // TEMP: should read actual attitude for this motion
        //Motion.GesturePhase phase = Motion.GesturePhase.Stroke; //(Motion.GesturePhase)UnityEngine.Random.Range(0, 4); // TEMP: should read actual phase for this motion;
        //float intensity = UnityEngine.Random.Range(0f, 5f);
        //motions.Add(new Motion(filename, attitude, intensity, phase));
        int index = motions.Count - 1;

        tokens.Add(s.GetEnumerator());

        while (tokens[index].MoveNext())
        {
            if (tokens[index].Current.ToString().Trim().StartsWith("HIERARCHY"))
                LoadHierarchy(index);
        }
        reader.Close();

        return motions[index];
    }

    /// <summary>
    /// save joint to bvh format
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="joint"></param>
    /// <param name="tab"></param>
    private void SaveJoint(StreamWriter writer, Joint joint, int tab = 0)
    {
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberDecimalSeparator = ".";
        nfi.NumberDecimalDigits = 6;

        string stab = "";
        for (int i = 0; i < tab; i++)
            stab += "\t";

        if (joint.parent == null)
            writer.WriteLine("ROOT {0}", joint.name);
        else if (joint.children.Count == 0)
            writer.WriteLine("{0}End Site", stab);
        else
            writer.WriteLine("{0}JOINT {1}", stab, joint.name);

        writer.WriteLine(stab + "{");

        stab += "\t";

        writer.WriteLine("{0}OFFSET {1} {2} {3}", stab, joint.offset.x.ToString(nfi), joint.offset.y.ToString(nfi), joint.offset.z.ToString(nfi));
        if (joint.channelsNumber > 0)
        {
            writer.Write("{0}CHANNELS {1}", stab, joint.channelsNumber);
            for (int i = 0; i < joint.channelsNumber; i++)
                writer.Write(" {0}", Joint.ChannelString(joint.channelsOrder[i]));
            writer.WriteLine("");
        }

        foreach (Joint child in joint.children)
            SaveJoint(writer, child, tab + 1);

        stab = "";
        for (int i = 0; i < tab; i++)
            stab += "\t";

        writer.WriteLine(stab + "}");
    }

    /// <summary>
    /// save motion data to bvh format
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="motion"></param>
    private void SaveMotion(StreamWriter writer, Motion motion)
    {
        NumberFormatInfo nfi = new NumberFormatInfo();
        nfi.NumberDecimalSeparator = ".";

        writer.WriteLine("MOTION");
        writer.WriteLine("Frames: {0}", motion.framesNumber);
        writer.WriteLine("Frame Time: {0}", motion.frameTime.ToString(nfi));
        for (int i = 0; i < motion.framesNumber; i++)
        {
            string s = "";
            int index = i * motion.channelsNumber;
            for (int j = 0; j < motion.channelsNumber; j++)
            {
                s += motion.data[index + j].ToString(nfi) + " ";
            }
            writer.WriteLine(s);
        }
    }

    /// <summary>
    /// load hieararchy from bvh file
    /// </summary>
    /// <param name="index"></param>
    private void LoadHierarchy(int index)
    {
        currentHierarchyID = 0;
        while (tokens[index].MoveNext())
        {
            if (tokens[index].Current.ToString().Trim().StartsWith("ROOT"))
                motions[index].rootJoint = LoadJoint(index);
            else if (tokens[index].Current.ToString().Trim().StartsWith("MOTION"))
                LoadMotion(index);
        }
    }

    /// <summary>
    /// load joint structure from bvh file
    /// </summary>
    /// <param name="index"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    private Joint LoadJoint(int index, Joint parent = null)
    {
        int channelOrderIndex = 0;

        tokens[index].MoveNext();   // ROOT / JOINT / End

        Joint joint = new Joint();
        joint.hierarchyID = currentHierarchyID;
        currentHierarchyID++;
        joint.name = tokens[index].Current.ToString().Trim();
        joint.matrix = Matrix4x4.identity;

        while (tokens[index].MoveNext())
        {
            string tmp = tokens[index].Current.ToString().Trim();
            if (tmp.StartsWith("Xposition"))
            {
                joint.channelsOrder[channelOrderIndex++] = Joint.Channel.Xposition;
            }
            if (tmp.StartsWith("Yposition"))
            {
                joint.channelsOrder[channelOrderIndex++] = Joint.Channel.Yposition;
            }
            if (tmp.StartsWith("Zposition"))
            {
                joint.channelsOrder[channelOrderIndex++] = Joint.Channel.Zposition;
            }

            if (tmp.StartsWith("Xrotation"))
            {
                joint.channelsOrder[channelOrderIndex++] = Joint.Channel.Xrotation;
            }
            if (tmp.StartsWith("Yrotation"))
            {
                joint.channelsOrder[channelOrderIndex++] = Joint.Channel.Yrotation;
            }
            if (tmp.StartsWith("Zrotation"))
            {
                joint.channelsOrder[channelOrderIndex++] = Joint.Channel.Zrotation;
            }

            // read OFFSET
            if (tmp.StartsWith("OFFSET"))
            {
                CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                // reading an offset values
                tokens[index].MoveNext(); joint.offset.x = float.Parse(tokens[index].Current.ToString().Trim(), NumberStyles.Any, ci);
                tokens[index].MoveNext(); joint.offset.y = float.Parse(tokens[index].Current.ToString().Trim(), NumberStyles.Any, ci);
                tokens[index].MoveNext(); joint.offset.z = float.Parse(tokens[index].Current.ToString().Trim(), NumberStyles.Any, ci);
            }
            else if (tmp.StartsWith("CHANNELS"))
            {
                // loading num of channels
                tokens[index].MoveNext();
                joint.channelsNumber = int.Parse(tokens[index].Current.ToString().Trim());

                // adding to motiondata
                motions[index].channelsNumber += joint.channelsNumber;

                // increasing static counter of channel index starting motion section
                joint.channelStart = motions[index].channelStart;
                motions[index].channelStart += joint.channelsNumber;

                // creating array for channel order specification
                joint.channelsOrder = new Joint.Channel[joint.channelsNumber];

            }
            else if (tmp.StartsWith("JOINT"))
            {
                // loading child joint and setting this as a parent
                Joint tmpJoint = LoadJoint(index, joint);

                tmpJoint.parent = joint;
                joint.children.Add(tmpJoint);

            }
            else if (tmp.StartsWith("End"))
            {
                Joint tmpJoint = LoadJoint(index, joint);

                tmpJoint.parent = joint;
                tmpJoint.channelsNumber = 0;
                joint.children.Add(tmpJoint);

            }
            else if (tmp == "}")
                return joint;
        }

        return joint;
    }

    /// <summary>
    /// load motion data from bvh file
    /// </summary>
    /// <param name="index"></param>
    private void LoadMotion(int index)
    {
        while (tokens[index].MoveNext())
        {
            string tmp = tokens[index].Current.ToString().Trim();

            if (tmp.StartsWith("Frames:"))
            {
                tokens[index].MoveNext();
                // loading frame number
                motions[index].framesNumber = int.Parse(tokens[index].Current.ToString().Trim());

            }
            else if (tmp.StartsWith("Frame"))
            {
                tokens[index].MoveNext();   // Time:
                tokens[index].MoveNext();

                // loading frame time
                CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                motions[index].frameTime = float.Parse(tokens[index].Current.ToString().Trim(), NumberStyles.Any, ci);

                int numFrames = motions[index].framesNumber;
                int numChannels = motions[index].channelsNumber;

                // creating motion data array
                motions[index].data = new float[numFrames * numChannels];

                // foreach frame read and store floats
                for (int frame = 0; frame < numFrames; frame++)
                {
                    for (int channel = 0; channel < numChannels; channel++)
                    {
                        tokens[index].MoveNext();

                        while (tokens[index].Current.ToString().Trim().Length == 0)
                        {
                            tokens[index].MoveNext();
                        }
                        int idx = frame * numChannels + channel;

                        // calculating index for storage
                        motions[index].data[idx] = float.Parse(tokens[index].Current.ToString().Trim(), NumberStyles.Any, ci);
                    }
                }
            }
        }
    }
}
