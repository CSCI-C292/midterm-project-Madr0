using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    public RuntimeData _runtimeData;

    public Slider slider;
    private float previousHealth = 0;
    private float currentHealth = 0;

    public GameObject[] rightGears;
    public GameObject[] leftGears;

    public float startX;
    public float endX;
    
    private float healthVel = 0;
    private float slideInVel = 0;
    private bool introSlide;

    void Start() {
        introSlide = true;
    }

    void Update() {
        previousHealth = Mathf.SmoothDamp(previousHealth, currentHealth, ref healthVel, .25f);
        slider.value = previousHealth;
        if(currentHealth > 0 && introSlide) {
            AnimateSlideIn();
        }
        AnimateGears();
    }

    public void SetMaxHealth(float health) {
        previousHealth = health;
        slider.maxValue = health;
        currentHealth = 0;
    }

    public void SetHealth(float health) {
        previousHealth = currentHealth;
        currentHealth = health;
    }

    public void AnimateSlideIn() {
        if(startX < -295.1f) {
            startX = Mathf.SmoothDamp(startX, endX, ref slideInVel, .4f);
            gameObject.transform.localPosition = new Vector3(startX,gameObject.transform.localPosition.y,gameObject.transform.localPosition.z);
        } else {
            introSlide = false;
            gameObject.transform.localPosition = new Vector3(endX,gameObject.transform.localPosition.y,gameObject.transform.localPosition.z);
        }
    }

    public void AnimateGears() {
        float rotation = (50+10*_runtimeData.currentBossStage) * slider.value / slider.maxValue * Time.deltaTime;
        foreach (GameObject gear in rightGears) {
            float degrees = gear.transform.localEulerAngles.z;
            degrees += rotation;
            if(degrees > 360)
                degrees -= 360;
            gear.transform.localEulerAngles = new Vector3(0,0,degrees);
        }
        foreach (GameObject gear in leftGears) {
            float degrees = gear.transform.localEulerAngles.z;
            degrees -= rotation;
            if(degrees > 360)
                degrees -= 360;
            gear.transform.localEulerAngles = new Vector3(0,0,degrees);
        }
    }
}
