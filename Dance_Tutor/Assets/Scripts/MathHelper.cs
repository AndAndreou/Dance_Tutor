using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MathHelper : MonoBehaviour
{
    public static float Calculate360Angle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.up, to - from).eulerAngles.z;
    }

    /// <summary>
    /// Transforms value X in the range [a,b], to a number y in the range [c,d] 
    /// </summary>
    /// <param name="x"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <param name="d"></param>
    public static float RangeTransformation(float x, float a, float b, float c, float d)
    {
        return (x - a) * ((d - c) / (b - a)) + c;
    }

    public static void ConvertCoordinatesCircleToSquare(float u, float v, ref float x, ref float y)
    {
        float u2 = u * u;
        float v2 = v * v;
        float twosqrt2 = 2.0f * Mathf.Sqrt(2.0f);
        float subtermx = 2.0f + u2 - v2;
        float subtermy = 2.0f - u2 + v2;
        float termx1 = subtermx + u * twosqrt2;
        float termx2 = subtermx - u * twosqrt2;
        float termy1 = subtermy + v * twosqrt2;
        float termy2 = subtermy - v * twosqrt2;
        x = 0.5f * Mathf.Sqrt(termx1) - 0.5f * Mathf.Sqrt(termx2);
        y = 0.5f * Mathf.Sqrt(termy1) - 0.5f * Mathf.Sqrt(termy2);
    }

    public static Vector3 RandomPositionInSphere(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }

    public static int mod(int a, int b)
    {
        return (a % b + b) % b;
    }

    public static Mesh GenerateMesh(Vector3[] vertices, int torus_count = 5)
    {
        Mesh mesh = new Mesh();
        mesh.Clear(false);

        mesh.vertices = vertices;

        List<int> triangles = new List<int>();
        for (int row = 0; row < vertices.Length / torus_count - 1; ++row)
        {
            for (int col = 0; col < torus_count - 1; ++col)
            {
                triangles.Add(row * torus_count + col);
                triangles.Add(row * torus_count + col + 1);
                triangles.Add((row + 1) * torus_count + col);

                triangles.Add(row * torus_count + col + 1);
                triangles.Add((row + 1) * torus_count + col + 1);
                triangles.Add((row + 1) * torus_count + col);
            }
        }
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();

        return mesh;
    }

    public static float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0.0f;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < mesh.triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];

            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }

        return Mathf.Abs(volume);
    }

    private static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        var v321 = p3.x * p2.y * p1.z;
        var v231 = p2.x * p3.y * p1.z;
        var v312 = p3.x * p1.y * p2.z;
        var v132 = p1.x * p3.y * p2.z;
        var v213 = p2.x * p1.y * p3.z;
        var v123 = p1.x * p2.y * p3.z;
        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }
}
