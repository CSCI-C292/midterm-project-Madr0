using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStateCheck : MonoBehaviour
{
    public RuntimeData _runtimeData;
    public GameObject fade;
    public GameObject text;

    void Update()
    {
        if(_runtimeData.currentGameState == State.playing) {
            fade.SetActive(false);
            text.SetActive(false);
        } else if(_runtimeData.currentGameState == State.loss) {
            fade.SetActive(true);
            text.SetActive(true);
            text.GetComponent<TextMeshProUGUI>().text = "You Lose";
        } else if(_runtimeData.currentGameState == State.won) {
            fade.SetActive(true);
            text.SetActive(true);
            text.GetComponent<TextMeshProUGUI>().text = "Boss Defeated";
        }
    }
}
