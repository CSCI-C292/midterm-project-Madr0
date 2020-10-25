using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawAttack : MonoBehaviour
{
    public float rotationSpeed;
    public float moveSpeed;
    public int moveDirectionX;
    public int moveDirectionY;

    void Update()
    {
        Rotate();
        Move();
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.name == "Player") {
            GameEvents.InvokePlayerDamage();
            Destroy(gameObject);
        }
    }

    void Rotate() {
        float rotation = rotationSpeed * Time.deltaTime;
        float degrees = gameObject.transform.localEulerAngles.z;
        degrees += rotation;
        if(degrees > 360)
            degrees -= 360;
        gameObject.transform.localEulerAngles = new Vector3(0,0,degrees);
    }

    void Move() {
        float moveX = gameObject.transform.position.x + (moveSpeed*Time.deltaTime*moveDirectionX);
        float moveY = gameObject.transform.position.y + (moveSpeed*Time.deltaTime*moveDirectionY);
        gameObject.transform.position = new Vector3(moveX,moveY,0);
    }
}
