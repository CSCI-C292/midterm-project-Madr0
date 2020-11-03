using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name == "Player" || collider.gameObject.name == "Hologram(Clone)") {
            GameEvents.InvokePlayerHealed();
            Destroy(gameObject);
        }
    }
}
