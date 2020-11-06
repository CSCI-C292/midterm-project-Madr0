using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hologram : MonoBehaviour
{
    public RuntimeData _runtimeData;

    public float moveSpeed;

    void Start()
    {
        _runtimeData.hologramActive = true;
        transform.position = new Vector3(-_runtimeData.playerPos.x,_runtimeData.playerPos.y,_runtimeData.playerPos.z);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(-_runtimeData.playerPos.x,_runtimeData.playerPos.y,_runtimeData.playerPos.z), moveSpeed * Time.deltaTime);
    }
}
