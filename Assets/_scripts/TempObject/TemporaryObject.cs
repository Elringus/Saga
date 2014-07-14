using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class TemporaryObject : MonoBehaviour
{
    public static TemporaryObject CreateTemp(Transform trans, Transform target)
    {
        TemporaryObject t = ((Transform)Instantiate(trans, target.position, trans.rotation)).GetComponent<TemporaryObject>();
        t.name = "tempobject";
        t.tag = "temporary";
        return t;
    }
    
    private float LifeTime = 0;

    public float MaxLifeTime = 10;

    private void Update()
    {
        if (tag == "temporary")
            LifeTime += Time.deltaTime;

        if (LifeTime > MaxLifeTime)
            GameObject.Destroy(gameObject);
    }
}
