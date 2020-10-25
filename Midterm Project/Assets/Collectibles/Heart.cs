using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name == "Player") {
            GameEvents.InvokePlayerHealed();
            Destroy(gameObject);
        }
    }
}
