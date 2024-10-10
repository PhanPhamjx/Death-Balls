using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BalloonManager : MonoBehaviour
{
    public GameObject SpawnPoits;
    public Transform[] spawnPositions;
    public GameObject[] balloonPrefabs;
    public float spawnInterval = 2.0f;
    public float spawnRandomTime;
    private _BalloonController currentBalloon;

    private void Awake()
    {

        GameObject spawnPoits = GameObject.Find("SpawnPoits");

        if (FindObjectsOfType<_BalloonManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        GameManager.Instance.TimeSpawn = spawnInterval;
        InvokeRepeating("TrySpawnBalloon", 2f, spawnInterval);
        StartCoroutine(SpawnBalloonsPeriodically(spawnRandomTime));
       
    }

    private void TrySpawnBalloon()
    {
        if (currentBalloon == null || !currentBalloon.isBaloonInPoit)
        {
            SpawnBalloon();
        }
    }

    private void SpawnBalloon()
    {
        GameObject spawnPos = GameObject.Find("spawnPos");
        int randomIndex = Random.Range(0, balloonPrefabs.Length);
        GameObject balloon = Instantiate(balloonPrefabs[randomIndex], spawnPos.transform.position, Quaternion.identity);
        if (balloonPrefabs.Length == 0)
        {
            Debug.LogWarning("No balloon prefabs assigned.");
            return;
        }

        if (spawnPos == null)
        {
            Debug.LogWarning("Spawn position is not assigned.");
            return;
        }


        if (balloon == null)
        {
            Debug.LogError("Failed to instantiate balloon.");
            return;
        }

        currentBalloon = balloon.GetComponent<_BalloonController>();

        if (currentBalloon == null)
        {
            Debug.LogError("BalloonController component is missing or the balloon has been destroyed.");
            return;
        }
    }
    public void SpawnRandomBalloonWithValues(int numberOfBalloons)
    {

        if (balloonPrefabs.Length == 0)
        {
            Debug.LogWarning("No balloon prefabs assigned.");
            return;
        }

        if (spawnPositions.Length == 0)
        {
            Debug.LogWarning("No spawn positions assigned.");
            return;
        }

        for (int i = 0; i < numberOfBalloons; i++)
        {
            int randomIndex = Random.Range(0, balloonPrefabs.Length);
            GameObject balloon = Instantiate(balloonPrefabs[randomIndex], spawnPositions[i % spawnPositions.Length].position, Quaternion.identity);

            float randomScale = Random.Range(0.3f, 0.7f);
            int randomValue = Random.Range(1, 9);
            int speedScale = Random.Range(5, 6);

            _BalloonController balloonController = balloon.GetComponent<_BalloonController>();
            if (balloonController != null)
            {
                balloonController.InitializeRandomBalloons(randomScale, randomValue, speedScale);
            }
        }
    }

    private IEnumerator SpawnBalloonsPeriodically(float interval)
    {
        while (true)
        {
            if (this == null) yield break;

            int Balls = spawnPositions.Length;
            SpawnRandomBalloonWithValues(Balls);
            yield return new WaitForSeconds(interval);
        }
    }

    private void OnDestroy()
    {
        CancelInvoke("TrySpawnBalloon");
    }
}
