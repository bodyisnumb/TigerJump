using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> prefabsToSpawn = new List<GameObject>();
    public float destroyTime = 5f;
    public float spawnInterval = 2f;
    public int minimumAttempts = 4;

    private RectTransform canvasRect;
    private List<GameObject> recentlySpawned = new List<GameObject>();

    private UIManagerGame uIManagerGame;

    private void Start()
    {
        canvasRect = GetComponent<RectTransform>();
        StartCoroutine(SpawnObjectCoroutine());

        uIManagerGame = FindObjectOfType<UIManagerGame>();
        if (uIManagerGame == null)
        {
            Debug.LogError("UIManagerGame script not found!");
        }
    }

    private void Update()
    {
        SetCurrentScore(uIManagerGame.currentScore);
    }

    IEnumerator SpawnObjectCoroutine()
    {
        while (true)
        {
            GameObject selectedPrefab = GetPrefabToSpawn();

            Vector2 canvasSpawnPosition = GetRandomPositionInsideCanvas();

            GameObject spawnedObject = Instantiate(selectedPrefab, canvasSpawnPosition, Quaternion.identity, transform);
            Destroy(spawnedObject, destroyTime);

            recentlySpawned.Add(selectedPrefab);

            if (recentlySpawned.Count > minimumAttempts)
            {
                recentlySpawned.RemoveAt(0);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    GameObject GetPrefabToSpawn()
    {
        foreach (GameObject prefab in prefabsToSpawn)
        {
            if (!recentlySpawned.Contains(prefab))
            {
                return prefab;
            }
        }

        return prefabsToSpawn[Random.Range(0, prefabsToSpawn.Count)];
    }

    Vector2 GetRandomPositionInsideCanvas()
    {
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        float randomX = Random.Range(-canvasWidth / 2f, canvasWidth / 2f);
        float randomY = Random.Range(-canvasHeight / 2f, canvasHeight / 2f);

        return new Vector2(randomX, randomY);
    }

    public void SetCurrentScore(int score)
    {
        AdjustParameters(score);
    }

    private void AdjustParameters(int score)
    {
        destroyTime = 5f - (score * 0.1f);
        destroyTime = Mathf.Clamp(destroyTime, 1f, 5f);

        spawnInterval = 2f - (score * 0.05f);
        spawnInterval = Mathf.Clamp(spawnInterval, 0.5f, 2f);
    }
}
