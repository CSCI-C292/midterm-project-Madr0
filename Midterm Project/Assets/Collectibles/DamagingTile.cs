using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingTile : MonoBehaviour
{
    public RuntimeData _runtimeData;
    public float score = 10;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name == "Player") {
            GameEvents.InvokeBossDamage(score);
            _runtimeData.tileActive = false;
            Destroy(gameObject);
        }
    }
}
