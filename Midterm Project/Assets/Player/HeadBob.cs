using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
    public float bobSpeed = 3f;
    public float bobMagnitude = .02f;
    public GameObject head;

    private float timeCycle = 0;

    void Update()
    {
        head.transform.localPosition = new Vector3(0,bobMagnitude*Mathf.Sin(timeCycle),0);
        timeCycle += Time.deltaTime*bobSpeed;
        if(timeCycle > Mathf.PI)
            timeCycle -= Mathf.PI;
    }
}
