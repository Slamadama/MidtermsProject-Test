using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankSystem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private Image mainBar;   // white delayed bar
    [SerializeField] private Image delayBar;  // yellow instant bar

    [Header("Settings")]
    [SerializeField] private float decayDelay = 3f;       // seconds before decay starts
    [SerializeField] private float decayPercentPerSecond = 0.05f; // 5% per second
    [SerializeField] private int maxPoints = 800;         // enough for SSS

    private float points;
    private float displayedPoints;
    private float lastKillTime;

    public void AddPoints(int amount)
    {
        points += amount;
        points = Mathf.Min(points, maxPoints);
        lastKillTime = Time.time;

        // Delay bar jumps instantly
        delayBar.fillAmount = points / maxPoints;
    }

    void OnEnable()
    {
        Enemy.OnEnemyDied += HandleEnemyDeath;
    }

    void OnDisable()
    {
        Enemy.OnEnemyDied -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        AddPoints(10); // or whatever value you want per kill
    }

    private string GetRank(float score)
    {
        if (score < 100) return "E";
        else if (score < 200) return "D";
        else if (score < 300) return "C";
        else if (score < 400) return "B";
        else if (score < 500) return "A";
        else if (score < 600) return "S";
        else if (score < 700) return "SS";
        else return "SSS";
    }

    void Update()
    {
        // Decay after delay
        if (Time.time - lastKillTime > decayDelay && points > 0)
        {
            points -= points * decayPercentPerSecond * Time.deltaTime;
            points = Mathf.Max(points, 0);
        }

        // Smooth catch-up for main bar (white)
        displayedPoints = Mathf.Lerp(displayedPoints, points, Time.deltaTime * 5f);

        // Update UI
        delayBar.fillAmount = GetRankProgress(points);
        mainBar.fillAmount = GetRankProgress(displayedPoints);
        rankText.text = GetRank(points);
    }

    private float GetRankProgress(float score)
    {
        int currentMin = 0;
        int nextMax = 100;

        if (score < 100) { currentMin = 0; nextMax = 100; }
        else if (score < 200) { currentMin = 100; nextMax = 200; }
        else if (score < 300) { currentMin = 200; nextMax = 300; }
        else if (score < 400) { currentMin = 300; nextMax = 400; }
        else if (score < 500) { currentMin = 400; nextMax = 500; }
        else if (score < 600) { currentMin = 500; nextMax = 600; }
        else if (score < 700) { currentMin = 600; nextMax = 700; }
        else { currentMin = 700; nextMax = maxPoints; }

        // Progress within current rank range
        return Mathf.InverseLerp(currentMin, nextMax, score);
    }
}
