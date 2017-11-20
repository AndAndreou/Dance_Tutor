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

    public class MotionWord
    {
        // List is for joints and array of quaternion for rotation valus by frame
        public List<Quaternion[]> joint ;

        public List<Vector3[]> GetDistanceBetweenWordsInDegrees(MotionWord[] motionWords)
        {
            List<Vector3[]> disWord = new List<Vector3[]>();
            for (int i = 0; i < motionWords.Length; i++) // motion words
            {
                for (int j = 0; j< motionWords[i].joint.Count; j++) // joints
                {
                    disWord.Add(new Vector3[motionWords[i].joint.Count]);
                    for (int x = 0; x < motionWords[i].joint[j].Length; x++) // frame
                    { 
                        disWord[j] = new Vector3[motionWords[i].joint[j].Length];
                        if (i == 0)
                        {
                            disWord[j][x] = (motionWords[i].joint[j][x]).eulerAngles;
                        }
                        else
                        {
                            disWord[j][x] -= (motionWords[i].joint[j][x]).eulerAngles;
                        }
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
                    sum[j] += distanceWord[j][x];
            }
            return sum;
        }
    }

    public class StyleWord
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

        public float GetMax(StyleWord styleWord)
        {
            float maximum = 0;
            foreach (var field in typeof(StyleWord).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                maximum = Mathf.Max(maximum, (float)field.GetValue(styleWord));
            }
            return maximum;
        }

        public StyleWord GetNormilizedWord(StyleWord styleWord, float valueForNormilized)
        {
            foreach (FieldInfo field in typeof(StyleWord).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                field.SetValue(styleWord, ((float)field.GetValue(styleWord) / valueForNormilized));
            }
            return styleWord;
        }

        public StyleWord GetDistanceBetweenWords(StyleWord[] styleWords)
        {
            StyleWord DisWord = styleWords[0];
            for (int i = 1; i < styleWords.Length; i++)
            {
                foreach (FieldInfo field in typeof(StyleWord).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                {

                    field.SetValue(DisWord, ((float)field.GetValue(DisWord) - (float)field.GetValue(styleWords[i])));
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
    }

    private List<Vector3> skeletonCentroid;

    public Skeleton()
    {
        joints = new List<Joint>();
        skeletonCentroid = new List<Vector3>();
        motionWords = new List<MotionWord>();
        styleWords = new List<StyleWord>();
        //motionWordStepCounter = motionWordWindowSize;
        //styleWordStepCounter = styleWordWindowSize;
    }

    public Skeleton(List<Joint> joints)
    {
        this.joints = joints;

        skeletonCentroid = new List<Vector3>();
        motionWords = new List<MotionWord>();
        styleWords = new List<StyleWord>();
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
    public void AutoAddFrameValuesForEachJoint()
    {
        Vector3 centroidForThisFrame = Vector3.zero;
        foreach (Joint joint in joints)
        {
            Frame f = joint.AddFrame();
            centroidForThisFrame = f.position;
        }

        // Calculate centroid
        centroidForThisFrame = centroidForThisFrame / joints.Count;
        skeletonCentroid.Add(centroidForThisFrame);

    }

    /// <summary>
    /// Add motion word in motion words list
    /// </summary>
    public MotionWord AddMotionWord(int motionWordWindowSize)
    {
        MotionWord motionWord = new MotionWord();
        motionWord.joint = new List<Quaternion[]>();

        foreach( Joint joint in joints)
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

        return motionWord;
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
                Vector3 headOrientation = (jointsWithLastFrames[JointName.Head][i].rotation * Vector3.forward);
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

