using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DanceE_Learning : MonoBehaviour {

    BVHLoader bvhLoader = new BVHLoader();

    public void main()
    {
        // -------------------------------------------------------------------------
        // Initialization
        // -------------------------------------------------------------------------
        int windowMotionWord = 35;             // Initialize window size for motion words.
        int stepSize = 5;                      // Initialize step size for motion words.
        int NumMotionWords = 0;                // Initialize the number of motion words.
        string methodMW = "StyleWords";           // Select which method to use for creating motion words(MotionWords or StyleWords).
        string methodMDS = "tMDS";                // Select which method to use for MDS(tMDS for Triangle MDS or cMDS for Classical MDS).
        string methodCluster = "Kmeans";          // Select which method to use for clustering(Peeling or Kmeans or Agglomerative or DBSCAN).
        string methodStyleDistance = "Euclidean"; // Select which method to use to compute the similarity(Euclidean or Pearson).
        string BVHskeleton = "UCY";               // Select the BVH skeleton format(CMU or UCY).
        string display = "Yes";                   // When "Yes" Matlab displays the motion word under processing, when "No" not
        string GenerateDistanceMatrix = "True";   // Select whether you want to generate the distance matrix
        string methodHistDist = "emd";            // Select which method to use to compute the histogram"s distance (emd or chisq).

        // -------------------------------------------------------------------------
        // Select the desired dataset
        // -------------------------------------------------------------------------

        // ---------------------------------------------------------------------
        // Dances - Dance Ethnography
        // ---------------------------------------------------------------------
        string dataset = "Dances";
        string[] optsA = { "Antonis3_1", "Evaki3_2" };


        //float tic; // ? Initialize time
        int[] JointNum = new int[1]; // ?
        float[,] T;
        List<float[,]> motionWords = new List<float[,]>() ;
        List<float[,]> styleWords = new List<float[,]>();

        for (int clip = 0; clip < optsA.Length; clip++)
        {

            print("Start of motion: " + optsA[clip] + " (clip " + clip + " out of " + optsA.Length + ")");

            string bvhLocation = "BVHs\\";
            string bvhFileName = optsA[clip] + ".bvh";
            BVHLoaderAndreas skeleton = new BVHLoaderAndreas(); // ?
            float[] time = new float[1];
            float fps = 0f;
            LoadBVH(bvhLocation, bvhFileName, ref skeleton, ref time, ref fps);

            // ---------------------------------------------------------------------
            // Initialization - frames rate
            // ---------------------------------------------------------------------
            int step = (int)Mathf.Round(fps / 30);           // Defines the step (24fps)
            float timeperiod = 1 / fps;

            if (methodMW == "MotionWords")
            {
                // -----------------------------------------------------------------
                // Define the important joints
                // -----------------------------------------------------------------
                if (BVHskeleton == "UCY")
                {
                    JointNum = new int[] { 2, 3, 4, 5, 7, 8, 9, 10, 12, 13, 14, 15, 65, 66, 41, 42, 43, 44, 63, 17, 18, 19, 20, 39 };
                }
                else if (BVHskeleton == "CMU")
                {
                    JointNum = new int[] { 3, 4, 5, 6, 9, 10, 11, 12, 14, 15, 16, 17, 18, 19, 22, 23, 24, 25, 28, 31, 32, 33, 34, 37 };
                }

                // -----------------------------------------------------------------
                // Create Textures
                // -----------------------------------------------------------------

                T = CreateTextures(skeleton, time, JointNum, step, methodMW);

                // -----------------------------------------------------------------
                // Create Motion Words
                // -Use of the method descibed in [1]
                // -----------------------------------------------------------------
                motionWords.Add(CreateMotionWords(T, windowMotionWord, stepSize));
                NumMotionWords = NumMotionWords + motionWords[clip].Length;
            }
            else if (methodMW == "StyleWords")
            {
                // -----------------------------------------------------------------
                // Define the important joints
                // -----------------------------------------------------------------
                if (BVHskeleton == "UCY")
                {
                    JointNum = new int[] { 1, 2, 3, 4, 5, 7, 8, 9, 10, 12, 13, 14, 15, 65, 66, 41, 42, 43, 44, 63, 17, 18, 19, 20, 39 };
                }
                else if (BVHskeleton == "CMU")
                {
                    JointNum = new int[] { 1, 3, 4, 5, 6, 9, 10, 11, 12, 14, 15, 16, 17, 18, 19, 22, 23, 24, 25, 28, 31, 32, 33, 34, 37 };
                }


                // -----------------------------------------------------------------
                // Create Textures
                // -----------------------------------------------------------------
                T = CreateTextures(skeleton, time, JointNum, step, methodMW);

                // -----------------------------------------------------------------
                // Create Motion Words
                // -Use of the method descibed in [2]
                // -----------------------------------------------------------------

                styleWords.Add(CreateStyleWords(T, windowMotionWord, stepSize, fps / step));
                NumMotionWords = NumMotionWords + styleWords[clip].Length;
            }
        }

        //MotionWords_time = toc; ?
        float MotionWords_time = 0;
        print("Time needed to read files and create motion words: " + MotionWords_time + " seconds(" + MotionWords_time / 60 + " minutes)");
        print("Total number of motion words: " + NumMotionWords);
        print("-----------------------------------------------------");
        print("Start of computing the distances between motion words");
        print("-----------------------------------------------------");


        // -------------------------------------------------------------------------
        // Distances between motionWords:
        // This procedure will return a distance matrix with the peer - to - peer
        // distances between all motion words.
        // -By default, use of the method descibed in [3]
        // -------------------------------------------------------------------------

        
        int[][] motionWordsAll = new int[10][]; //?
        int[][] motionWordsClip = new int[10][]; //?
        int[][] styleWordsAll = new int[10][]; //?
        int[] D = new int[10]; //?
        float[][] normStyleWords = new float[10][]; //?
        if (GenerateDistanceMatrix == "True")
        {
            //tic = 0; // Initialize time
            if (methodMW == "MotionWords")
            {
                // ---------------------------------------------------------------------
                // Merge all motion words from different clips into one file
                // ---------------------------------------------------------------------
                MergeMotionWords(motionWords, ref motionWordsAll, ref motionWordsClip);
                // ---------------------------------------------------------------------
                // -Use of the method descibed in [3]
                // ---------------------------------------------------------------------
                D = DistanceMotionWords(motionWordsAll, windowMotionWord, display);
            }
            else if (methodMW == "StyleWords")
            {
                // ---------------------------------------------------------------------
                // Merge all motion words from different clips into one file
                // ---------------------------------------------------------------------
                MergeMotionWords(styleWords, ref styleWordsAll, ref motionWordsClip);
                normStyleWords = NormalizeFeatures(styleWordsAll);
                // ---------------------------------------------------------------------
                // -Use of the method descibed in [2]
                // ---------------------------------------------------------------------
                D = DistanceStyleWords(normStyleWords, methodStyleDistance, display);
            }

            // DistanceMatrix_time = toc; ?
            float DistanceMatrix_time = 0f;
            print("Time needed to compute the distance matrix: " + DistanceMatrix_time + " seconds (" + DistanceMatrix_time / 60 + " minutes)");

            // -------------------------------------------------------------------------
            // Write result
            // -------------------------------------------------------------------------
            print("DistanceMatrix : " + D.ToString());

            print("MotionWordsClip : " + motionWordsClip.ToString());

        }
    }


    private void LoadBVH(string bvhLocation, string bvhFileName, ref BVHLoaderAndreas skeleton, ref float[] time, ref float fps)
    {
        skeleton.Load(bvhLocation, bvhFileName);
        time = skeleton.time;
        fps = skeleton.fps;
    }

    private float[,] CreateTextures(BVHLoaderAndreas skeleton, float[] time, int[] JointNum, int step, string methodMW)
    {
        int fa = 0;
        float[,] T = new float[JointNum.Length,time.Length];
        for (int frame = 1; frame < time.Length; frame = frame + step)
        {
            if (methodMW == "MotionWords")
            {
                for (int temp = 0; temp < JointNum.Length; temp++)
                {
                    T[((3 * temp) + 0), fa] = skeleton.skeleton[JointNum[temp]].rxyz[frame][0];
                    T[((3 * temp) + 1), fa] = skeleton.skeleton[JointNum[temp]].rxyz[frame][1];
                    T[((3 * temp) + 2), fa] = skeleton.skeleton[JointNum[temp]].rxyz[frame][2];
                }
            }
            else if (methodMW == "StyleWords")
            {
                for (int temp = 0; temp < JointNum.Length; temp++)
                {
                    T[((3 * temp) + 0), fa] = skeleton.skeleton[JointNum[temp]].Dxyz[frame][0];
                    T[((3 * temp) + 1), fa] = skeleton.skeleton[JointNum[temp]].Dxyz[frame][1];
                    T[((3 * temp) + 2), fa] = skeleton.skeleton[JointNum[temp]].Dxyz[frame][2];
                }
            }
            fa++;
        }
        return T; 
    }

    private float[,] CreateMotionWords(float[,] T, int windowMotionWord, int stepSize)
    {
        int k = 0;
        int TotalFrames = T.GetLength(1);

        int rep = TotalFrames - windowMotionWord;

        float[,] motionWords = new float[rep / stepSize, T.GetLength(0)];

        for (int i = 0; i < rep; i = i + stepSize)
        {
            for (int z = 0; z < T.GetLength(0); z++)
            {
                for (int x = i; x < i + windowMotionWord; x++)
                {
                    motionWords[k, z] = T[z, x];
                }
            }
            k++;
        }
        return motionWords;
    }

    private float[,] CreateStyleWords(float[,] T, int windowMotionWord, int stepSize, float fpsPerStep)
    {
        int k = 0;
        int TotalFrames = T.GetLength(1);

        float[,] styleWord = new float[TotalFrames, 85];

        // ?

        k++;
        return new float[1,1];
    }

    private void MergeMotionWords(List<float[,]> motionWords, ref int[][] motionWordsAll, ref int[][] motionWordsClip)
    {

    }

    private int[] DistanceMotionWords(int[][] motionWordsAll, int windowMotionWord, string display)
    {
        return new int[10];
    }

    private float[][] NormalizeFeatures(int[][] styleWordsAll)
    {
        return new float[10][];
    }

    private int[] DistanceStyleWords(float[][] normStyleWords, string methodStyleDistance, string display)
    {
        return new int[10];
    }
}
