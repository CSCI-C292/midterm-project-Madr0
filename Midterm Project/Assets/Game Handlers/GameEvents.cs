using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossEventArgs : EventArgs 
{
    public float damagePayload;
}

public static class GameEvents
{
    public static event EventHandler<BossEventArgs> BossDamaged;
    public static event EventHandler PlayerDamaged;
    public static event EventHandler PlayerHealed;
    public static event EventHandler StageChanged;
    public static event EventHandler HologramSpawned;

    public static void InvokeBossDamage(float damage) {
        BossDamaged(null, new BossEventArgs {damagePayload = damage});
    }

    public static void InvokePlayerDamage() {
        PlayerDamaged(null, EventArgs.Empty);
    }

    public static void InvokePlayerHealed() {
        PlayerHealed(null, EventArgs.Empty);
    }

    public static void InvokeStageChanged() {
        StageChanged(null, EventArgs.Empty);
    }

    public static void SpawnHologram() {
        HologramSpawned(null, EventArgs.Empty);
    }
}
