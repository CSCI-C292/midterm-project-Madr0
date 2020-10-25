using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New RuntimeData")]
public class RuntimeData : ScriptableObject
{
    public bool tileActive;
    public Vector3 playerPos;
    public int currentBossStage;
}
