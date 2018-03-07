using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class Skeleton
{
    public List<Joint> joints;

    //[Header("MotionWord")]
    //public int motionWordStep = 5;
    //public int motionWordWindowSize = 35;
    public List<MotionWord> motionWords { get; private set; }
    //private int motionWordStepCounter;

    //[Header("StyleWord")]
    //public int stykeWordStep = 5;
    //public int styleWordWindowSize = 35;
    public List<StyleWord> styleWords { get; private set; } 
    //private int styleWordStepCounter;

    public List<MotionWord> frameMotions { get; private set; }

    public class MotionWord
    {
        // List is for joints and array of quaternion for rotation valus by frame
        public List<Vector3[]> joint ;

        public List<Vector3[]> GetDistanceBetweenWordsInDegrees(MotionWord[] motionWords)
        {
            List<Vector3[]> disWord = new List<Vector3[]>();
            for (int i = 0; i < motionWords.Length; i++) // motion words
            {
                for (int j = 0; j< motionWords[i].joint.Count; j++) // joints
                {
                    if (i == 0) // The first motion word
                    {
                        disWord.Add(new Vector3[motionWords[i].joint.Count]);
                    }
                    for (int x = 0; x < motionWords[i].joint[j].Length; x++) // frame
                    { 
                        if (i == 0)
                        {
                            if (x == 0)
                            {
                                disWord[j] = new Vector3[motionWords[i].joint[j].Length];
                            }
                            disWord[j][x] = (motionWords[i].joint[j][x]);
                            //Debug.Log("word:" + i + " wordRot:" + motionWords[i].joint[j][x] + " disWord:" + disWord[j][x]);
                        }
                        else
                        {
                            //Debug.Log(motionWords[i-1].joint[j][x] + " * " + disWord[j][x] + " , " + motionWords[i].joint[j][x] + " - joint:" + j + " frame:" + x + " word:" + i);
                            //disWord[j][x] -= (motionWords[i].joint[j][x]);
                            disWord[j][x] = new Vector3(Mathf.Abs(disWord[j][x].x - motionWords[i].joint[j][x].x), Mathf.Abs(disWord[j][x].y - motionWords[i].joint[j][x].y), Mathf.Abs(disWord[j][x].z - motionWords[i].joint[j][x].z));
                        }

                        // Normilaze degrees
                        disWord[j][x].x = disWord[j][x].x % 360f;
                        disWord[j][x].y = disWord[j][x].y % 360f;
                        disWord[j][x].z = disWord[j][x].z % 360f;

                        if(disWord[j][x].x < 0)
                        {
                            disWord[j][x].x = disWord[j][x].x + 360f;
                        }
                        if(disWord[j][x].y < 0)
                        {
                            disWord[j][x].y = disWord[j][x].y + 360f;
                        }
                        if(disWord[j][x].z < 0)
                        {
                            disWord[j][x].z = disWord[j][x].z + 360f;
                        }
                        //Debug.Log("-- " + disWord[j][x]);
                    }
                }
                
            }
            return disWord;
        }

        public Vector3[] GetSumOfFrames(List<Vector3[]> distanceWord)
        {
            Vector3[] sum = new Vector3[distanceWord.Count];
            for (int j = 0; j < distanceWord.Count; j++) // joints
            {
                for (int x = 0; x < distanceWord[j].Length; x++) // frame
                {
                    sum[j] += distanceWord[j][x];
                }
                // Normilaze degrees
                sum[j].x = sum[j].x % 360f;
                sum[j].y = sum[j].y % 360f;
                sum[j].z = sum[j].z % 360f;

                if (sum[j].x < 0)
                {
                    sum[j].x = sum[j].x + 360f;
                }
                if (sum[j].y < 0)
                {
                    sum[j].y = sum[j].y + 360f;
                }
                if (sum[j].z < 0)
                {
                    sum[j].z = sum[j].z + 360f;
                }
                //sum[j] = sum[j] / distanceWord[j].Length; // avg
            }
            return sum;
        }

        public float GetTotalSum(Vector3[] sumOfFrames)
        {
            float sum = 0;
            for(int i = 0; i < sumOfFrames.Length; i++)
            {
                float axisSum = (sumOfFrames[i].x + sumOfFrames[i].y + sumOfFrames[i].z);
                sum += axisSum;
            }

            return sum;
        }
    }

    [System.Serializable]
    public class StyleWord
    {
        public float feetHipDistanceMax = 0;
        public float feetHipDistanceMin = 0;
        public float feetHipDistanceMean = 0;
        public float feetHipDistanceStd = 0;

        public float handsShoulderDistanceMax = 0;
        public float handsShoulderDistanceMin = 0;
        public float handsShoulderDistanceMean = 0;
        public float handsShoulderDistanceStd = 0;

        public float handsDistanceMax = 0;
        public float handsDistanceMin = 0;
        public float handsDistanceMean = 0;
        public float handsDistanceStd = 0;

        public float handsHeadDistanceMax = 0;
        public float handsHeadDistanceMin = 0;
        public float handsHeadDistanceMean = 0;
        public float handsHeadDistanceStd = 0;

        public float pelvisHeightMax = 0;
        public float pelvisHeightMin = 0;
        public float pelvisHeightMean = 0;
        public float pelvisHeightStd = 0;

        public float hipGroundMinusFeetHipMax = 0;
        public float hipGroundMinusFeetHipMin = 0;
        public float hipGroundMinusFeetHipMean = 0;
        public float hipGroundMinusFeetHipStd = 0;

        public float centroidHeightMax = 0;
        public float centroidHeightMin = 0;
        public float centroidHeightMean = 0;
        public float centroidHeightStd = 0;

        public float centroidPelvisDistanceMax = 0;
        public float centroidPelvisDistanceMin = 0;
        public float centroidPelvisDistanceMean = 0;
        public float centroidPelvisDistanceStd = 0;

        public float gaitSizeMax = 0;
        public float gaitSizeMin = 0;
        public float gaitSizeMean = 0;
        public float gaitSizeStd = 0;

        public float headOrientationMax = 0;
        public float headOrientationMin = 0;
        public float headOrientationMean = 0;
        public float headOrientationStd = 0;

        public float decelerationPeaksNo = 0;

        public float hipVelocityMax = 0;
        public float hipVelocityMin = 0;
        public float hipVelocityStd = 0;

        public float handsVelocityMax = 0;
        public float handsVelocityMin = 0;
        public float handsVelocityStd = 0;

        public float feetVelocityMax = 0;
        public float feetVelocityMin = 0;
        public float feetVelocityStd = 0;

        public float hipAccelerationMax = 0;
        public float hipAccelerationStd = 0;

        public float handsAccelerationMax = 0;
        public float handsAccelerationStd = 0;

        public float feetAccelerationMax = 0;
        public float feetAccelerationStd = 0;

        public float jerkAccelerationMax = 0;
        public float jerkAccelerationStd = 0;

        public float volumeMax = 0;
        public float volumeMin = 0;
        public float volumeMean = 0;
        public float volumeStd = 0;

        public float volumeUpperBodyMax = 0;
        public float volumeUpperBodyMin = 0;
        public float volumeUpperBodyMean = 0;
        public float volumeUpperBodyStd = 0;

        public float volumeLowerBodyMax = 0;
        public float volumeLowerBodyMin = 0;
        public float volumeLowerBodyMean = 0;
        public float volumeLowerBodyStd = 0;

        public float volumeLeftSideMax = 0;
        public float volumeLeftSideMin = 0;
        public float volumeLeftSideMean = 0;
        public float volumeLeftSideStd = 0;

        public float volumeRightSideMax = 0;
        public float volumeRightSideMin = 0;
        public float volumeRightSideMean = 0;
        public float volumeRightSideStd = 0;

        public float torsoHeightMax = 0;
        public float torsoHeightMin = 0;
        public float torsoHeightMean = 0;
        public float torsoHeightStd = 0;

        public float handsLevelNo1 = 0;
        public float handsLevelNo2 = 0;
        public float handsLevelNo3 = 0;

        public float totalDistanceNo = 0;

        public float totalAreaNo = 0;

        public StyleWord GetMax(StyleWord styleWord,StyleWord maxStyleWord)
        {
            //float maximum = 0;
            StyleWord newStyleWord = maxStyleWord;
            foreach (var field in typeof(StyleWord).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                field.SetValue(newStyleWord, Mathf.Max((float)field.GetValue(styleWord), (float)field.GetValue(maxStyleWord)));
                //maximum = Mathf.Max(maximum, (float)field.GetValue(styleWord));
                //Debug.Log((float)field.GetValue(styleWord) + " , " + (float)field.GetValue(maxStyleWord));
                //Debug.Log(Mathf.Max((float)field.GetValue(styleWord), (float)field.GetValue(maxStyleWord)));
                //Debug.Log("..");
                //if (float.IsNaN((float)field.GetValue(styleWord))){
                //    Debug.Log(0); Debug.Log(field.Name);
                //}
                //if (float.IsNaN((float)field.GetValue(maxStyleWord)))
                //{
                //    Debug.Log(1); Debug.Log(field.Name);
                //}
                //if (float.IsNaN(Mathf.Max((float)field.GetValue(styleWord), (float)field.GetValue(maxStyleWord))))
                //{
                //    Debug.Log(2); Debug.Log(field.Name);
                //}
            }
            //Debug.Log("*************************************************************");
            return newStyleWord;
        }

        public StyleWord GetNormilizedWord(StyleWord styleWord, StyleWord maxStyleWord)
        {
            StyleWord newStyleWord = new StyleWord();
            foreach (FieldInfo field in typeof(StyleWord).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                float div = (float)field.GetValue(maxStyleWord);
                div = (div != 0f? div:1f); // Division with 0 is NaN
                field.SetValue(newStyleWord, ((float)field.GetValue(styleWord) / div));
            }
            return newStyleWord;
        }

        public StyleWord GetDistanceBetweenWords(StyleWord[] styleWords)
        {
            StyleWord DisWord = styleWords[0];
            for (int i = 1; i < styleWords.Length; i++)
            {
                foreach (FieldInfo field in typeof(StyleWord).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    //Debug.Log((float)field.GetValue(DisWord) + " , " + (float)field.GetValue(styleWords[i]));
                    field.SetValue(DisWord, Mathf.Abs((float)field.GetValue(DisWord) - (float)field.GetValue(styleWords[i])));

                }
            }
            return DisWord;
        }

        public float GetSumOfVars(StyleWord styleWord)
        {
            float sum = 0;
            foreach (var field in typeof(StyleWord).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                sum += (float)field.GetValue(styleWord);
            }

            return sum;
        }

        public enum LMACompnents
        {
            Body = 0,
            Effort = 1,
            Shape = 2,
            Space = 3
        }

        public enum LMAOperationType
        {
            Sum = 0,
        }

        public float GetLMAComponent(LMACompnents lmaComponent,LMAOperationType operationType)
        {
            float result = 0;
            switch (lmaComponent)
            {
                case LMACompnents.Body:
                    float feetHipDistance = feetHipDistanceMax + feetHipDistanceMin + feetHipDistanceMean + feetHipDistanceStd;
                    float handsSholderDistance = handsShoulderDistanceMax + handsShoulderDistanceMin + handsShoulderDistanceMean + handsShoulderDistanceStd;
                    float handsDistance = handsDistanceMax + handsDistanceMin + handsDistanceMean + handsDistanceStd;
                    float handsHeadDistance = handsHeadDistanceMax + handsHeadDistanceMin + handsHeadDistanceMean + handsHeadDistanceStd;
                    float pelvisHeight = pelvisHeightMax + pelvisHeightMin + pelvisHeightMean + pelvisHeightStd;  
                    float hipGroundMinusFeetHipDistance = hipGroundMinusFeetHipMax + hipGroundMinusFeetHipMin + hipGroundMinusFeetHipMean + hipGroundMinusFeetHipStd;
                    float centroidHeight = centroidHeightMax + centroidHeightMin + centroidHeightMean + centroidHeightStd;
                    float centroidPelvisDistance = centroidPelvisDistanceMax + centroidPelvisDistanceMin + centroidPelvisDistanceMean + centroidPelvisDistanceStd;
                    float gaitSize = gaitSizeMax + gaitSizeMin + gaitSizeMean + gaitSizeStd;
                    float body = feetHipDistance + handsSholderDistance + handsDistance + handsHeadDistance + pelvisHeight + hipGroundMinusFeetHipDistance + centroidHeight + centroidPelvisDistance + gaitSize;

                    result = body;
                    break;
                case LMACompnents.Effort:
                    float headOrientation = headOrientationMax + headOrientationMin + headOrientationMean + headOrientationStd;
                    float decelerationPeaks = decelerationPeaksNo;
                    float hipVelocity = hipVelocityMax + hipVelocityMin + hipVelocityStd;
                    float handsVelocity = handsVelocityMax + handsVelocityMin + handsVelocityStd;
                    float feetVelocity = feetVelocityMax + feetVelocityMin + feetVelocityStd;
                    float hipAcceleration = hipAccelerationMax + hipAccelerationStd;
                    float handsAcceleration = handsAccelerationMax + handsAccelerationStd;
                    float feetAcceleration = feetAccelerationMax + feetAccelerationStd;
                    float jerk = jerkAccelerationMax + jerkAccelerationStd;
                    float effort = headOrientation + decelerationPeaks + hipVelocity + handsVelocity + feetVelocity + hipAcceleration + handsAcceleration + feetAcceleration + jerk;

                    result = effort;
                    break;
                case LMACompnents.Shape:
                    float volume = volumeMax + volumeMin + volumeMean + volumeStd;
                    float volumeUpperBody = volumeUpperBodyMax + volumeUpperBodyMin + volumeUpperBodyMean + volumeUpperBodyStd;
                    float volumeLowerBody = volumeLowerBodyMax + volumeLowerBodyMin + volumeLowerBodyMean + volumeLowerBodyStd;
                    float volumeLeftSide = volumeLeftSideMax + volumeLeftSideMin + volumeLeftSideMean + volumeLeftSideStd;
                    float volumeRightSide = volumeRightSideMax + volumeRightSideMin + volumeRightSideMean + volumeRightSideStd;
                    float torsoHeight = torsoHeightMax + torsoHeightMin + torsoHeightMean + torsoHeightStd;
                    float handsLevel = handsLevelNo1 + handsLevelNo2 + handsLevelNo3;
                    float shape = volume + volumeUpperBody + volumeLowerBody + volumeLeftSide + volumeRightSide + torsoHeight + handsLevel;

                    result = shape;
                    break;
                case LMACompnents.Space:
                    float totalDistance = totalDistanceNo;
                    float totalArea = totalAreaNo;
                    float space = totalDistance + totalArea;

                    result = space;
                    break;
            }

            return result;
        }
        
    }

    private List<Vector3> skeletonCentroid;

    public Skeleton()
    {
        joints = new List<Joint>();
        skeletonCentroid = new List<Vector3>();
        motionWords = new List<MotionWord>();
        styleWords = new List<StyleWord>();
        frameMotions = new List<MotionWord>();
        //motionWordStepCounter = motionWordWindowSize;
        //styleWordStepCounter = styleWordWindowSize;
    }

    public Skeleton(List<Joint> joints)
    {
        this.joints = joints;

        skeletonCentroid = new List<Vector3>();
        motionWords = new List<MotionWord>();
        styleWords = new List<StyleWord>();
        frameMotions = new List<MotionWord>();
        //motionWordStepCounter = motionWordWindowSize;
        //styleWordStepCounter = styleWordWindowSize;
    }

    /// <summary>
    /// Return joint with name 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Joint GetJointByName(JointName name)
    {
        foreach(Joint joint in joints)
        {
            if(joint.GetJointName() == name)
            {
                return joint;
            }
        }
        return null;
    }

    /// <summary>
    /// Add values of each joint into a frame value
    /// </summary>
    public void AutoAddFrameValuesForEachJoint(Transform callerTransform)
    {
        Vector3 centroidForThisFrame = Vector3.zero;
        int activeJoints = 0;
        foreach (Joint joint in joints)
        {
                Frame f = joint.AddFrame(callerTransform);

                centroidForThisFrame += f.position;
                activeJoints++;
        }

        // Calculate centroid
        centroidForThisFrame = centroidForThisFrame / activeJoints;
        skeletonCentroid.Add(centroidForThisFrame);

        AddFrameMotion();
    }

    /// <summary>
    /// Add motion word in motion words list
    /// </summary>
    public MotionWord AddMotionWord(int motionWordWindowSize)
    {
        MotionWord motionWord = new MotionWord();
        motionWord.joint = new List<Vector3[]>();

        foreach( Joint joint in joints)
        {
                // Get last frames per joint and get only rotations
                Frame[] framePerJoint = joint.GetLastFrames(motionWordWindowSize).ToArray();
                Vector3[] jointRotationByFrame = new Vector3[motionWordWindowSize];
                for (int i = 0; i < motionWordWindowSize; i++)
                {
                    jointRotationByFrame[i] = framePerJoint[i].rotation;
                }
                motionWord.joint.Add(jointRotationByFrame);
        }

        motionWords.Add(motionWord);

        return motionWord;
    }

    /// <summary>
    /// Add motion word from start frame(included) to end frame(included) in motion words list
    /// </summary>
    public MotionWord AddMotionWord(int startFrame, int endFrame)
    {
        int size = endFrame - startFrame + 1;
        MotionWord motionWord = new MotionWord();
        motionWord.joint = new List<Vector3[]>();
        //Debug.Log("startFrame: " + (selectedFrame - Mathf.FloorToInt(motionWordWindowSize / 2f)) + ", endFrame: " + (selectedFrame + Mathf.FloorToInt(motionWordWindowSize / 2f)));
        foreach (Joint joint in joints)
        {
                // Get last frames per joint and get only rotations
                Frame[] framePerJoint = joint.GetFramesRange(startFrame, endFrame).ToArray();
                Vector3[] jointRotationByFrame = new Vector3[size];
                for (int i = 0; i < size; i++)
                {
                    jointRotationByFrame[i] = framePerJoint[i].rotation;
                }
                motionWord.joint.Add(jointRotationByFrame);
        }

        motionWords.Add(motionWord);

        return motionWord;
    }

    /// <summary>
    /// Add frame motion in frame motion list
    /// </summary>
    public MotionWord AddFrameMotion()
    {
        MotionWord frameMotion = new MotionWord();
        frameMotion.joint = new List<Vector3[]>();

        foreach (Joint joint in joints)
        {
            // Get last frames per joint and get only rotations
            Frame[] framePerJoint = joint.GetLastFrames().ToArray();
            Vector3[] jointRotationByFrame = new Vector3[1];
            for (int i = 0; i < 1; i++)
            {
                jointRotationByFrame[i] = framePerJoint[i].rotation;
            }
            frameMotion.joint.Add(jointRotationByFrame);
        }

        frameMotions.Add(frameMotion);

        return frameMotion;
    }

    /// <summary>
    /// Add style word in style words list
    /// </summary>
    public StyleWord AddStyleWord(int styleWordWindowSize)
    {
        Dictionary<JointName, List<Frame>> jointsWithLastFrames = new Dictionary<JointName, List<Frame>>();
        foreach(Joint joint in joints)
        {
            jointsWithLastFrames.Add(joint.GetJointName(),joint.GetLastFrames(styleWordWindowSize));
        }

        // Feet to hips distance
        float[] f1 = new float[styleWordWindowSize];

        // Hands to shoulders distance
        float[] f2 = new float[styleWordWindowSize];

        // Right hand to left hand distance
        float[] f3 = new float[styleWordWindowSize];

        // Hands to head distance
        float[] f4 = new float[styleWordWindowSize];

        // Pelvis heigh
        float[] f5 = new float[styleWordWindowSize];

        // Distance of the hips to the ground, minus the distance of the feet to the hips
        float[] f6 = new float[styleWordWindowSize];

        // Distance between the ground and the centroid
        float[] f7 = new float[styleWordWindowSize];

        // Distance between the centroid and the pelvis (Centroid)
        float[] f8 = new float[styleWordWindowSize];

        // Distance of the right foot to the left (Gait)
        float[] f9 = new float[styleWordWindowSize];

        // Head orientation
        float[] f10 = new float[styleWordWindowSize];

        // Deceleration of motion
        float f11 = 0; // ? how to calculate this?

        // jointsWithLastFrames[JointName.Root] Velocity
        float[] f12 = new float[styleWordWindowSize];

        // The average velocity of both hands
        float[] f13 = new float[styleWordWindowSize];

        // The average velocity of both feet
        float[] f14 = new float[styleWordWindowSize];

        // jointsWithLastFrames[JointName.Root] acceleration
        float[] f15 = new float[styleWordWindowSize];

        // The average acceleration of both hands
        float[] f16 = new float[styleWordWindowSize];

        // The average acceleration of both feet
        float[] f17 = new float[styleWordWindowSize];

        // Changes of acceleration or force and it iscalculated by taking the derivative of the acceleration (Jerk)
        float[] f18 = new float[styleWordWindowSize];

        // Volume
        float[] f19 = new float[styleWordWindowSize];

        // Volume upper body
        float[] f20 = new float[styleWordWindowSize];

        // Volume lower body
        float[] f21 = new float[styleWordWindowSize];

        // Volume left side
        float[] f22 = new float[styleWordWindowSize];

        // Volume right side
        float[] f23 = new float[styleWordWindowSize];

        // Torso height
        float[] f24 = new float[styleWordWindowSize];

        // Hands Level Upper,Middle and Low
        Vector3 f25 = new Vector3();
        f25 = Vector3.zero;

        // Distance
        float f26 = 0;

        // Area
        float f27 = 0; // ? how to calculate this? Is not the same with distance?

        // Calculate f by frame
        for (int i = 0; i < styleWordWindowSize; i++)
        {
            // Feet to hips distance
            f1[i] = (Vector3.Distance(jointsWithLastFrames[JointName.EndSiteLeftToeBase][i].position, jointsWithLastFrames[JointName.LeftUpLeg][i].position) + Vector3.Distance(jointsWithLastFrames[JointName.EndSiteRightToeBase][i].position, jointsWithLastFrames[JointName.RightUpLeg][i].position)) / 2f;
            // Hands to shoulders distance
            f2[i] = (Vector3.Distance(jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].position, jointsWithLastFrames[JointName.LeftShoulder][i].position) + Vector3.Distance(jointsWithLastFrames[JointName.EndSiteRightHandMiddle][i].position, jointsWithLastFrames[JointName.RightShoulder][i].position)) / 2f;
            // Right hand to left hand distance
            f3[i] = Vector3.Distance(jointsWithLastFrames[JointName.EndSiteRightHandMiddle][i].position, jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].position);
            // Hands to head distance
            f4[i] = (Vector3.Distance(jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].position, jointsWithLastFrames[JointName.EndSiteHead][i].position) + Vector3.Distance(jointsWithLastFrames[JointName.EndSiteRightHandMiddle][i].position, jointsWithLastFrames[JointName.EndSiteHead][i].position)) / 2f;
            // Pelvis heigh
            f5[i] = jointsWithLastFrames[JointName.Root][i].position.y;
            // Distance of the hips to the ground, minus the distance of the feet to the hips
            f6[i] = ((jointsWithLastFrames[JointName.LeftUpLeg][i].position.y - Vector3.Distance(jointsWithLastFrames[JointName.EndSiteLeftToeBase][i].position, jointsWithLastFrames[JointName.LeftFoot][i].position)) + (jointsWithLastFrames[JointName.RightUpLeg][i].position.y - Vector3.Distance(jointsWithLastFrames[JointName.EndSiteRightToeBase][i].position, jointsWithLastFrames[JointName.RightUpLeg][i].position))) / 2f;
            // Distance between the ground and the centroid
            Vector3 centroid = skeletonCentroid[skeletonCentroid.Count - 1 - i];
            f7[i] = centroid.y;
            // Distance between the centroid and the pelvis (Centroid)
            f8[i] = Vector3.Distance(centroid, jointsWithLastFrames[JointName.Root][i].position);
            // Distance of the right foot to the left (Gait)
            f9[i] = Vector3.Distance(jointsWithLastFrames[JointName.EndSiteLeftToeBase][i].position, jointsWithLastFrames[JointName.EndSiteRightToeBase][i].position);
            // Deceleration of motion
            // ?

            if (i > 0)
            {
                // Head orientation
                Vector3 rootForward = (jointsWithLastFrames[JointName.Root][i].position - jointsWithLastFrames[JointName.Root][i - 1].position);
                Vector3 headOrientation = (jointsWithLastFrames[JointName.Head][i].rotation /* * Vector3.forward*/);
                f10[i] = Vector3.Angle(headOrientation, rootForward);

                // jointsWithLastFrames[JointName.Root] Velocity
                f12[i] = ((jointsWithLastFrames[JointName.Root][i].position - jointsWithLastFrames[JointName.Root][i-1].position) / (jointsWithLastFrames[JointName.Root][i].time - jointsWithLastFrames[JointName.Root][i-1].time)).magnitude;

                float leftHandVelocity = ((jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].position - jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i - 1].position) / (jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].time - jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i - 1].time)).magnitude;
                float rightHandVelocity = ((jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].position - jointsWithLastFrames[JointName.EndSiteRightHandMiddle][i - 1].position) / (jointsWithLastFrames[JointName.EndSiteRightHandMiddle][i].time - jointsWithLastFrames[JointName.EndSiteRightHandMiddle][i - 1].time)).magnitude;
                // The average velocity of both hands
                f13[i] = (leftHandVelocity + rightHandVelocity) /2f;

                float leftFootVelocity = ((jointsWithLastFrames[JointName.EndSiteLeftToeBase][i].position - jointsWithLastFrames[JointName.EndSiteLeftToeBase][i - 1].position) / (jointsWithLastFrames[JointName.EndSiteLeftToeBase][i].time - jointsWithLastFrames[JointName.EndSiteLeftToeBase][i - 1].time)).magnitude;
                float rightFootVelocity = ((jointsWithLastFrames[JointName.EndSiteRightToeBase][i].position - jointsWithLastFrames[JointName.EndSiteRightToeBase][i - 1].position) / (jointsWithLastFrames[JointName.EndSiteRightToeBase][i].time - jointsWithLastFrames[JointName.EndSiteRightToeBase][i - 1].time)).magnitude;
                // The average velocity of both feet
                f14[i] = (leftFootVelocity + rightFootVelocity) / 2f;
                // jointsWithLastFrames[JointName.Root] acceleration
                f15[i] = (f12[i] - f12[i - 1]) / (jointsWithLastFrames[JointName.Root][i].time - jointsWithLastFrames[JointName.Root][i - 1].time);
                // The average acceleration of both hands
                f16[i] = (f13[i] - f13[i - 1]) / (jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].time - jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i - 1].time);
                // The average acceleration of both feet
                f17[i] = (f14[i] - f14[i - 1]) / (jointsWithLastFrames[JointName.EndSiteLeftToeBase][i].time - jointsWithLastFrames[JointName.EndSiteLeftToeBase][i - 1].time);
                // Changes of acceleration or force and it iscalculated by taking the derivative of the acceleration (Jerk)
                f18[i] = (f15[i] - f15[i - 1]) / (jointsWithLastFrames[JointName.Root][i].time - jointsWithLastFrames[JointName.Root][i - 1].time);

                // Distance
                f26 += Vector3.Distance(jointsWithLastFrames[JointName.Root][i].position, jointsWithLastFrames[JointName.Root][i - 1].position);
            }

            // List for volumes
            #region List for volumes
            // Head
            List<Vector3> headList = new List<Vector3>();
            headList.Add(jointsWithLastFrames[JointName.EndSiteHead][i].position);
            headList.Add(jointsWithLastFrames[JointName.Head][i].position);
            // Spine
            List<Vector3> spineList = new List<Vector3>();
            spineList.Add(jointsWithLastFrames[JointName.Neck][i].position);
            spineList.Add(jointsWithLastFrames[JointName.Spine3][i].position);
            spineList.Add(jointsWithLastFrames[JointName.Spine2][i].position);
            spineList.Add(jointsWithLastFrames[JointName.Spine1][i].position);
            spineList.Add(jointsWithLastFrames[JointName.Spine][i].position);
            spineList.Add(jointsWithLastFrames[JointName.Root][i].position);
            // Left Hand
            List<Vector3> leftHandList = new List<Vector3>();
            leftHandList.Add(jointsWithLastFrames[JointName.LeftShoulder][i].position);
            leftHandList.Add(jointsWithLastFrames[JointName.LeftArm][i].position);
            leftHandList.Add(jointsWithLastFrames[JointName.LeftForeArm][i].position);
            leftHandList.Add(jointsWithLastFrames[JointName.LeftHand][i].position);
            leftHandList.Add(jointsWithLastFrames[JointName.LeftHandMiddle1][i].position);
            leftHandList.Add(jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].position);
            // Right Hand
            List<Vector3> rightHandList = new List<Vector3>();
            rightHandList.Add(jointsWithLastFrames[JointName.RightShoulder][i].position);
            rightHandList.Add(jointsWithLastFrames[JointName.RightArm][i].position);
            rightHandList.Add(jointsWithLastFrames[JointName.RightForeArm][i].position);
            rightHandList.Add(jointsWithLastFrames[JointName.RightHand][i].position);
            rightHandList.Add(jointsWithLastFrames[JointName.RightHandMiddle1][i].position);
            rightHandList.Add(jointsWithLastFrames[JointName.EndSiteRightHandMiddle][i].position);
            // Left Foot
            List<Vector3> leftFootList = new List<Vector3>();
            leftFootList.Add(jointsWithLastFrames[JointName.LeftUpLeg][i].position);
            leftFootList.Add(jointsWithLastFrames[JointName.LeftLeg][i].position);
            leftFootList.Add(jointsWithLastFrames[JointName.LeftFoot][i].position);
            leftFootList.Add(jointsWithLastFrames[JointName.LeftToeBase][i].position);
            leftFootList.Add(jointsWithLastFrames[JointName.EndSiteLeftToeBase][i].position);
            // Right Foot
            List<Vector3> rightFootList = new List<Vector3>();
            rightFootList.Add(jointsWithLastFrames[JointName.RightUpLeg][i].position);
            rightFootList.Add(jointsWithLastFrames[JointName.RightLeg][i].position);
            rightFootList.Add(jointsWithLastFrames[JointName.RightFoot][i].position);
            rightFootList.Add(jointsWithLastFrames[JointName.RightToeBase][i].position);
            rightFootList.Add(jointsWithLastFrames[JointName.EndSiteRightToeBase][i].position);
            #endregion

            // Volume  
            #region Valumes 
            List<Vector3> pointsForVolume = new List<Vector3>();
            pointsForVolume.AddRange(headList);
            pointsForVolume.AddRange(spineList);
            pointsForVolume.AddRange(leftHandList);
            pointsForVolume.AddRange(rightHandList);
            pointsForVolume.AddRange(leftFootList);
            pointsForVolume.AddRange(rightFootList);

            Mesh mesh = MathHelper.GenerateMesh(pointsForVolume.ToArray());
            float volume = MathHelper.VolumeOfMesh(mesh);
            f19[i] = volume;

            // Volume upper body
            List<Vector3> pointsForUpperBodyVolume = new List<Vector3>();
            pointsForUpperBodyVolume.AddRange(headList);
            pointsForUpperBodyVolume.AddRange(spineList);
            pointsForUpperBodyVolume.AddRange(leftHandList);
            pointsForUpperBodyVolume.AddRange(rightHandList);

            Mesh meshUpperBody = MathHelper.GenerateMesh(pointsForUpperBodyVolume.ToArray());
            float volumeUpperBody = MathHelper.VolumeOfMesh(meshUpperBody);
            f20[i] = volumeUpperBody;

            // Volume lower body
            List<Vector3> pointsForLowerBodyVolume = new List<Vector3>();
            pointsForVolume.Add(jointsWithLastFrames[JointName.Root][i].position);
            pointsForVolume.AddRange(leftFootList);
            pointsForVolume.AddRange(rightFootList);

            Mesh meshLowerBody = MathHelper.GenerateMesh(pointsForLowerBodyVolume.ToArray());
            float volumeLowerBody = MathHelper.VolumeOfMesh(meshLowerBody);
            f21[i] = volumeLowerBody;

            // Volume left side
            List<Vector3> pointsForLeftSideVolume = new List<Vector3>();
            pointsForLeftSideVolume.AddRange(spineList);
            pointsForLeftSideVolume.AddRange(leftHandList);
            pointsForLeftSideVolume.AddRange(leftFootList);

            Mesh meshLeftSide = MathHelper.GenerateMesh(pointsForLeftSideVolume.ToArray());
            float volumeLeftSide = MathHelper.VolumeOfMesh(meshLeftSide);
            f22[i] = volumeLeftSide;

            // Volume Right side
            List<Vector3> pointsForRightSideVolume = new List<Vector3>();
            pointsForRightSideVolume.AddRange(spineList);
            pointsForRightSideVolume.AddRange(rightHandList);
            pointsForRightSideVolume.AddRange(rightFootList);
            
            Mesh meshRightSide = MathHelper.GenerateMesh(pointsForRightSideVolume.ToArray());
            float volumeRightSide = MathHelper.VolumeOfMesh(meshRightSide);
            f23[i] = volumeRightSide;
            #endregion

            // Torso height
            f24[i] = Vector3.Distance(jointsWithLastFrames[JointName.EndSiteHead][i].position, jointsWithLastFrames[JointName.Root][i].position);

            // Hands level
            #region Hands Level
            float leftHandY = jointsWithLastFrames[JointName.LeftHand][i].position.y;
            float rightHandY = jointsWithLastFrames[JointName.RightHand][i].position.y;
            float headY = jointsWithLastFrames[JointName.EndSiteHead][i].position.y;
            float spineY = jointsWithLastFrames[JointName.Spine][i].position.y;
            // Left Hand
            if (leftHandY > headY)
            {
                // Upper level
                f25.x++;
            }
            else if (leftHandY < spineY)
            {
                // Lower level
                f25.z++;
            }
            else
            {
                // Middle level
                f25.y++;
            }
            // Right Hand
            if (rightHandY > headY)
            {
                // Upper level
                f25.x++;
            }
            else if (rightHandY < spineY)
            {
                // Lower level
                f25.z++;
            }
            else
            {
                // Middle level
                f25.y++;
            }
            #endregion

        } // End Calculation by frame

        // Set first position of array equals with second position not to effect maximum and minimun
        f10[0] = f10[1];
        f12[0] = f12[1];
        f13[0] = f13[1];
        f14[0] = f14[1];
        f15[0] = f15[1];
        f16[0] = f16[1];
        f17[0] = f17[1];
        f18[0] = f18[1];

        // Create and add Style word
        #region Create style word
        StyleWord newStyleWord = new StyleWord();
        newStyleWord.feetHipDistanceMax = GetMaximumOfArray(f1);
        newStyleWord.feetHipDistanceMin = GetMinimumOfArray(f1);
        newStyleWord.feetHipDistanceMean = GetMeanOfArray(f1);
        newStyleWord.feetHipDistanceStd = GetStdOfArray(f1);

        newStyleWord.handsShoulderDistanceMax = GetMaximumOfArray(f2);
        newStyleWord.handsShoulderDistanceMin = GetMinimumOfArray(f2);
        newStyleWord.handsShoulderDistanceMean = GetMeanOfArray(f2);
        newStyleWord.handsShoulderDistanceStd = GetStdOfArray(f2);

        newStyleWord.handsDistanceMax = GetMaximumOfArray(f3);
        newStyleWord.handsDistanceMin = GetMinimumOfArray(f3);
        newStyleWord.handsDistanceMean = GetMeanOfArray(f3);
        newStyleWord.handsDistanceStd = GetStdOfArray(f3);

        newStyleWord.handsHeadDistanceMax = GetMaximumOfArray(f4);
        newStyleWord.handsHeadDistanceMin = GetMinimumOfArray(f4);
        newStyleWord.handsHeadDistanceMean = GetMeanOfArray(f4);
        newStyleWord.handsHeadDistanceStd = GetStdOfArray(f4);

        newStyleWord.pelvisHeightMax = GetMaximumOfArray(f5);
        newStyleWord.pelvisHeightMin = GetMinimumOfArray(f5);
        newStyleWord.pelvisHeightMean = GetMeanOfArray(f5);
        newStyleWord.pelvisHeightStd = GetStdOfArray(f5);

        newStyleWord.hipGroundMinusFeetHipMax = GetMaximumOfArray(f6);
        newStyleWord.hipGroundMinusFeetHipMin = GetMinimumOfArray(f6);
        newStyleWord.hipGroundMinusFeetHipMean = GetMeanOfArray(f6);
        newStyleWord.hipGroundMinusFeetHipStd = GetStdOfArray(f6);

        newStyleWord.centroidHeightMax = GetMaximumOfArray(f7);
        newStyleWord.centroidHeightMin = GetMinimumOfArray(f7);
        newStyleWord.centroidHeightMean = GetMeanOfArray(f7);
        newStyleWord.centroidHeightStd = GetStdOfArray(f7);

        newStyleWord.centroidPelvisDistanceMax = GetMaximumOfArray(f8);
        newStyleWord.centroidPelvisDistanceMin = GetMinimumOfArray(f8);
        newStyleWord.centroidPelvisDistanceMean = GetMeanOfArray(f8);
        newStyleWord.centroidPelvisDistanceStd = GetStdOfArray(f8);

        newStyleWord.gaitSizeMax = GetMaximumOfArray(f9);
        newStyleWord.gaitSizeMin = GetMinimumOfArray(f9);
        newStyleWord.gaitSizeMean = GetMeanOfArray(f9);
        newStyleWord.gaitSizeStd = GetStdOfArray(f9);

        newStyleWord.headOrientationMax = GetMaximumOfArray(f10);
        newStyleWord.headOrientationMin = GetMinimumOfArray(f10);
        newStyleWord.headOrientationMean = GetMeanOfArray(f10);
        newStyleWord.headOrientationMean = GetStdOfArray(f10);

        newStyleWord.decelerationPeaksNo = f11;

        newStyleWord.hipVelocityMax = GetMaximumOfArray(f12);
        newStyleWord.hipVelocityMin = GetMinimumOfArray(f12);
        newStyleWord.hipVelocityStd = GetStdOfArray(f12);

        newStyleWord.handsVelocityMax = GetMaximumOfArray(f13);
        newStyleWord.handsVelocityMin = GetMinimumOfArray(f13);
        newStyleWord.handsVelocityStd = GetStdOfArray(f13);

        newStyleWord.feetVelocityMax = GetMaximumOfArray(f14);
        newStyleWord.feetVelocityMin = GetMinimumOfArray(f14);
        newStyleWord.feetVelocityStd = GetStdOfArray(f14);

        newStyleWord.hipAccelerationMax = GetMaximumOfArray(f15);
        newStyleWord.hipAccelerationStd = GetStdOfArray(f15);

        newStyleWord.handsAccelerationMax = GetMaximumOfArray(f16);
        newStyleWord.handsAccelerationStd = GetStdOfArray(f16);

        newStyleWord.feetAccelerationMax = GetMaximumOfArray(f17);
        newStyleWord.feetAccelerationStd = GetStdOfArray(f17);

        newStyleWord.jerkAccelerationMax = GetMaximumOfArray(f18);
        newStyleWord.jerkAccelerationStd = GetStdOfArray(f18);

        newStyleWord.volumeMax = GetMaximumOfArray(f19);
        newStyleWord.volumeMin = GetMinimumOfArray(f19);
        newStyleWord.volumeMean = GetMeanOfArray(f19);
        newStyleWord.volumeStd = GetStdOfArray(f19);

        newStyleWord.volumeUpperBodyMax = GetMaximumOfArray(f20);
        newStyleWord.volumeUpperBodyMin = GetMinimumOfArray(f20);
        newStyleWord.volumeUpperBodyMean = GetMeanOfArray(f20);
        newStyleWord.volumeUpperBodyStd = GetStdOfArray(f20);

        newStyleWord.volumeLowerBodyMax = GetMaximumOfArray(f21);
        newStyleWord.volumeLowerBodyMin = GetMinimumOfArray(f21);
        newStyleWord.volumeLowerBodyMean = GetMeanOfArray(f21);
        newStyleWord.volumeLowerBodyStd = GetStdOfArray(f21);

        newStyleWord.volumeLeftSideMax = GetMaximumOfArray(f22);
        newStyleWord.volumeLeftSideMin = GetMinimumOfArray(f22);
        newStyleWord.volumeLeftSideMean = GetMeanOfArray(f22);
        newStyleWord.volumeLeftSideStd = GetStdOfArray(f22);

        newStyleWord.volumeRightSideMax = GetMaximumOfArray(f23);
        newStyleWord.volumeRightSideMin = GetMinimumOfArray(f23);
        newStyleWord.volumeRightSideMean = GetMeanOfArray(f23);
        newStyleWord.volumeRightSideStd = GetStdOfArray(f23);

        newStyleWord.torsoHeightMax = GetMaximumOfArray(f24);
        newStyleWord.torsoHeightMin = GetMinimumOfArray(f24);
        newStyleWord.torsoHeightMean = GetMeanOfArray(f24);
        newStyleWord.torsoHeightStd = GetStdOfArray(f24);

        newStyleWord.handsLevelNo1 = f25.x;
        newStyleWord.handsLevelNo2 = f25.y;
        newStyleWord.handsLevelNo3 = f25.z;

        newStyleWord.totalDistanceNo = f26;

        newStyleWord.totalAreaNo = f27;

        #endregion

        styleWords.Add(newStyleWord);

        return newStyleWord;
    }

    /// <summary>
    /// Add style word in style words list
    /// </summary>
    public StyleWord AddStyleWord(int startFrame, int endFrame)
    {
        int styleWordWindowSize = endFrame - startFrame + 1;

        Dictionary<JointName, List<Frame>> jointsWithLastFrames = new Dictionary<JointName, List<Frame>>();
        foreach (Joint joint in joints)
        {
            jointsWithLastFrames.Add(joint.GetJointName(), joint.GetFramesRange(startFrame, endFrame));
        }

        // Feet to hips distance
        float[] f1 = new float[styleWordWindowSize];

        // Hands to shoulders distance
        float[] f2 = new float[styleWordWindowSize];

        // Right hand to left hand distance
        float[] f3 = new float[styleWordWindowSize];

        // Hands to head distance
        float[] f4 = new float[styleWordWindowSize];

        // Pelvis heigh
        float[] f5 = new float[styleWordWindowSize];

        // Distance of the hips to the ground, minus the distance of the feet to the hips
        float[] f6 = new float[styleWordWindowSize];

        // Distance between the ground and the centroid
        float[] f7 = new float[styleWordWindowSize];

        // Distance between the centroid and the pelvis (Centroid)
        float[] f8 = new float[styleWordWindowSize];

        // Distance of the right foot to the left (Gait)
        float[] f9 = new float[styleWordWindowSize];

        // Head orientation
        float[] f10 = new float[styleWordWindowSize];

        // Deceleration of motion
        float f11 = 0; // ? how to calculate this?

        // jointsWithLastFrames[JointName.Root] Velocity
        float[] f12 = new float[styleWordWindowSize];

        // The average velocity of both hands
        float[] f13 = new float[styleWordWindowSize];

        // The average velocity of both feet
        float[] f14 = new float[styleWordWindowSize];

        // jointsWithLastFrames[JointName.Root] acceleration
        float[] f15 = new float[styleWordWindowSize];

        // The average acceleration of both hands
        float[] f16 = new float[styleWordWindowSize];

        // The average acceleration of both feet
        float[] f17 = new float[styleWordWindowSize];

        // Changes of acceleration or force and it iscalculated by taking the derivative of the acceleration (Jerk)
        float[] f18 = new float[styleWordWindowSize];

        // Volume
        float[] f19 = new float[styleWordWindowSize];

        // Volume upper body
        float[] f20 = new float[styleWordWindowSize];

        // Volume lower body
        float[] f21 = new float[styleWordWindowSize];

        // Volume left side
        float[] f22 = new float[styleWordWindowSize];

        // Volume right side
        float[] f23 = new float[styleWordWindowSize];

        // Torso height
        float[] f24 = new float[styleWordWindowSize];

        // Hands Level Upper,Middle and Low
        Vector3 f25 = new Vector3();
        f25 = Vector3.zero;

        // Distance
        float f26 = 0;

        // Area
        float f27 = 0; // ? how to calculate this? Is not the same with distance?

        // Calculate f by frame
        for (int i = 0; i < styleWordWindowSize; i++)
        {
            // Feet to hips distance
            f1[i] = (Vector3.Distance(jointsWithLastFrames[JointName.EndSiteLeftToeBase][i].position, jointsWithLastFrames[JointName.LeftUpLeg][i].position) + Vector3.Distance(jointsWithLastFrames[JointName.EndSiteRightToeBase][i].position, jointsWithLastFrames[JointName.RightUpLeg][i].position)) / 2f;
            // Hands to shoulders distance
            f2[i] = (Vector3.Distance(jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].position, jointsWithLastFrames[JointName.LeftShoulder][i].position) + Vector3.Distance(jointsWithLastFrames[JointName.EndSiteRightHandMiddle][i].position, jointsWithLastFrames[JointName.RightShoulder][i].position)) / 2f;
            // Right hand to left hand distance
            f3[i] = Vector3.Distance(jointsWithLastFrames[JointName.EndSiteRightHandMiddle][i].position, jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].position);
            // Hands to head distance
            f4[i] = (Vector3.Distance(jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].position, jointsWithLastFrames[JointName.EndSiteHead][i].position) + Vector3.Distance(jointsWithLastFrames[JointName.EndSiteRightHandMiddle][i].position, jointsWithLastFrames[JointName.EndSiteHead][i].position)) / 2f;
            // Pelvis heigh
            f5[i] = jointsWithLastFrames[JointName.Root][i].position.y;
            // Distance of the hips to the ground, minus the distance of the feet to the hips
            f6[i] = ((jointsWithLastFrames[JointName.LeftUpLeg][i].position.y - Vector3.Distance(jointsWithLastFrames[JointName.EndSiteLeftToeBase][i].position, jointsWithLastFrames[JointName.LeftFoot][i].position)) + (jointsWithLastFrames[JointName.RightUpLeg][i].position.y - Vector3.Distance(jointsWithLastFrames[JointName.EndSiteRightToeBase][i].position, jointsWithLastFrames[JointName.RightUpLeg][i].position))) / 2f;
            // Distance between the ground and the centroid
            Vector3 centroid = skeletonCentroid[skeletonCentroid.Count - 1 - i];
            f7[i] = centroid.y;
            // Distance between the centroid and the pelvis (Centroid)
            f8[i] = Vector3.Distance(centroid, jointsWithLastFrames[JointName.Root][i].position);
            // Distance of the right foot to the left (Gait)
            f9[i] = Vector3.Distance(jointsWithLastFrames[JointName.EndSiteLeftToeBase][i].position, jointsWithLastFrames[JointName.EndSiteRightToeBase][i].position);
            // Deceleration of motion
            // ?

            if (i > 0)
            {
                // Head orientation
                Vector3 rootForward = (jointsWithLastFrames[JointName.Root][i].position - jointsWithLastFrames[JointName.Root][i - 1].position);
                Vector3 headOrientation = (jointsWithLastFrames[JointName.Head][i].rotation /* * Vector3.forward*/);
                f10[i] = Vector3.Angle(headOrientation, rootForward);

                // jointsWithLastFrames[JointName.Root] Velocity
                f12[i] = ((jointsWithLastFrames[JointName.Root][i].position - jointsWithLastFrames[JointName.Root][i - 1].position) / (jointsWithLastFrames[JointName.Root][i].time - jointsWithLastFrames[JointName.Root][i - 1].time)).magnitude;

                float leftHandVelocity = ((jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].position - jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i - 1].position) / (jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].time - jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i - 1].time)).magnitude;
                float rightHandVelocity = ((jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].position - jointsWithLastFrames[JointName.EndSiteRightHandMiddle][i - 1].position) / (jointsWithLastFrames[JointName.EndSiteRightHandMiddle][i].time - jointsWithLastFrames[JointName.EndSiteRightHandMiddle][i - 1].time)).magnitude;
                // The average velocity of both hands
                f13[i] = (leftHandVelocity + rightHandVelocity) / 2f;

                float leftFootVelocity = ((jointsWithLastFrames[JointName.EndSiteLeftToeBase][i].position - jointsWithLastFrames[JointName.EndSiteLeftToeBase][i - 1].position) / (jointsWithLastFrames[JointName.EndSiteLeftToeBase][i].time - jointsWithLastFrames[JointName.EndSiteLeftToeBase][i - 1].time)).magnitude;
                float rightFootVelocity = ((jointsWithLastFrames[JointName.EndSiteRightToeBase][i].position - jointsWithLastFrames[JointName.EndSiteRightToeBase][i - 1].position) / (jointsWithLastFrames[JointName.EndSiteRightToeBase][i].time - jointsWithLastFrames[JointName.EndSiteRightToeBase][i - 1].time)).magnitude;
                // The average velocity of both feet
                f14[i] = (leftFootVelocity + rightFootVelocity) / 2f;
                // jointsWithLastFrames[JointName.Root] acceleration
                f15[i] = (f12[i] - f12[i - 1]) / (jointsWithLastFrames[JointName.Root][i].time - jointsWithLastFrames[JointName.Root][i - 1].time);
                // The average acceleration of both hands
                f16[i] = (f13[i] - f13[i - 1]) / (jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].time - jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i - 1].time);
                // The average acceleration of both feet
                f17[i] = (f14[i] - f14[i - 1]) / (jointsWithLastFrames[JointName.EndSiteLeftToeBase][i].time - jointsWithLastFrames[JointName.EndSiteLeftToeBase][i - 1].time);
                // Changes of acceleration or force and it iscalculated by taking the derivative of the acceleration (Jerk)
                f18[i] = (f15[i] - f15[i - 1]) / (jointsWithLastFrames[JointName.Root][i].time - jointsWithLastFrames[JointName.Root][i - 1].time);

                // Distance
                f26 += Vector3.Distance(jointsWithLastFrames[JointName.Root][i].position, jointsWithLastFrames[JointName.Root][i - 1].position);
            }

            // List for volumes
            #region List for volumes
            // Head
            List<Vector3> headList = new List<Vector3>();
            headList.Add(jointsWithLastFrames[JointName.EndSiteHead][i].position);
            headList.Add(jointsWithLastFrames[JointName.Head][i].position);
            // Spine
            List<Vector3> spineList = new List<Vector3>();
            spineList.Add(jointsWithLastFrames[JointName.Neck][i].position);
            spineList.Add(jointsWithLastFrames[JointName.Spine3][i].position);
            spineList.Add(jointsWithLastFrames[JointName.Spine2][i].position);
            spineList.Add(jointsWithLastFrames[JointName.Spine1][i].position);
            spineList.Add(jointsWithLastFrames[JointName.Spine][i].position);
            spineList.Add(jointsWithLastFrames[JointName.Root][i].position);
            // Left Hand
            List<Vector3> leftHandList = new List<Vector3>();
            leftHandList.Add(jointsWithLastFrames[JointName.LeftShoulder][i].position);
            leftHandList.Add(jointsWithLastFrames[JointName.LeftArm][i].position);
            leftHandList.Add(jointsWithLastFrames[JointName.LeftForeArm][i].position);
            leftHandList.Add(jointsWithLastFrames[JointName.LeftHand][i].position);
            leftHandList.Add(jointsWithLastFrames[JointName.LeftHandMiddle1][i].position);
            leftHandList.Add(jointsWithLastFrames[JointName.EndSiteLeftHandMiddle][i].position);
            // Right Hand
            List<Vector3> rightHandList = new List<Vector3>();
            rightHandList.Add(jointsWithLastFrames[JointName.RightShoulder][i].position);
            rightHandList.Add(jointsWithLastFrames[JointName.RightArm][i].position);
            rightHandList.Add(jointsWithLastFrames[JointName.RightForeArm][i].position);
            rightHandList.Add(jointsWithLastFrames[JointName.RightHand][i].position);
            rightHandList.Add(jointsWithLastFrames[JointName.RightHandMiddle1][i].position);
            rightHandList.Add(jointsWithLastFrames[JointName.EndSiteRightHandMiddle][i].position);
            // Left Foot
            List<Vector3> leftFootList = new List<Vector3>();
            leftFootList.Add(jointsWithLastFrames[JointName.LeftUpLeg][i].position);
            leftFootList.Add(jointsWithLastFrames[JointName.LeftLeg][i].position);
            leftFootList.Add(jointsWithLastFrames[JointName.LeftFoot][i].position);
            leftFootList.Add(jointsWithLastFrames[JointName.LeftToeBase][i].position);
            leftFootList.Add(jointsWithLastFrames[JointName.EndSiteLeftToeBase][i].position);
            // Right Foot
            List<Vector3> rightFootList = new List<Vector3>();
            rightFootList.Add(jointsWithLastFrames[JointName.RightUpLeg][i].position);
            rightFootList.Add(jointsWithLastFrames[JointName.RightLeg][i].position);
            rightFootList.Add(jointsWithLastFrames[JointName.RightFoot][i].position);
            rightFootList.Add(jointsWithLastFrames[JointName.RightToeBase][i].position);
            rightFootList.Add(jointsWithLastFrames[JointName.EndSiteRightToeBase][i].position);
            #endregion

            // Volume  
            #region Valumes 
            List<Vector3> pointsForVolume = new List<Vector3>();
            pointsForVolume.AddRange(headList);
            pointsForVolume.AddRange(spineList);
            pointsForVolume.AddRange(leftHandList);
            pointsForVolume.AddRange(rightHandList);
            pointsForVolume.AddRange(leftFootList);
            pointsForVolume.AddRange(rightFootList);

            Mesh mesh = MathHelper.GenerateMesh(pointsForVolume.ToArray());
            float volume = MathHelper.VolumeOfMesh(mesh);
            f19[i] = volume;

            // Volume upper body
            List<Vector3> pointsForUpperBodyVolume = new List<Vector3>();
            pointsForUpperBodyVolume.AddRange(headList);
            pointsForUpperBodyVolume.AddRange(spineList);
            pointsForUpperBodyVolume.AddRange(leftHandList);
            pointsForUpperBodyVolume.AddRange(rightHandList);

            Mesh meshUpperBody = MathHelper.GenerateMesh(pointsForUpperBodyVolume.ToArray());
            float volumeUpperBody = MathHelper.VolumeOfMesh(meshUpperBody);
            f20[i] = volumeUpperBody;

            // Volume lower body
            List<Vector3> pointsForLowerBodyVolume = new List<Vector3>();
            pointsForVolume.Add(jointsWithLastFrames[JointName.Root][i].position);
            pointsForVolume.AddRange(leftFootList);
            pointsForVolume.AddRange(rightFootList);

            Mesh meshLowerBody = MathHelper.GenerateMesh(pointsForLowerBodyVolume.ToArray());
            float volumeLowerBody = MathHelper.VolumeOfMesh(meshLowerBody);
            f21[i] = volumeLowerBody;

            // Volume left side
            List<Vector3> pointsForLeftSideVolume = new List<Vector3>();
            pointsForLeftSideVolume.AddRange(spineList);
            pointsForLeftSideVolume.AddRange(leftHandList);
            pointsForLeftSideVolume.AddRange(leftFootList);

            Mesh meshLeftSide = MathHelper.GenerateMesh(pointsForLeftSideVolume.ToArray());
            float volumeLeftSide = MathHelper.VolumeOfMesh(meshLeftSide);
            f22[i] = volumeLeftSide;

            // Volume Right side
            List<Vector3> pointsForRightSideVolume = new List<Vector3>();
            pointsForRightSideVolume.AddRange(spineList);
            pointsForRightSideVolume.AddRange(rightHandList);
            pointsForRightSideVolume.AddRange(rightFootList);

            Mesh meshRightSide = MathHelper.GenerateMesh(pointsForRightSideVolume.ToArray());
            float volumeRightSide = MathHelper.VolumeOfMesh(meshRightSide);
            f23[i] = volumeRightSide;
            #endregion

            // Torso height
            f24[i] = Vector3.Distance(jointsWithLastFrames[JointName.EndSiteHead][i].position, jointsWithLastFrames[JointName.Root][i].position);

            // Hands level
            #region Hands Level
            float leftHandY = jointsWithLastFrames[JointName.LeftHand][i].position.y;
            float rightHandY = jointsWithLastFrames[JointName.RightHand][i].position.y;
            float headY = jointsWithLastFrames[JointName.EndSiteHead][i].position.y;
            float spineY = jointsWithLastFrames[JointName.Spine][i].position.y;
            // Left Hand
            if (leftHandY > headY)
            {
                // Upper level
                f25.x++;
            }
            else if (leftHandY < spineY)
            {
                // Lower level
                f25.z++;
            }
            else
            {
                // Middle level
                f25.y++;
            }
            // Right Hand
            if (rightHandY > headY)
            {
                // Upper level
                f25.x++;
            }
            else if (rightHandY < spineY)
            {
                // Lower level
                f25.z++;
            }
            else
            {
                // Middle level
                f25.y++;
            }
            #endregion

        } // End Calculation by frame

        // Set first position of array equals with second position not to effect maximum and minimun
        f10[0] = f10[1];
        f12[0] = f12[1];
        f13[0] = f13[1];
        f14[0] = f14[1];
        f15[0] = f15[1];
        f16[0] = f16[1];
        f17[0] = f17[1];
        f18[0] = f18[1];

        // Create and add Style word
        #region Create style word
        StyleWord newStyleWord = new StyleWord();
        newStyleWord.feetHipDistanceMax = GetMaximumOfArray(f1);
        newStyleWord.feetHipDistanceMin = GetMinimumOfArray(f1);
        newStyleWord.feetHipDistanceMean = GetMeanOfArray(f1);
        newStyleWord.feetHipDistanceStd = GetStdOfArray(f1);

        newStyleWord.handsShoulderDistanceMax = GetMaximumOfArray(f2);
        newStyleWord.handsShoulderDistanceMin = GetMinimumOfArray(f2);
        newStyleWord.handsShoulderDistanceMean = GetMeanOfArray(f2);
        newStyleWord.handsShoulderDistanceStd = GetStdOfArray(f2);

        newStyleWord.handsDistanceMax = GetMaximumOfArray(f3);
        newStyleWord.handsDistanceMin = GetMinimumOfArray(f3);
        newStyleWord.handsDistanceMean = GetMeanOfArray(f3);
        newStyleWord.handsDistanceStd = GetStdOfArray(f3);

        newStyleWord.handsHeadDistanceMax = GetMaximumOfArray(f4);
        newStyleWord.handsHeadDistanceMin = GetMinimumOfArray(f4);
        newStyleWord.handsHeadDistanceMean = GetMeanOfArray(f4);
        newStyleWord.handsHeadDistanceStd = GetStdOfArray(f4);

        newStyleWord.pelvisHeightMax = GetMaximumOfArray(f5);
        newStyleWord.pelvisHeightMin = GetMinimumOfArray(f5);
        newStyleWord.pelvisHeightMean = GetMeanOfArray(f5);
        newStyleWord.pelvisHeightStd = GetStdOfArray(f5);

        newStyleWord.hipGroundMinusFeetHipMax = GetMaximumOfArray(f6);
        newStyleWord.hipGroundMinusFeetHipMin = GetMinimumOfArray(f6);
        newStyleWord.hipGroundMinusFeetHipMean = GetMeanOfArray(f6);
        newStyleWord.hipGroundMinusFeetHipStd = GetStdOfArray(f6);

        newStyleWord.centroidHeightMax = GetMaximumOfArray(f7);
        newStyleWord.centroidHeightMin = GetMinimumOfArray(f7);
        newStyleWord.centroidHeightMean = GetMeanOfArray(f7);
        newStyleWord.centroidHeightStd = GetStdOfArray(f7);

        newStyleWord.centroidPelvisDistanceMax = GetMaximumOfArray(f8);
        newStyleWord.centroidPelvisDistanceMin = GetMinimumOfArray(f8);
        newStyleWord.centroidPelvisDistanceMean = GetMeanOfArray(f8);
        newStyleWord.centroidPelvisDistanceStd = GetStdOfArray(f8);

        newStyleWord.gaitSizeMax = GetMaximumOfArray(f9);
        newStyleWord.gaitSizeMin = GetMinimumOfArray(f9);
        newStyleWord.gaitSizeMean = GetMeanOfArray(f9);
        newStyleWord.gaitSizeStd = GetStdOfArray(f9);

        newStyleWord.headOrientationMax = GetMaximumOfArray(f10);
        newStyleWord.headOrientationMin = GetMinimumOfArray(f10);
        newStyleWord.headOrientationMean = GetMeanOfArray(f10);
        newStyleWord.headOrientationMean = GetStdOfArray(f10);

        newStyleWord.decelerationPeaksNo = f11;

        newStyleWord.hipVelocityMax = GetMaximumOfArray(f12);
        newStyleWord.hipVelocityMin = GetMinimumOfArray(f12);
        newStyleWord.hipVelocityStd = GetStdOfArray(f12);

        newStyleWord.handsVelocityMax = GetMaximumOfArray(f13);
        newStyleWord.handsVelocityMin = GetMinimumOfArray(f13);
        newStyleWord.handsVelocityStd = GetStdOfArray(f13);

        newStyleWord.feetVelocityMax = GetMaximumOfArray(f14);
        newStyleWord.feetVelocityMin = GetMinimumOfArray(f14);
        newStyleWord.feetVelocityStd = GetStdOfArray(f14);

        newStyleWord.hipAccelerationMax = GetMaximumOfArray(f15);
        newStyleWord.hipAccelerationStd = GetStdOfArray(f15);

        newStyleWord.handsAccelerationMax = GetMaximumOfArray(f16);
        newStyleWord.handsAccelerationStd = GetStdOfArray(f16);

        newStyleWord.feetAccelerationMax = GetMaximumOfArray(f17);
        newStyleWord.feetAccelerationStd = GetStdOfArray(f17);

        newStyleWord.jerkAccelerationMax = GetMaximumOfArray(f18);
        newStyleWord.jerkAccelerationStd = GetStdOfArray(f18);

        newStyleWord.volumeMax = GetMaximumOfArray(f19);
        newStyleWord.volumeMin = GetMinimumOfArray(f19);
        newStyleWord.volumeMean = GetMeanOfArray(f19);
        newStyleWord.volumeStd = GetStdOfArray(f19);

        newStyleWord.volumeUpperBodyMax = GetMaximumOfArray(f20);
        newStyleWord.volumeUpperBodyMin = GetMinimumOfArray(f20);
        newStyleWord.volumeUpperBodyMean = GetMeanOfArray(f20);
        newStyleWord.volumeUpperBodyStd = GetStdOfArray(f20);

        newStyleWord.volumeLowerBodyMax = GetMaximumOfArray(f21);
        newStyleWord.volumeLowerBodyMin = GetMinimumOfArray(f21);
        newStyleWord.volumeLowerBodyMean = GetMeanOfArray(f21);
        newStyleWord.volumeLowerBodyStd = GetStdOfArray(f21);

        newStyleWord.volumeLeftSideMax = GetMaximumOfArray(f22);
        newStyleWord.volumeLeftSideMin = GetMinimumOfArray(f22);
        newStyleWord.volumeLeftSideMean = GetMeanOfArray(f22);
        newStyleWord.volumeLeftSideStd = GetStdOfArray(f22);

        newStyleWord.volumeRightSideMax = GetMaximumOfArray(f23);
        newStyleWord.volumeRightSideMin = GetMinimumOfArray(f23);
        newStyleWord.volumeRightSideMean = GetMeanOfArray(f23);
        newStyleWord.volumeRightSideStd = GetStdOfArray(f23);

        newStyleWord.torsoHeightMax = GetMaximumOfArray(f24);
        newStyleWord.torsoHeightMin = GetMinimumOfArray(f24);
        newStyleWord.torsoHeightMean = GetMeanOfArray(f24);
        newStyleWord.torsoHeightStd = GetStdOfArray(f24);

        newStyleWord.handsLevelNo1 = f25.x;
        newStyleWord.handsLevelNo2 = f25.y;
        newStyleWord.handsLevelNo3 = f25.z;

        newStyleWord.totalDistanceNo = f26;

        newStyleWord.totalAreaNo = f27;

        #endregion

        styleWords.Add(newStyleWord);

        return newStyleWord;
    }

    private float GetMaximumOfArray(float[] array)
    {
        return Mathf.Max(array);
    }
    private float GetMinimumOfArray(float[] array)
    {
        return Mathf.Min(array);
    }
    private float GetMeanOfArray(float[] array)
    {
        return array.Average();
    }
    private float GetStdOfArray(float[] array)
    {
        float average = array.Average();
        float sumOfSquaresOfDifferences = array.Select(val => (val - average) * (val - average)).Sum();
        float std = Mathf.Sqrt(sumOfSquaresOfDifferences / array.Length);
        return std;
    }
}

