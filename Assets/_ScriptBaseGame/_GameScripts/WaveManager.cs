using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private int baseCount = 2;

    private DifficultyManager difficultyManager;
    private int currentWave = 0;
    private int aliveEnemies = 0;

    [SerializeField] private TextMeshProUGUI waveText;

    [SerializeField] private GameObject bossPrefab;

    void OnEnable()
    {
        Enemy.OnEnemyDied += HandleEnemyDeath;
    }

    void OnDisable()
    {
        Enemy.OnEnemyDied -= HandleEnemyDeath;
    }

    void Start()
    {
        difficultyManager = UnityEngine.Object.FindFirstObjectByType<DifficultyManager>();
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWave++;
        float multiplier = difficultyManager.DifficultyMultiplier;
        int enemyCount = Mathf.RoundToInt(baseCount * currentWave * multiplier);

        Debug.Log($"[WaveManager] Starting Wave {currentWave}, spawning {enemyCount} enemies.");

        aliveEnemies = enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            spawner.SpawnEnemy();
        }

        // Boss spawn every 3 waves
        if (currentWave % 3 == 0 && bossPrefab != null)
        {
            GameObject boss = Instantiate(bossPrefab, spawner.transform.position, Quaternion.identity);
            aliveEnemies++; // count boss as part of alive enemies
            Debug.Log($"[WaveManager] Boss spawned on Wave {currentWave}!");
        }

        // Update UI
        if (waveText != null)
        {
            waveText.text = $"Wave {currentWave}";
        }
    }

    void HandleEnemyDeath(Enemy enemy)
    {
        aliveEnemies--;

        if (aliveEnemies <= 0)
        {
            Debug.Log($"[WaveManager] Wave {currentWave} cleared!");
            StartNextWave();
        }
    }
}
