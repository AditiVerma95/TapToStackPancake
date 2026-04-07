using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PancakeSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject pancakePrefab;
    public CameraFollow camFollow;

    [Header("UI")]
    public TextMeshProUGUI scoreText; // shows ": 0", ": 1", etc.

    [Header("Pooling")]
    public int poolSize = 50;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private GameObject currentPancake;
    private int stackCount = 0;

    private int score = 0;
    private int highScore = 0;

    private UserInputActionAsset inputActions;

    void Awake()
    {
        // Input
        inputActions = new UserInputActionAsset();
        inputActions.Enable();

        // Pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(pancakePrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        PancakeMover.OnPancakePlaced += SpawnNextPancake;
    }

    void OnDestroy()
    {
        PancakeMover.OnPancakePlaced -= SpawnNextPancake;
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI();

        SpawnFirstPancake();
    }

    void Update()
    {
        if (inputActions.Player.Drop.WasPressedThisFrame())
        {
            HandleDrop();
        }
    }

    void HandleDrop()
    {
        if (currentPancake == null) return;

        PancakeMover mover = currentPancake.GetComponent<PancakeMover>();
        if (mover != null)
            mover.Drop();
    }

    void SpawnFirstPancake()
    {
        stackCount = 1;
        score = 0;
        UpdateScoreUI();

        currentPancake = GetFromPool(new Vector3(0, 5f, 0));
        camFollow.UpdateCameraHeight(stackCount);
    }

    void SpawnNextPancake()
    {
        if (currentPancake == null)
        {
            Debug.LogError("Current pancake is NULL!");
            return;
        }

        // ✅ Update score
        score++;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }

        UpdateScoreUI();

        Collider col = currentPancake.GetComponentInChildren<Collider>();

        if (col == null)
        {
            Debug.LogError("Collider missing on pancake!");
            return;
        }

        float height = col.bounds.size.y;

        Vector3 spawnPos = currentPancake.transform.position + Vector3.up * (height + 0.5f);

        currentPancake = GetFromPool(spawnPos);

        stackCount++;
        camFollow.UpdateCameraHeight(stackCount);
    }

    GameObject GetFromPool(Vector3 pos)
    {
        GameObject obj;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
        }
        else
        {
            obj = Instantiate(pancakePrefab);
        }

        obj.transform.position = pos;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);

        PancakeMover mover = obj.GetComponent<PancakeMover>();
        if (mover != null)
            mover.ResetPancake();

        return obj;
    }

    void UpdateScoreUI()
    {
        // 👉 Simple format like you wanted
        scoreText.text = " : " + score.ToString();

        // If later you want:
        // scoreText.text = "Score : " + score + "  Best : " + highScore;
    }
}