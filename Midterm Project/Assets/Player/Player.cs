using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
    public RuntimeData _runtimeData;
    public GameObject _hologramPrefab;

    public float moveSpeed = 5f;
    public Transform movePoint;
    public float verticalGridLength = .2f;
    public float horizontalGridLength = .3f;
    public int playerHealthCap = 4;
    public float playerITime = 1f;
    
    public int playerHealth;
    public Slider healthBar;

    private float timeInvincible = 0;

    private float xLow = -1.6f;
    private float xHigh = 1.6f;

    void Awake() {
        GameEvents.PlayerDamaged += OnPlayerDamage;
        GameEvents.PlayerHealed += OnPlayerHealed;
        GameEvents.HologramSpawned += OnHologramSpawned;
    }

    void Start()
    {
        movePoint.parent = null;
        playerHealth = playerHealthCap;
        _runtimeData.playerPos = movePoint.transform.position;
        healthBar.maxValue = playerHealthCap;
        healthBar.value = playerHealth;
    }

    void Update()
    {
        if(_runtimeData.currentGameState == State.playing) {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
            timeInvincible -= Time.deltaTime;
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
    }

    void MoveX(float dist) {
        if(movePoint.position.x + dist < xHigh && movePoint.position.x + dist > xLow) {
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
        if (timeInvincible <= 0 && _runtimeData.currentGameState == State.playing) {
            timeInvincible = playerITime;
            playerHealth--;
        }
        if(playerHealth <= 0) {
            _runtimeData.currentGameState = State.loss;
        }
        healthBar.value = playerHealth;
    }

    void OnPlayerHealed(object sender, EventArgs args) {
        if(playerHealth < playerHealthCap) {
            playerHealth++;
        }
        healthBar.value = playerHealth;
    }

    void OnHologramSpawned(object sender, EventArgs args) {
        if(_runtimeData.playerPos.x < 0) {
            xHigh = 0;
        } else {
            xLow = 0;
        }
        GameObject hologram = Instantiate(_hologramPrefab, new Vector3(-_runtimeData.playerPos.x,_runtimeData.playerPos.y,_runtimeData.playerPos.z),Quaternion.identity);
        hologram.GetComponent<Hologram>().moveSpeed = moveSpeed;
    }
}
