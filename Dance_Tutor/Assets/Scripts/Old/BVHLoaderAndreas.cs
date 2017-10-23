using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class BVHLoaderAndreas
{
    public List<JointsAndreas> skeleton = new List<JointsAndreas>();
    public int Nframes;
    public float frame_time;
    public float fps;
    public float[] time;

    JointsAndreas currentJoin;

    public enum Channel
    {
        Xposition = 4,
        Yposition = 5,
        Zposition = 6,
        Xrotation = 1,
        Yrotation = 2,
        Zrotation = 3
    };

    List<IEnumerator> tokens = new List<IEnumerator>();
    int currentHierarchyID = 0;

    public void Load(string directory, string filename)
    {
        string filePath = directory + filename;
        char[] delimiterChars = { ' ', '\t', '\n' };

        CultureInfo ci;

        StreamReader reader = new StreamReader(filePath);
        string ss = reader.ReadToEnd();

        ArrayList s = new ArrayList();
        s.AddRange(ss.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries));

        tokens.Add(s.GetEnumerator());

        int nn = -1;
        int brace_count = 1;

        int index = -1;
        int totalChan = 0;
        int totalChainEnds = 0;

        while (tokens[index].MoveNext())
        {
            if (tokens[index].Current.ToString().Trim().StartsWith("MOTION"))
            {
                break;
            }

            tokens[index].MoveNext();

            if (tokens[index].Current.ToString().Trim().StartsWith("{"))
            {
                brace_count++;
            }
            else if (tokens[index].Current.ToString().Trim().StartsWith("}"))
            {
                brace_count--;
            }
            else if (tokens[index].Current.ToString().Trim().StartsWith("OFFSET"))
            {
                ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                // reading an offset values
                tokens[index].MoveNext(); currentJoin.offset.x = float.Parse(tokens[index].Current.ToString().Trim(), NumberStyles.Any, ci);
                tokens[index].MoveNext(); currentJoin.offset.y = float.Parse(tokens[index].Current.ToString().Trim(), NumberStyles.Any, ci);
                tokens[index].MoveNext(); currentJoin.offset.z = float.Parse(tokens[index].Current.ToString().Trim(), NumberStyles.Any, ci);
            }
            else if (tokens[index].Current.ToString().Trim().StartsWith("CHANNELS"))
            {
                // loading num of channels
                tokens[index].MoveNext();
                currentJoin.Nchannels = int.Parse(tokens[index].Current.ToString().Trim());
                totalChan += currentJoin.Nchannels;

                currentJoin.order = new int[currentJoin.Nchannels];
                if (currentJoin.Nchannels == 6)
                {
                    tokens[index].MoveNext();
                    tokens[index].MoveNext();
                    tokens[index].MoveNext();
                }

                for (int i = 0; i < 3; i++) {
                    int temp = 0;
                    switch (tokens[index].Current.ToString().Trim())
                    {
                        case "Xrotation": temp = (int) Channel.Xrotation;
                            break;
                        case "Yrotation":
                            temp = (int)Channel.Yrotation;
                            break;
                        case "Zrotation":
                            temp = (int)Channel.Zrotation;
                            break;
                    }
                    currentJoin.order[i] = temp;
                    tokens[index].MoveNext();
                }
            }
            else if (tokens[index].Current.ToString().Trim().StartsWith("JOINT") || tokens[index].Current.ToString().Trim().StartsWith("ROOT"))
            {
                nn++;

                if(nn != 0)
                {
                    skeleton.Add(currentJoin);
                }
                JointsAndreas newJoint = new JointsAndreas();
                currentJoin = newJoint;
                //skeleton.Add(newJoint);

                tokens[index].MoveNext();
                currentJoin.name = tokens[index].Current.ToString().Trim();
                currentJoin.nestdepth = brace_count;

                if (brace_count == 1)
                {
                    //root node
                    currentJoin.parent = 0;
                }
                else if((skeleton[nn-1].nestdepth + 1) == brace_count)
                {
                    currentJoin.parent = nn - 1;
                }
                else
                {
                    int prev_parent = skeleton[nn - 1].parent;
                    while(skeleton[prev_parent].nestdepth+1 != brace_count)
                    {
                        prev_parent = skeleton[prev_parent].parent;
                    }
                    currentJoin.parent = prev_parent;
                }

            }
            else if (tokens[index].Current.ToString().Trim().StartsWith("End"))
            {
                currentJoin.Dxyz = new List<float[]>();
                currentJoin.rxyz = new List<float[]>();
                currentJoin.trans = new List<Matrix4x4>();

                skeleton.Add(currentJoin);
            
                JointsAndreas newJoint = new JointsAndreas();
                currentJoin = newJoint;

                tokens[index].MoveNext(); // read site
                tokens[index].MoveNext(); // read {
                brace_count++;
                tokens[index].MoveNext(); // read offset

                currentJoin.name = "";
                currentJoin.parent = nn - 1;
                currentJoin.nestdepth = brace_count;
                currentJoin.Nchannels = 0;

                totalChainEnds++;

                // reading an offset values
                ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                tokens[index].MoveNext(); currentJoin.offset.x = float.Parse(tokens[index].Current.ToString().Trim(), NumberStyles.Any, ci);
                tokens[index].MoveNext(); currentJoin.offset.y = float.Parse(tokens[index].Current.ToString().Trim(), NumberStyles.Any, ci);
                tokens[index].MoveNext(); currentJoin.offset.z = float.Parse(tokens[index].Current.ToString().Trim(), NumberStyles.Any, ci);

                currentJoin.Dxyz = new List<float[]>();
                currentJoin.rxyz = new List<float[]>();
                currentJoin.trans = new List<Matrix4x4>();

                skeleton.Add(currentJoin);
            }
        }

        //Initial processing and error checking

        int Nnodes = skeleton.Count;
        int Nchannels = totalChan;
        int Nchainends = totalChainEnds;

        // motion
        while (tokens[index].MoveNext())
        {
            string tmp = tokens[index].Current.ToString().Trim();

            if (tmp.StartsWith("Frames:"))
            {
                tokens[index].MoveNext();
                // loading frame number
                Nframes = int.Parse(tokens[index].Current.ToString().Trim());

            }
            else if (tmp.StartsWith("Frame"))
            {
                tokens[index].MoveNext();   // Time:
                tokens[index].MoveNext();

                // loading frame time
                ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                frame_time = float.Parse(tokens[index].Current.ToString().Trim(), NumberStyles.Any, ci);

                break;
            }
        }

        fps = 1 / frame_time;
        time = new float[Nframes];
        for(int i = 0; i < Nframes; i++)
        {
            time[i] = frame_time * i;
        }

        //motion data
        int frameCounter = 0;
        ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        ci.NumberFormat.CurrencyDecimalSeparator = ".";

        while (tokens[index].MoveNext())
        {
            for(int i = 0; i < skeleton.Count; i++)
            {
                JointsAndreas temp = skeleton[i];
                float[] tempDxyz = new float[3];
                float[] temprxyz = new float[3];
                if (temp.Nchannels == 6)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        tempDxyz[x]=(temp.offset[i] + float.Parse(tokens[index].Current.ToString().Trim(), NumberStyles.Any, ci));
                        tokens[index].MoveNext();
                    }
                    temp.Dxyz.Add(tempDxyz);
                }

                for(int z = 0; z < 3; z++)
                {
                    temprxyz[z] = (float.Parse(tokens[index].Current.ToString().Trim(), NumberStyles.Any, ci));
                    tokens[index].MoveNext();
                }
                temp.rxyz.Add(temprxyz);

                //transformation_matrix
                if (temp.Nchannels != 0)
                {
                    float[] array1 = new float[3];
                    float[] array2 = new float[3];
                    int[] array3 = new int[3];
                    if (temp.Nchannels == 6)
                    {
                        array1 = tempDxyz;
                        array2 = temprxyz;
                        array3 = temp.order;
                    }
                    else // == 3
                    {
                        array1 = new float[3] { temp.offset[0], temp.offset[1], temp.offset[2] };
                        array2 = temprxyz;
                        array3 = temp.order;
                    }
                    Matrix4x4 tempMatrix = new Matrix4x4();
                    //temp.trans.Add(new Matrix4x4());
                    float[] cos = new float[3] { Mathf.Cos(array2[0]), Mathf.Cos(array2[0]), Mathf.Cos(array2[0]) };
                    float[] sin = new float[3] { Mathf.Sin(array2[0]), Mathf.Sin(array2[0]), Mathf.Sin(array2[0]) };

                    float[][][] RxRyRz = new float[3][][];
                    RxRyRz[0] = new float[3][];
                    RxRyRz[1] = new float[3][];
                    RxRyRz[2] = new float[3][];
                    RxRyRz[0][1] = new float[3];
                    RxRyRz[0][2] = new float[3];
                    RxRyRz[0][3] = new float[3];
                    RxRyRz[1][1] = new float[3];
                    RxRyRz[1][2] = new float[3];
                    RxRyRz[1][3] = new float[3];
                    RxRyRz[2][1] = new float[3];
                    RxRyRz[2][2] = new float[3];
                    RxRyRz[2][3] = new float[3];

                    RxRyRz[0][0][0] = 1;
                    RxRyRz[0][0][1] = 0;
                    RxRyRz[0][0][2] = 0;
                    RxRyRz[0][1][0] = 0;
                    RxRyRz[0][1][1] = cos[0];
                    RxRyRz[0][1][2] = -sin[0];
                    RxRyRz[0][2][0] = 0;
                    RxRyRz[0][2][1] = sin[0];
                    RxRyRz[0][2][2] = cos[0];

                    RxRyRz[1][0][0] = cos[1];
                    RxRyRz[1][1][0] = 0;
                    RxRyRz[1][2][0] = sin[1];
                    RxRyRz[1][0][1] = 0;
                    RxRyRz[1][1][1] = 1;
                    RxRyRz[1][2][1] = 0;
                    RxRyRz[1][0][2] = -sin[1];
                    RxRyRz[1][1][2] = 0;
                    RxRyRz[1][2][2] = cos[1];

                    RxRyRz[2][0][0] = cos[2];
                    RxRyRz[2][1][0] = -sin[2];
                    RxRyRz[2][2][0] = 0;
                    RxRyRz[2][0][1] = sin[2];
                    RxRyRz[2][1][1] = cos[2];
                    RxRyRz[2][2][1] = 0;
                    RxRyRz[2][0][2] = 0;
                    RxRyRz[2][1][2] = 0;
                    RxRyRz[2][2][2] = 1;

                    float[][] tempMulty = MatrixProduct(RxRyRz[array3[0]], RxRyRz[array3[1]]);
                    float[][] rotM = MatrixProduct(tempMulty, RxRyRz[array3[2]]);

                    if (temp.Nchannels != 0)
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            for (int y = 0; y < 3; y++)
                            {
                                tempMatrix[x, y] = rotM[x][y];
                            }
                        }
                    }
                    tempMatrix[0, 3] = array1[0];
                    tempMatrix[1, 3] = array1[1];
                    tempMatrix[2, 3] = array1[2];

                    tempMatrix[3, 0] = 0;
                    tempMatrix[3, 1] = 0;
                    tempMatrix[3, 2] = 0;
                    tempMatrix[3, 3] = 1;

                    if(temp.Nchannels == 3)
                    {
                        tempMatrix = (skeleton[temp.parent].trans[frameCounter] * tempMatrix);
                        for(int x = 0; x < tempDxyz.Length; x++)
                        {
                            tempDxyz[x] = tempMatrix[x, 4];
                        }
                        temp.Dxyz[frameCounter] = tempDxyz;
                    }
                    else if (temp.Nchannels == 0)
                    {
                        Matrix4x4 m = Matrix4x4.identity;
                        m[0,3] = temp.offset[0];
                        m[1, 3] = temp.offset[1];
                        m[1, 3] = temp.offset[1];
                        tempMatrix = skeleton[temp.parent].trans[frameCounter] * m;
                    }

                    temp.trans.Add(tempMatrix);
                }

                skeleton[i] = temp;
            }
            frameCounter++;

        }


            //int index = motions.Count - 1;

            //tokens.Add(s.GetEnumerator());

            //while (tokens[index].MoveNext())
            //{
            //    if (tokens[index].Current.ToString().Trim().StartsWith("HIERARCHY"))
            //        LoadHierarchy(index);
            //}
            //reader.Close();

            //return motions[index];
        }

    public static float[][] MatrixProduct(float[][] matrixA,
    float[][] matrixB)
    {
        int aRows = matrixA.Length; int aCols = matrixA[0].Length;
        int bRows = matrixB.Length; int bCols = matrixB[0].Length;
        if (aCols != bRows)
            throw new Exception("Non-conformable matrices in MatrixProduct");
        float[][] result = MatrixCreate(aRows, bCols);
        for (int i = 0; i < aRows; ++i) // each row of A
            for (int j = 0; j < bCols; ++j) // each col of B
                for (int k = 0; k < aCols; ++k)
                    result[i][j] += matrixA[i][k] * matrixB[k][j];
        return result;
    }

    public static float[][] MatrixCreate(int rows, int cols)
    {
        // creates a matrix initialized to all 0.0s  
        // do error checking here?  
        float[][] result = new float[rows][];
        for (int i = 0; i < rows; ++i)
            result[i] = new float[cols];
        // auto init to 0.0  
        return result;
    }
}

public struct JointsAndreas
{
    public String name;
    public int nestdepth;
    public int parent;
    public Vector3 offset;
    public int Nchannels;
    public int[] order;
    public List<float[]> Dxyz;
    public List<float[]> rxyz;
    public List<Matrix4x4> trans;
}