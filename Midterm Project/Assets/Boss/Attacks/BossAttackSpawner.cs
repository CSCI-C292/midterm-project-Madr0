using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackSpawner : MonoBehaviour
{
    public RuntimeData _runtimeData;

    public float timeBetweenAttacks = 3;

    public GameObject _sawPrefab;
    public GameObject _sawLauncherPrefab;

    private float timeSinceLastAttack;
    private bool attackActive;

    void Start()
    {
        timeSinceLastAttack = 0;
        attackActive = false;
    }

    void Update()
    {
        if(!attackActive) {
            timeSinceLastAttack += Time.deltaTime;
        }
        if(timeSinceLastAttack > timeBetweenAttacks) {
            timeSinceLastAttack = 0;
            if(_runtimeData.currentBossStage == 0) {
                FullSawAttack(.5f*5+5,.5f);
            } else if(_runtimeData.currentBossStage == 1) {

            }
        }
    }

    void RightSawAttack(float duration, float delayBetweenSaws) {
        StartCoroutine(AttackWaitCoroutine(duration));
        for(int i=0; i<3; i++) {
            StartCoroutine(SawSpawnCoroutine(delayBetweenSaws*i,-1,2.5f,-.15f-.6f*i));
        }
    }

    void LeftSawAttack(float duration, float delayBetweenSaws) {
        StartCoroutine(AttackWaitCoroutine(duration));
        for(int i=0; i<2; i++) {
            StartCoroutine(SawSpawnCoroutine(delayBetweenSaws*i,1,-2.5f,-.45f-.6f*i));
        }
    }

    void FullSawAttack(float duration, float delayBetweenSaws) {
        StartCoroutine(AttackWaitCoroutine(duration));
        for(int i=0; i<5; i++) {
            int mod = i%2==0? 1 : -1;
            StartCoroutine(SawSpawnCoroutine(delayBetweenSaws*i,1*mod,-2.5f*mod,-.15f-.3f*i));
        }
    }

    IEnumerator AttackWaitCoroutine(float seconds) {
        attackActive = true;
        yield return new WaitForSeconds(seconds);
        attackActive = false;
    }

    IEnumerator SawSpawnCoroutine(float seconds, int direction, float xLoc, float yLoc) {
        yield return new WaitForSeconds(seconds);
        StartCoroutine(SpawnSawLaunchers(direction, yLoc));
        yield return new WaitForSeconds(1);
        GameObject saw = Instantiate(_sawPrefab,new Vector3(xLoc,yLoc,0),Quaternion.identity);
        saw.GetComponent<SawAttack>().moveDirectionX = direction;
        Destroy(saw,5f);
    }

    IEnumerator SpawnSawLaunchers(int direction, float yLoc) {
        GameObject launcherThrow = Instantiate(_sawLauncherPrefab,new Vector3(-2.5f*direction,yLoc,0),Quaternion.identity);
        if(direction == 1) {
            launcherThrow.transform.localEulerAngles = new Vector3(0,0,180f);
        }
        StartCoroutine(FadeInOut(launcherThrow,.25f,1.5f,.25f));
        Destroy(launcherThrow,2.1f);
        yield return new WaitForSeconds(5);
        GameObject launcherCatch = Instantiate(_sawLauncherPrefab,new Vector3(2.5f*direction,yLoc,0),Quaternion.identity);
        if(direction == -1) {
            launcherCatch.transform.localEulerAngles = new Vector3(0,0,180f);
        }
        StartCoroutine(FadeInOut(launcherCatch,.25f,1.5f,.25f));
        Destroy(launcherCatch,2.1f);
    }

    IEnumerator FadeInOut(GameObject go, float durationIn, float durationWait, float durationOut) {
        Color modColor = go.GetComponent<Renderer>().material.color;
        modColor.a = 0;
        go.GetComponent<Renderer>().material.color = modColor;
        for(float t=0; t<durationIn; t+=Time.deltaTime) {
            float normalizedTime = t/durationIn;
            modColor.a = Mathf.Lerp(0,1,normalizedTime);
            go.GetComponent<Renderer>().material.color = modColor;
            yield return null;
        }
        modColor.a = 1;
        go.GetComponent<Renderer>().material.color = modColor;
        yield return new WaitForSeconds(durationWait);
        for(float t=0; t<durationOut; t+=Time.deltaTime) {
            float normalizedTime = t/durationOut;
            modColor.a = Mathf.Lerp(1,0,normalizedTime);
            go.GetComponent<Renderer>().material.color = modColor;
            yield return null;
        }
        modColor.a = 0;
        go.GetComponent<Renderer>().material.color = modColor;
    }
}
