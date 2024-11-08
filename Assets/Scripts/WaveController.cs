using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    Transform player;

    [Header("Fighting Ring")]
    public List<Corner> corners = new List<Corner>();
    float lerpTime = 6;
    public Transform ringPivots;
    public float ringSize = 1;

    [Header("Gate")]
    public Transform gate;
    public List<Transform> gatePivots = new List<Transform>();
    Transform gateTarget;


    [Header("Wave")]
    public GameObject enemyPrefab;
    List<GameObject> enemies = new List<GameObject>();
    int currentWave = 0;
    int enemiesAmount = 0;
    int enemiesKilled = 0;
    int remainingEnemies = 0;
    float waveHealth = 40;

    bool startGame = false;

    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>().transform;
        gateTarget = gatePivots[0];
        //StartWave();
    }

    public IEnumerator StartFirstWave()
    {
        yield return new WaitForSeconds(2f);
        startGame = true;
        StartWave();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameController.Instance.GameHasStarted() || !startGame) return;

        ringPivots.localScale = new Vector3(ringSize, 1, ringSize);
        gate.position = Vector3.Lerp(gate.position, gateTarget.position, (lerpTime / 2) * Time.deltaTime);

        for (int i = 0; i < corners.Count; i++)
        {
            corners[i].corner.position = Vector3.Lerp(corners[i].corner.position, corners[i].cornerPivot.position, lerpTime * Time.deltaTime);
        }

        if (enemiesKilled >= enemiesAmount)
            gateTarget = gatePivots[1];
        else if(enemiesKilled <= 0)
            gateTarget = gatePivots[0];

        if(enemies.Count <= 0)
            print("Wave Cleared");
    }

    void SpawnEnemy(Transform spawnPoint)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, transform.rotation, null);
        newEnemy.GetComponent<EnemyBehaviour>().SetParameters(player, waveHealth);
        //newEnemy.GetComponent<EnemyBehaviour>().GetHealth().maxHealth = waveHealth;
        if (!enemies.Contains(newEnemy))
            enemies.Add(newEnemy);
    }

    public void StartWave()
    {
        if (!GameController.Instance.GameHasStarted()) return;

        currentWave = 1;
        remainingEnemies = 0;
        enemiesAmount = 2;
        waveHealth = 20;

        ringSize = 1;

        int currentSpawnPoint = 0;


        for (int i = 0; i < enemiesAmount; i++)
        {
            SpawnEnemy(corners[currentSpawnPoint].spawnPoint);
            if (currentSpawnPoint < 7)
                currentSpawnPoint++;
            else
                currentSpawnPoint = 0;
        }
    }

    public void NextWave()
    {
        enemiesKilled = 0;
        remainingEnemies = 0;
        currentWave ++;
        enemiesAmount ++;
        if(waveHealth < 100)
            waveHealth += 10;

        if(ringSize < 2.5f)
            ringSize += 0.25f;

        int currentSpawnPoint = 0;


        for (int i = 0; i < enemiesAmount; i++)
        {
            SpawnEnemy(corners[currentSpawnPoint].spawnPoint);
            if (currentSpawnPoint < 7)
                currentSpawnPoint++;
            else
                currentSpawnPoint = 0;
        }
    }

    public void KillEnemy()
    {
        enemiesKilled++;
        remainingEnemies++;
    }

    public void CheckClearWave(int enemiesDelivered)
    {
        enemiesKilled -= enemiesDelivered;
        if (enemiesKilled <= 0)
            NextWave();
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }

    public int GetEnemiesRemaining()
    {
        int remaining = enemiesAmount - remainingEnemies;
        return remaining;
    }
}

[System.Serializable]
public class Corner
{
    public Transform corner;
    public Transform cornerPivot;
    public Transform spawnPoint;
}
