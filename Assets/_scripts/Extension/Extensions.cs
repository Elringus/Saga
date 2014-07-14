using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public static class eMath
{
    #region Angle
    public static float Angle2Dplus(Vector3 c, Vector3 p)
    {
        float r = Vector3.Distance(c, p);
        int q = Findquarter(c, p);

        return 2 * Mathf.PI - Angle2D(c, p, r, q);
    }

    public static float Angle2D(Vector3 c, Vector3 p)
    {
        float a = Angle2Dplus(c, p);

        if (a > Mathf.PI)
            a = a - 2 * Mathf.PI; //если было a=330, то мы получим 330-360=-30!

        return a;
    }

    private static float Angle2D(Vector3 center, Vector3 point, float radius, int quarter)
    {
        switch (quarter)
        {
            case 1:
                return (float)Mathf.Abs(((point.y - center.y) / radius).ASin());
            case 2:
                return (float)(Mathf.Abs(((point.x - center.x) / radius).ASin()) + Mathf.PI / 2);
            case 3:
                return (float)(Mathf.Abs(((point.y - center.y) / radius).ASin() + Mathf.PI));
            case 4:
                return (float)(Mathf.Abs(((point.x - center.x) / radius)).ASin() + 3 * Mathf.PI / 2);
            default:
                return 0;
        }
    }
    
    private static int Findquarter(Vector3 center, Vector3 point)
    {
        if (point.x >= center.x && point.y < center.y)
            return 1;
        if (point.x < center.x && point.y < center.y)
            return 2;
        if (point.x < center.x && point.y >= center.y)
            return 3;
        if (point.x >= center.x && point.y >= center.y)
            return 4;
        return 0;
    }
    #endregion
    #region Float
    static private int defAccuracy = 0;
    static public float R(this float f,int acc)
    {
        float t=10;
        return (float)Mathf.Round(f * t.P(acc)) / t.P(acc);
    }
    
    static public float R(this float f)
    {
        return (float)f.R(defAccuracy);
    }

    static public float P(this float f, float p)
    {
        return Mathf.Pow(f, p);
    }

    static public float ASin(this float d)
    {
        if (Mathf.Abs(d) <= 1)
            return Mathf.Asin(d);
        else
            if ((Mathf.Abs(d) - 1).R(1) == 0)
                if (d > 0)
                    return Mathf.Asin(1);
                else
                    return Mathf.Asin(-1);
            else
                return float.NaN;
    }

    static public float ACos(this float d)
    {
        if (Mathf.Abs(d) <= 1)
            return Mathf.Acos(d);
        else
            if ((Mathf.Abs(d) - 1).R(1) == 0)
                if (d > 0)
                    return Mathf.Acos(1);
                else
                    return Mathf.Acos(-1);
            else
                return float.NaN;
    }
    #endregion
    #region Equation

    public static Vector2 SqrRoots(float a, float b, float c)
    {
        float d = b.P(2) - 4 * a * c;

        if (d < 0)
            return new Vector2(float.NaN, float.NaN);

        d = Mathf.Sqrt(d);

        return new Vector2((-d - b) / (2 * a),(d - b) / (2 * a));
    }

    #endregion
    #region Vector

    public static Vector3 RandomVector(float kxy, float z)
    {
        float x = UnityEngine.Random.Range(-1f, 1f);
        float y = UnityEngine.Random.Range(-1f, 1f);

        return new Vector3(kxy * x, kxy * y, z);
    }

    public static float Magnitude2D(this Vector3 v)
    {
        return (v.x.P(2) + v.y.P(2)).P(0.5f);
    }

    #endregion
}

public interface IUniversalCopy
{
        object uCopy();
}

public static class eObjects
{
    public static object maneCopy(this Array arr)
    {
        Type type = arr.GetType().GetElementType();
        Array copyarr = Array.CreateInstance(type, arr.Length);

        if (type.GetInterfaces().Contains(typeof(IUniversalCopy)))
        {
            for (int i = 0; i < arr.Length; i++)
                copyarr.SetValue(((IUniversalCopy)arr.GetValue(i)).uCopy(), i);

            return copyarr;
        }


        if (type.IsValueType || type == typeof(string))
        {
            for (int i = 0; i < arr.Length; i++)
                copyarr.SetValue(arr.GetValue(i), i);

            return copyarr;
        }

        return null;
    }

    public static IEnumerable<TSourse> uCopy<TSourse>(this IEnumerable<TSourse> sourse)
    {
        Array arr = sourse.ToArray();

        return ((TSourse[])arr.maneCopy());
    }
}