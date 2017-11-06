using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkeletonAndreas
{
    public List<JointAndreas> skeleton;

    [Header("MotionWord")]
    public int motionWordStep = 5;
    public int motionWordWindowSize = 35;
    [HideInInspector]
    public List<MotionWord> motionWords = new List<MotionWord>();
    private int motionWordStepCounter;

    [Header("StyleWord")]
    public int stykeWordStep = 5;
    public int styleWordWindowSize = 35;
    [HideInInspector]
    public List<StyleWord> styleWords = new List<StyleWord>();
    private int styleWordStepCounter;

    public struct MotionWord
    {
        // List is for joints and array of quaternion for rotation valus by frame
        public List<Quaternion[]> joint;
    }

    public struct StyleWord
    {
        public float feetHipDistanceMax;
        public float feetHipDistanceMin;
        public float feetHipDistanceMean;
        public float feetHipDistanceStd;

        public float handsShoulderDistanceMax;
        public float handsShoulderDistanceMin;
        public float handsShoulderDistanceMean;
        public float handsShoulderDistanceStd;

        public float handsDistanceMax;
        public float handsDistanceMin;
        public float handsDistanceMean;
        public float handsDistanceStd;

        public float handsHeadDistanceMax;
        public float handsHeadDistanceMin;
        public float handsHeadDistanceMean;
        public float handsHeadDistanceStd;

        public float pelvisHeightMax;
        public float pelvisHeightMin;
        public float pelvisHeightMean;
        public float pelvisHeightStd;

        public float hipGroundMinusFeetHipMax;
        public float hipGroundMinusFeetHipMin;
        public float hipGroundMinusFeetHipMean;
        public float hipGroundMinusFeetHipStd;

        public float centroidHeightMax;
        public float centroidHeightMin;
        public float centroidHeightMean;
        public float centroidHeightStd;

        public float centroidPelvisDistanceMax;
        public float centroidPelvisDistanceMin;
        public float centroidPelvisDistanceMean;
        public float centroidPelvisDistanceStd;

        public float gaitSizeMax;
        public float gaitSizeMin;
        public float gaitSizeMean;
        public float gaitSizeStd;

        public float headOrientationMax;
        public float headOrientationMin;
        public float headOrientationMean;

        public float decelerationPeaksNo;

        public float hipVelocityMax;
        public float hipVelocityMin;
        public float hipVelocityStd;

        public float handsVelocityMax;
        public float handsVelocityMin;
        public float handsVelocityStd;

        public float feetVelocityMax;
        public float feetVelocityMin;
        public float feetVelocityStd;

        public float hipAccelerationMax;
        public float hipAccelerationStd;

        public float handsAccelerationMax;
        public float handsAccelerationStd;

        public float feetAccelerationMax;
        public float feetAccelerationStd;

        public float jerkAccelerationMax;
        public float jerkAccelerationStd;

        public float volumeMax;
        public float volumeMin;
        public float volumeMean;
        public float volumeStd;

        public float volumeUpperBodyMax;
        public float volumeUpperBodyMin;
        public float volumeUpperBodyMean;
        public float volumeUpperBodyStd;

        public float volumeLowerBodyMax;
        public float volumeLowerBodyMin;
        public float volumeLowerBodyMean;
        public float volumeLowerBodyStd;

        public float volumeLeftSideMax;
        public float volumeLeftSideMin;
        public float volumeLeftSideMean;
        public float volumeLeftSideStd;

        public float volumeRightSideMax;
        public float volumeRightSideMin;
        public float volumeRightSideMean;
        public float volumeRightSideStd;

        public float torsoHeightMax;
        public float torsoHeightMin;
        public float torsoHeightMean;
        public float torsoHeightStd;

        public float handsLevelNo1;
        public float handsLevelNo2;
        public float handsLevelNo3;

        public float totalDistanceNo;

        public float totalAreaNo;
    }

    private List<Vector3> skeletonCentroid;

    public SkeletonAndreas()
    {
        skeleton = new List<JointAndreas>();
        skeletonCentroid = new List<Vector3>();
        motionWordStepCounter = motionWordWindowSize;
        styleWordStepCounter = styleWordWindowSize;
    }

    public SkeletonAndreas(List<JointAndreas> joints)
    {
        skeleton = joints;

        skeletonCentroid = new List<Vector3>();
        motionWordStepCounter = motionWordWindowSize;
        styleWordStepCounter = styleWordWindowSize;
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
        Vector3 centroidForThisFrame = Vector3.zero;
        foreach (JointAndreas joint in skeleton)
        {
            Frame f = joint.AddFrame();
            centroidForThisFrame = f.position;
        }

        // Calculate centroid
        centroidForThisFrame = centroidForThisFrame / skeleton.Count;
        skeletonCentroid.Add(centroidForThisFrame);

        // Check and put the new motion word if is the time
        motionWordStepCounter--;
        if(motionWordStepCounter <= 0)
        {
            AddMotionWord();
        }

        // Check and put the new style word if is the time
        styleWordStepCounter--;
        if (styleWordStepCounter <= 0)
        {
            AddStyleWord();
        }
    }

    /// <summary>
    /// Add motion word in motion words list
    /// </summary>
    public void AddMotionWord()
    {
        MotionWord motionWord = new MotionWord();

        foreach( JointAndreas joint in skeleton)
        {
            // Get last frames per joint and get only rotations
            Frame[] framePerJoint = joint.GetLastFrames(motionWordWindowSize).ToArray();
            Quaternion[] jointRotationByFrame = new Quaternion[motionWordWindowSize];
            for (int i = 0; i < motionWordWindowSize; i++)
            {
                jointRotationByFrame[i] = framePerJoint[i].rotation;
            }
            motionWord.joint.Add(jointRotationByFrame);
        }

        motionWords.Add(motionWord);

        motionWordStepCounter = motionWordWindowSize;
    }

    /// <summary>
    /// Add style word in style words list
    /// </summary>
    public void AddStyleWord()
    {
        // Feet to hips distance
        float[] f1 = new float[styleWordWindowSize];

        List<Frame> endSiteLeftToeBase = GetJointByName(JointName.EndSiteLeftToeBase).GetLastFrames(styleWordWindowSize);
        List<Frame> leftUpLeg = GetJointByName(JointName.LeftUpLeg).GetLastFrames(styleWordWindowSize);

        List<Frame> endSiteRightToeBase = GetJointByName(JointName.EndSiteRightToeBase).GetLastFrames(styleWordWindowSize);
        List<Frame> rightUpLeg = GetJointByName(JointName.RightUpLeg).GetLastFrames(styleWordWindowSize);

        // Hands to shoulders distance
        float[] f2 = new float[styleWordWindowSize];

        List<Frame> endSiteLeftHandMiddle = GetJointByName(JointName.EndSiteLeftHandMiddle).GetLastFrames(styleWordWindowSize);
        List<Frame> leftShoulder = GetJointByName(JointName.LeftShoulder).GetLastFrames(styleWordWindowSize);

        List<Frame> endSiteRightHandMiddle = GetJointByName(JointName.EndSiteRightHandMiddle).GetLastFrames(styleWordWindowSize);
        List<Frame> rightShoulder = GetJointByName(JointName.RightShoulder).GetLastFrames(styleWordWindowSize);

        // Right hand to left hand distance
        float[] f3 = new float[styleWordWindowSize];

        // Hands to head distance
        float[] f4 = new float[styleWordWindowSize];

        List<Frame> endSiteHead = GetJointByName(JointName.EndSiteHead).GetLastFrames(styleWordWindowSize);

        // Pelvis heigh
        float[] f5 = new float[styleWordWindowSize];

        List<Frame> root = GetJointByName(JointName.Root).GetLastFrames(styleWordWindowSize);

        // Distance of the hips to the ground, minus the distance of the feet to the hips
        float[] f6 = new float[styleWordWindowSize];

        List<Frame> leftFoot = GetJointByName(JointName.LeftFoot).GetLastFrames(styleWordWindowSize);
        List<Frame> rightFoot = GetJointByName(JointName.RightFoot).GetLastFrames(styleWordWindowSize);

        // Distance between the ground and the centroid
        float[] f7 = new float[styleWordWindowSize];

        // Distance between the centroid and the pelvis (Centroid)
        float[] f8 = new float[styleWordWindowSize];

        // Distance of the right foot to the left (Gait)
        float[] f9 = new float[styleWordWindowSize];

        // Head orientation
        Quaternion[] f10 = new Quaternion[styleWordWindowSize];

        // Deceleration of motion
        float[] f11 = new float[styleWordWindowSize]; // ? how to calculate this?

        // Root Velocity
        Vector3[] f12 = new Vector3[styleWordWindowSize];
        f12[0] = Vector3.zero;

        // The average velocity of both hands
        Vector3[] f13 = new Vector3[styleWordWindowSize];
        f13[0] = Vector3.zero;

        // The average velocity of both feet
        Vector3[] f14 = new Vector3[styleWordWindowSize];
        f14[0] = Vector3.zero;

        // Root acceleration
        Vector3[] f15 = new Vector3[styleWordWindowSize];
        f15[0] = Vector3.zero;

        // The average acceleration of both hands
        Vector3[] f16 = new Vector3[styleWordWindowSize];
        f16[0] = Vector3.zero;

        // The average acceleration of both feet
        Vector3[] f17 = new Vector3[styleWordWindowSize];
        f17[0] = Vector3.zero;

        // Changes of acceleration or force and it iscalculated by taking the derivative of the acceleration (Jerk)
        Vector3[] f18 = new Vector3[styleWordWindowSize];
        f18[0] = Vector3.zero;


        for (int i = 0; i < styleWordWindowSize; i++)
        {
            f1[i] = (Vector3.Distance(endSiteLeftToeBase[i].position, leftUpLeg[i].position) + Vector3.Distance(endSiteRightToeBase[i].position, rightUpLeg[i].position)) / 2f;

            f2[i] = (Vector3.Distance(endSiteLeftHandMiddle[i].position, leftShoulder[i].position) + Vector3.Distance(endSiteRightHandMiddle[i].position, rightShoulder[i].position)) / 2f;

            f3[i] = Vector3.Distance(endSiteRightHandMiddle[i].position, endSiteLeftHandMiddle[i].position);

            f4[i] = (Vector3.Distance(endSiteLeftHandMiddle[i].position, endSiteHead[i].position) + Vector3.Distance(endSiteRightHandMiddle[i].position, endSiteHead[i].position)) / 2f;

            f5[i] = root[i].position.y;

            f6[i] = ((leftUpLeg[i].position.y - Vector3.Distance(endSiteLeftToeBase[i].position, leftFoot[i].position)) + (rightUpLeg[i].position.y - Vector3.Distance(endSiteRightToeBase[i].position, rightFoot[i].position))) / 2f;

            Vector3 centroid = skeletonCentroid[skeletonCentroid.Count - 1 - i];
            f7[i] = centroid.y;

            f8[i] = Vector3.Distance(centroid, root[i].position);

            f9[i] = Vector3.Distance(endSiteLeftToeBase[i].position, endSiteRightToeBase[i].position);

            f10[i] = endSiteHead[i].rotation;

            if (i > 0)
            {
                f12[i] = ((root[i].position - root[i-1].position) / (root[i].time - root[i-1].time));

                Vector3 leftHandVelocity = ((endSiteLeftHandMiddle[i].position - endSiteLeftHandMiddle[i - 1].position) / (endSiteLeftHandMiddle[i].time - endSiteLeftHandMiddle[i - 1].time));
                Vector3 rightHandVelocity = ((endSiteRightHandMiddle[i].position - endSiteRightHandMiddle[i - 1].position) / (endSiteRightHandMiddle[i].time - endSiteRightHandMiddle[i - 1].time));
                f13[i] = (leftHandVelocity + rightHandVelocity) /2f;

                Vector3 leftFootVelocity = ((endSiteLeftToeBase[i].position - endSiteLeftToeBase[i - 1].position) / (endSiteLeftToeBase[i].time - endSiteLeftToeBase[i - 1].time));
                Vector3 rightFootVelocity = ((endSiteRightToeBase[i].position - endSiteRightToeBase[i - 1].position) / (endSiteRightToeBase[i].time - endSiteRightToeBase[i - 1].time));
                f14[i] = (leftFootVelocity + rightFootVelocity) / 2f;

                f15[i] = (f12[i] - f12[i - 1]) / (root[i].time - root[i - 1].time);

                f16[i] = (f13[i] - f13[i - 1]) / (endSiteLeftHandMiddle[i].time - endSiteLeftHandMiddle[i - 1].time);

                f17[i] = (f14[i] - f14[i - 1]) / (endSiteLeftToeBase[i].time - endSiteLeftToeBase[i - 1].time);

                f18[i] = (f15[i] - f15[i - 1]) / (root[i].time - root[i - 1].time);
            }

        }
    }
}

