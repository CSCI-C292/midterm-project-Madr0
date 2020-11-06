using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public RuntimeData _runtimeData;

    public float[] healthStages = {100,100,100,100};
    public float currentHealth;
    public float[] timeBetweenStages = {5,5,5,5};

    public BossBar bossBar;

    void Awake() {
        GameEvents.BossDamaged += OnHealthDamage;
    }

    void Start()
    {
        currentHealth = 0;
        bossBar.SetMaxHealth(healthStages[_runtimeData.currentBossStage]);
    }

    void healthManager(float damage) {
        currentHealth += damage;
        bossBar.SetHealth(currentHealth);
        if(currentHealth >= healthStages[_runtimeData.currentBossStage]) {
            _runtimeData.currentBossStage++;
            currentHealth = 0;
            if(_runtimeData.currentBossStage >= healthStages.Length-1) {
                _runtimeData.currentGameState = State.won;
            } else {
                GameEvents.InvokeStageChanged();
                StartCoroutine(StageChangeCoroutine());
            }
        }
    }

    IEnumerator StageChangeCoroutine() {
        yield return new WaitForSeconds(.2f);
        _runtimeData.tileActive = true;
        yield return new WaitForSeconds(timeBetweenStages[_runtimeData.currentBossStage]-.2f);
        bossBar.SetMaxHealth(healthStages[_runtimeData.currentBossStage]);
        currentHealth = 0;
        _runtimeData.tileActive = false;
    }

    void OnHealthDamage(object sender, BossEventArgs args) {
        healthManager(args.damagePayload);
    }
}
