using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackSpawner : MonoBehaviour
{
    public RuntimeData _runtimeData;

    public GameObject _sawPrefab;
    public GameObject _sawLauncherPrefab;

    public GameObject _lightningPrefab;
    public GameObject _lightningGunPrefab;

    public GameObject _spikePrefab;
    public GameObject _spikeWarningPrefab;

    private int previousRand;
    private bool attackActive;

    void Start()
    {
        GameEvents.SpawnHologram();
        StartCoroutine(AttackWaitCoroutine(5));
    }

    void Update()
    {
        if(!_runtimeData.tileActive && !attackActive) {
            int rand = Random.Range(0,3);
            if(rand != previousRand) {
                if(rand == 0) {
                    StartCoroutine(AttackWaitCoroutine(5));
                    LightningAttack(2,2,3);
                } else if(rand == 1) {
                    StartCoroutine(AttackWaitCoroutine(5));
                    FullSawAttack(2,.7f);
                } else if(rand == 2) {
                    StartCoroutine(AttackWaitCoroutine(5));
                    SpikeAttack(1.5f,1.5f,14);
                }
            }
            previousRand = rand;
        }
    }

    IEnumerator AttackWaitCoroutine(float seconds) {
        attackActive = true;
        yield return new WaitForSeconds(seconds);
        attackActive = false;
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

    void RightSawAttack(float sawSpeed, float delayBetweenSaws) {
        for(int i=0; i<3; i++) {
            StartCoroutine(SawSpawnCoroutine(delayBetweenSaws*i,-sawSpeed,2.5f,-.15f-.6f*i));
        }
    }

    void LeftSawAttack(float sawSpeed, float delayBetweenSaws) {
        for(int i=0; i<2; i++) {
            StartCoroutine(SawSpawnCoroutine(delayBetweenSaws*i,sawSpeed,-2.5f,-.45f-.6f*i));
        }
    }

    void FullSawAttack(float sawSpeed, float delayBetweenSaws) {
        for(int i=0; i<5; i++) {
            int value = i%2==0 ? 1 : -1;
            StartCoroutine(SawSpawnCoroutine(delayBetweenSaws*i,-sawSpeed*value,2.5f*value,-.15f-.3f*i));
        }
    }

    IEnumerator SawSpawnCoroutine(float initialWait, float speed, float xLoc, float yLoc) {
        yield return new WaitForSeconds(initialWait);
        StartCoroutine(SpawnSawLaunchers(speed, yLoc));
        yield return new WaitForSeconds(.5f);
        GameObject saw = Instantiate(_sawPrefab,new Vector3(xLoc,yLoc,0),Quaternion.identity);
        saw.GetComponent<SawAttack>().moveSpeedX = speed;
        Destroy(saw,5/Mathf.Abs(speed));
    }

    IEnumerator SpawnSawLaunchers(float speed, float yLoc) {
        int direction = (int) (speed/Mathf.Abs(speed));
        GameObject launcherThrow = Instantiate(_sawLauncherPrefab,new Vector3(-2.5f*direction,yLoc,0),Quaternion.identity);
        if(direction == 1) {
            launcherThrow.transform.localEulerAngles = new Vector3(0,0,180f);
        }
        StartCoroutine(FadeInOut(launcherThrow,.125f,.75f,.125f));
        Destroy(launcherThrow,1.1f);
        yield return new WaitForSeconds(5/Mathf.Abs(speed));
        GameObject launcherCatch = Instantiate(_sawLauncherPrefab,new Vector3(2.5f*direction,yLoc,0),Quaternion.identity);
        if(direction == -1) {
            launcherCatch.transform.localEulerAngles = new Vector3(0,0,180f);
        }
        StartCoroutine(FadeInOut(launcherCatch,.125f,.75f,.125f));
        Destroy(launcherCatch,1.1f);
    }

    // warning duration should not be below 1
    void LightningAttack(float warningDuration, float duration, int amount) {
        List<float> currentLocs = new List<float>();
        for (int i=0; i<amount; i++) {
            float xLoc = (float) (Random.Range(0,7) * .4 - 1.4f);
            if(!currentLocs.Contains(xLoc)) {
                currentLocs.Add(xLoc);
            } else {
                i--;
            }
        }
        foreach (float i in currentLocs) {
            StartCoroutine(SpawnLightning(warningDuration, duration, i));
        }
    }

    IEnumerator SpawnLightning(float warningDuration, float duration, float xLoc) {
        GameObject spawner = Instantiate(_lightningGunPrefab,new Vector3(xLoc,.13f,0),Quaternion.identity);
        StartCoroutine(FadeInOut(spawner,.25f,warningDuration-.5f+duration,.25f));
        Destroy(spawner,duration+warningDuration+.15f);
        yield return new WaitForSeconds(warningDuration);
        GameObject lightning = Instantiate(_lightningPrefab,new Vector3(xLoc,-.75f,0),Quaternion.identity);
        Destroy(lightning,duration);
    }

    void SpikeAttack(float warningDuration, float duration, int numOfSpikes) {
        bool[,] spawnLocations = new bool[4,5];
        int numTotal = 20;
        for (int i = 0; i < spawnLocations.GetLength(0); i++) {
            for (int j = 0; j < spawnLocations.GetLength(1); j++) {
                if(Random.Range(0,numTotal) < numOfSpikes) {
                    numOfSpikes--;
                    spawnLocations[i,j] = true;
                }
                numTotal--;
            }
        }
        for (int i = 0; i < spawnLocations.GetLength(0); i++) {
            for (int j = 0; j < spawnLocations.GetLength(1); j++) {
                if(spawnLocations[i,j] == true) {
                    StartCoroutine(SpawnSpikes(warningDuration, duration, i*.4f-1.4f, -j*.3f-.15f));
                }
            }
        }
    }

    IEnumerator SpawnSpikes(float warningDuration, float duration, float xLoc, float yLoc) {
        GameObject warning = Instantiate(_spikeWarningPrefab,new Vector3(xLoc,yLoc,.5f),Quaternion.identity);
        GameObject warningMirror = Instantiate(_spikeWarningPrefab,new Vector3(-xLoc,yLoc,.5f),Quaternion.identity);
        StartCoroutine(FadeInOut(warning,.15f,warningDuration+duration+.1f,.15f));
        StartCoroutine(FadeInOut(warningMirror,.15f,warningDuration+duration+.1f,.15f));
        Destroy(warning,warningDuration+duration+.5f);
        Destroy(warningMirror,warningDuration+duration+.5f);
        yield return new WaitForSeconds(warningDuration);
        GameObject spike = Instantiate(_spikePrefab,new Vector3(xLoc,yLoc,.48f),Quaternion.identity);
        GameObject spikeMirror = Instantiate(_spikePrefab,new Vector3(-xLoc,yLoc,.48f),Quaternion.identity);
        StartCoroutine(FadeInOut(spike,.05f,duration-.1f,.05f));
        StartCoroutine(FadeInOut(spike,.05f,duration-.1f,.05f));
        Destroy(spike,duration+.05f);
        Destroy(spikeMirror,duration+.05f);
    }
}
