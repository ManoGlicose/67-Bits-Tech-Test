using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("Menu UI")]
    public TMP_Text menuCoins;
    public CanvasGroup menuCanvas;
    public CanvasGroup storeCanvas;

    [Header("Start Game")]
    public CanvasGroup mainCanvas;
    public CanvasGroup gameCanvas;

    [Header("Pause/Resume")]
    public CanvasGroup pauseScreen;

    [Header("Game Over")]
    public CanvasGroup gameOverCanvas;

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
        if (!playerValues.GetComponent<PlayerController>().IsBeingDamaged())
            damageBar.fillAmount = Mathf.Lerp(damageBar.fillAmount, healthBar.fillAmount, damageSpeed * Time.deltaTime);

        healthBar.fillAmount = health;

        coinCounter.text = playerValues.money.ToString();

        wavesCounter.text = "WAVE: " + waves.GetCurrentWave().ToString() + "\nENEMIES: " + waves.GetEnemiesRemaining().ToString();

        // Menu
        menuCoins.text = GameController.Instance.CurrentMoney().ToString();
    }

    public void StartGame()
    {
        mainCanvas.alpha = 0;
        mainCanvas.blocksRaycasts = false;

        gameCanvas.alpha = 1;
        gameCanvas.blocksRaycasts = true;

        Camera.main.GetComponent<CameraController>().SetRotation();
        playerValues.LoadGameValues();
        StartCoroutine(FindFirstObjectByType<WaveController>().StartFirstWave());

        GameController.Instance.StartGame();
    }

    public void PauseGame()
    {
        GameController.Instance.PauseResumeGame(GameController.Instance.GameIsPaused() ? false : true);
        pauseScreen.alpha = GameController.Instance.GameIsPaused() ? 1 : 0;
        pauseScreen.blocksRaycasts = GameController.Instance.GameIsPaused();
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameController.Instance.RestartGame();
        if (GameController.Instance.GameIsPaused())
            GameController.Instance.PauseResumeGame(false);
    }

    public void GameOver()
    {
        gameOverCanvas.alpha = 1;
        gameOverCanvas.blocksRaycasts = true;
        StartCoroutine(DeathScreen());
    }

    public void GoToStore(bool toStore)
    {
        menuCanvas.alpha = toStore ? 0 : 1;
        menuCanvas.blocksRaycasts = !toStore;

        storeCanvas.alpha = toStore ? 1 : 0;
        storeCanvas.blocksRaycasts = toStore;

        Camera.main.GetComponent<CameraController>().isInStore = toStore;
    }

    IEnumerator DeathScreen()
    {
        yield return new WaitForSecondsRealtime(5);

        QuitGame();
    }
}
