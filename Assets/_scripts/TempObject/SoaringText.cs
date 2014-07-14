using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SoaringText : TemporaryObject
{
    public static SoaringText Create(Transform target, string text)
    {
        SoaringText t = ((Transform)Instantiate(BaseTransform, target.position, BaseTransform.rotation)).GetComponent<SoaringText>();
        t.name = "soaringtext";
        t.tag = "temporary";
        t.Text = text;
        t.rigidbody.AddForce(eMath.RandomVector(5,-10), ForceMode.VelocityChange);

        return t;
    }

    public static SoaringText Create(Transform target, string text, Color textcolor)
    {
        SoaringText t = ((Transform)Instantiate(BaseTransform, target.position, BaseTransform.rotation)).GetComponent<SoaringText>();
        t.name = "soaringtext";
        t.tag = "temporary";
        t.Text = text;
        t.rigidbody.AddForce(eMath.RandomVector(5, -10), ForceMode.VelocityChange);
        t.GetComponent<TextMesh>().color = textcolor;
        return t;
    }

    public static SoaringText Create(Vector3 position, string text, Color tcolor, Vector3 velocity)
    {
        SoaringText t = ((Transform)Instantiate(BaseTransform, position, BaseTransform.rotation)).GetComponent<SoaringText>();
        t.name = "soaringtext";
        t.tag = "temporary";
        t.Text = text;
        t.rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        t.GetComponent<TextMesh>().color = tcolor;
        return t;
    }

    public static SoaringText Create(Vector3 position, string text, Color tcolor)
    {
        return Create(position, text, tcolor, eMath.RandomVector(5, -10));
    }

    public static Transform BaseTransform
    {
        get
        {
            return GameObject.Find("soaringtext").transform;
        }
    }

    public string Text
    {
        get
        {
            return GetComponent<TextMesh>().text;
        }
        set
        {
            GetComponent<TextMesh>().text = value;
        }
    }

}
