using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    PlayerActions controls;

    bool gameHasStarted;
    bool gameIsPaused = false;

    [Header("Save Data Info")]
    int money;
    float strengthLevel;
    int bodiesMaxAmount;
    string colors;
    int currentColor;

    private void Awake()
    {
        controls = new PlayerActions();

        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //ResetData();
        LoadData();

        controls.Controls.ResetData.performed += ctx => ResetData();
    }

    public bool GameIsPaused()
    {
        return gameIsPaused;
    }

    public bool GameHasStarted()
    {
        return gameHasStarted;
    }

    public void RestartGame()
    {
        gameHasStarted = false;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        gameHasStarted = true;
    }

    public void PauseResumeGame(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        gameIsPaused = pause;
    }

    public void SaveData(int myMoney = 0)
    {
        int currentMoney = PlayerPrefs.GetInt("Money");
        PlayerPrefs.SetInt("Money", currentMoney + myMoney);

        PlayerPrefs.SetFloat("Strength", strengthLevel);
        PlayerPrefs.SetInt("Bodies", bodiesMaxAmount);
        PlayerPrefs.SetString("Colors", colors);
        PlayerPrefs.SetInt("Current Color", currentColor);

        LoadData();
    }

    public void LoadData()
    {
        money = PlayerPrefs.GetInt("Money", 0);
        strengthLevel = PlayerPrefs.GetFloat("Strength", 1);
        bodiesMaxAmount = PlayerPrefs.GetInt("Bodies", 1);
        colors = PlayerPrefs.GetString("Colors", "100000");
        currentColor = PlayerPrefs.GetInt("Current Color", 0);
    }

    void ResetData()
    {
        PlayerPrefs.SetInt("Money", 0);

        PlayerPrefs.SetFloat("Strength", 1);
        PlayerPrefs.SetInt("Bodies", 1);
        PlayerPrefs.SetString("Colors", "100000");
        PlayerPrefs.SetInt("Current Color", 0);

        print("<color=red>DATA ERASED</color>");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        RestartGame();

        //LoadData();
    }

    public int CurrentMoney()
    {
        return money;
    }

    /// <summary>
    /// To deduct money, add a negative value
    /// </summary>
    /// <param name="value"></param>
    public void AddSpendMoney(int value)
    {
        int currentMoney = PlayerPrefs.GetInt("Money");
        PlayerPrefs.SetInt("Money", currentMoney + value);
    }


    /// <summary>
    /// Position goes from 0 to 5. More than that, it will show an error
    /// </summary>
    /// <param name="value"></param>
    /// <param name="position"></param>
    public void SetColorString(char value, int position)
    {
        var text = colors.ToCharArray();
        text[position] = value;
        colors = new string(text);
        //print(colors);
    }

    public float GetStrengthMultiplier()
    {
        return strengthLevel;
    }

    public int GetStrengthLevel()
    {
        int level = (int)((GetStrengthMultiplier() * 10) - 9);
        return level;
    }

    public void SetStrengthLevel(float newLevel)
    {
        strengthLevel += newLevel;
    }

    public int GetMaxBodies()
    {
        return bodiesMaxAmount;
    }

    public void SetMaxBodies(int newValue)
    {
        bodiesMaxAmount += newValue;
    }

    public string GetColors()
    {
        return colors;
    }

    public int GetCurrentColorIndex()
    {
        return currentColor;
    }

    public void SetCurrentColorIndex(int index)
    {
        currentColor = index;
    }

    #region Enable Disable Input System
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    #endregion
}
