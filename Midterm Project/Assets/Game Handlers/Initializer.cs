using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    public RuntimeData _runtimeData;

    void Awake() {
        _runtimeData.tileActive = false;
        _runtimeData.currentBossStage = 0;
    }
}
