using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public RuntimeData _runtimeData;

    public float moveSpeed = 5f;
    public Transform movePoint;
    public float verticalGridLength = .2f;
    public float horizontalGridLength = .3f;
    public int playerHealthCap = 4;
    
    public int playerHealth;

    void Awake() {
        GameEvents.PlayerDamaged += OnPlayerDamage;
        GameEvents.PlayerHealed += OnPlayerHealed;
    }

    void Start()
    {
        movePoint.parent = null;
        playerHealth = playerHealthCap;
        _runtimeData.playerPos = movePoint.transform.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        
        if(Vector3.Distance(transform.position, movePoint.position) <= .05) {
            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                MoveX(horizontalGridLength);
            }
            else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                MoveX(-horizontalGridLength);
            }
            else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
                MoveY(verticalGridLength);
            }
            else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
                MoveY(-verticalGridLength);
            }
        }
    }

    void MoveX(float dist) {
        if(Mathf.Abs(movePoint.position.x + dist) < 1.7) {
            movePoint.position += new Vector3(dist,0,0);
        }
        _runtimeData.playerPos = movePoint.transform.position;
    }

    void MoveY(float dist) {
        if(movePoint.position.y + dist < 0 && movePoint.position.y + dist > -1.5) {
            movePoint.position += new Vector3(0,dist,0);
        }
        _runtimeData.playerPos = movePoint.transform.position;
    }

    void OnPlayerDamage(object sender, EventArgs args) {
        playerHealth--;
        if(playerHealth <= 0) {
            // game over
        }
    }

    void OnPlayerHealed(object sender, EventArgs args) {
        if(playerHealth < playerHealthCap) {
            playerHealth++;
        }
    }
}
