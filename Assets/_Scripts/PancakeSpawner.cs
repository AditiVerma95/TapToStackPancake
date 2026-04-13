using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class PancakeSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject pancakePrefab;
    public CameraFollow camFollow;

    [Header("UI")]
    public TextMeshProUGUI scoreText;

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
        inputActions = new UserInputActionAsset();
        inputActions.Enable();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(pancakePrefab);

            // 🔥 RESET PARTICLES IN POOL
            ParticleSystem[] psList = obj.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in psList)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

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
        GameOverArea.isGameOver = false;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateScoreUI();

        SpawnFirstPancake();
    }

    void Update()
    {
        if (GameOverArea.isGameOver) return;

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

        currentPancake = GetFromPool(new Vector3(0, 2.5f, 0));

        camFollow.UpdateCameraHeight(stackCount);
    }

    void SpawnNextPancake()
    {
        if (GameOverArea.isGameOver) return;

        if (currentPancake == null)
        {
            Debug.LogError("Current pancake is NULL!");
            return;
        }

        // 🔥 PLAY RANDOM CRUMBS FROM THIS PANCAKE
        PlayCrumbsRandom(currentPancake);

        // Score update
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

        float topY = col.bounds.max.y;

        float spawnOffset = 1.2f;

        Vector3 spawnPos = new Vector3(
            currentPancake.transform.position.x,
            topY + spawnOffset,
            currentPancake.transform.position.z
        );

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

        // 🔥 RESET PARTICLES WHEN REUSING
        ParticleSystem[] psList = obj.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in psList)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        PancakeMover mover = obj.GetComponent<PancakeMover>();
        if (mover != null)
            mover.ResetPancake();

        return obj;
    }

    void PlayCrumbsRandom(GameObject pancake)
    {
        ParticleSystem[] systems = pancake.GetComponentsInChildren<ParticleSystem>();

        if (systems.Length == 0) return;

        int rand = Random.Range(0, 3); // 0 left, 1 right, 2 both

        if (systems.Length == 1)
        {
            PlayParticle(systems[0]);
            return;
        }

        if (rand == 0)
        {
            PlayParticle(systems[0]);
        }
        else if (rand == 1)
        {
            PlayParticle(systems[1]);
        }
        else
        {
            PlayParticle(systems[0]);
            PlayParticle(systems[1]);
        }
    }

    void PlayParticle(ParticleSystem ps)
    {
        if (ps == null) return;

        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ps.Play();
    }

    void UpdateScoreUI()
    {
        scoreText.text = " : " + score.ToString();
    }
}