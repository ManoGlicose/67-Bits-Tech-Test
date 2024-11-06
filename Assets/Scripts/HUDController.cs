using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Components")]
    PlayerValues playerValues;
    public WaveController waves;

    [Header("UI")]
    public Image healthBar;
    public Image damageBar;
    public float damageSpeed = 5;
    public TMP_Text coinCounter;
    public TMP_Text wavesCounter;

    // Start is called before the first frame update
    void Start()
    {
        playerValues = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerValues>();
    }

    // Update is called once per frame
    void Update()
    {
        ControlUI();
    }

    void ControlUI()
    {
        float health = (float)(playerValues.GetHealth() / playerValues.maxHealth);
        if(!playerValues.GetComponent<PlayerController>().IsBeingDamaged())
            damageBar.fillAmount = Mathf.Lerp(damageBar.fillAmount, healthBar.fillAmount, damageSpeed * Time.deltaTime);

        healthBar.fillAmount = health;

        coinCounter.text = playerValues.money.ToString();

        wavesCounter.text = "WAVE: " + waves.GetCurrentWave().ToString() + "\nENEMIES: " + waves.GetEnemiesRemaining().ToString();
    }
}
