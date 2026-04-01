using UnityEngine;

public class PancakeSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject pancakePrefab;

    [Header("Settings")]
    public float spawnHeightOffset = 2f;

    private GameObject lastPancake;

    void Start()
    {
        SpawnFirstPancake();
    }

    void SpawnFirstPancake()
    {
        lastPancake = Instantiate(pancakePrefab, new Vector3(0, 5f, 0), Quaternion.identity);
        SetupPancake(lastPancake);
    }

    public void SpawnNextPancake()
    {
        Vector3 spawnPos = lastPancake.transform.position + Vector3.up * spawnHeightOffset;

        GameObject newPancake = Instantiate(pancakePrefab, spawnPos, Quaternion.identity);

        lastPancake = newPancake;
        SetupPancake(newPancake);
    }

    void SetupPancake(GameObject pancake)
    {
        PancakeMover mover = pancake.GetComponent<PancakeMover>();
        mover.Initialize(this);
    }
}