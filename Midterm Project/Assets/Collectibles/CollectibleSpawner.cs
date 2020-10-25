using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectibleSpawner : MonoBehaviour
{
    public RuntimeData _runtimeData;

    public GameObject tilePrefab;
    public GameObject heartPrefab;
    public float spawnRate;

    private float timeSinceLastTile = 3;

    void Awake() {
        GameEvents.StageChanged += SpawnHeart;
    }

    void Update()
    {
        if(_runtimeData.tileActive == false)
            timeSinceLastTile += Time.deltaTime;
        if(timeSinceLastTile > spawnRate) {
            SpawnTile();
            timeSinceLastTile = 0;
        }
    }

    void SpawnTile() {
        _runtimeData.tileActive = true;
        float randX = UnityEngine.Random.Range(0,3);
        if(_runtimeData.playerPos.x < 0)
            randX = UnityEngine.Random.Range(4,7);
        float randY = UnityEngine.Random.Range(0,4);
        Instantiate(tilePrefab, new Vector3(-1.4f+.4f*randX,-.15f-.3f*randY,.1f), Quaternion.identity);
    }

    void SpawnHeart(object sender, EventArgs args) {
        float Xmod = 1;
        if(_runtimeData.playerPos.x < 0)
            Xmod = -1;
        Instantiate(heartPrefab, new Vector3(.6f*Xmod,-.15f,.1f), Quaternion.identity);
    }
}
