using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;

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
            
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.touchCount > 0 &&
                EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

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

        PlayCrumbsRandom(currentPancake);

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
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(pancakePrefab);

        obj.transform.position = pos;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);

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

        int rand = Random.Range(0, 3);

        if (systems.Length == 1)
        {
            PlayParticle(systems[0]);
            return;
        }

        if (rand == 0) PlayParticle(systems[0]);
        else if (rand == 1) PlayParticle(systems[1]);
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