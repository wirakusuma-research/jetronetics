using UnityEngine;
using System;
using System.Collections.Generic;

public class EnvironmentSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Tooltip("Target yang menjadi pusat spawn (biasanya pemain).")]
    public Transform playerTarget;

    [Tooltip("Jarak minimum dari pemain tempat environment akan di-spawn.")]
    public float spawnDistance = 20f;

    [Tooltip("Jarak maksimum dari pemain tempat environment bisa di-spawn.")]
    public float maxSpawnDistance = 35f;

    [Header("Spawnable Environments")]
    public List<SpawnableEnvironment> spawnableEnvironments;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Start()
    {
        if (playerTarget == null)
        {
            Debug.LogError("Player Target belum diatur di EnvironmentSpawner!");
            enabled = false;
        }

        // Atur waktu spawn awal untuk setiap environment
        foreach (var env in spawnableEnvironments)
        {
            env.nextSpawnTime = Time.time + env.spawnRate;
        }
    }

    void Update()
    {
        // Bersihkan daftar environment yang sudah hancur
        spawnedObjects.RemoveAll(obj => obj == null);

        // Loop setiap jenis environment
        for (int i = 0; i < spawnableEnvironments.Count; i++)
        {
            var env = spawnableEnvironments[i];

            if (Time.time >= env.nextSpawnTime)
            {
                int currentCount = GetCurrentObjectCount(env.prefab);
                if (currentCount < env.maxCount)
                {
                    SpawnEnvironment(env.prefab);

                    if (env.spawnOnce)
                    {
                        spawnableEnvironments.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        env.nextSpawnTime = Time.time + env.spawnRate;
                    }
                }
                else
                {
                    env.nextSpawnTime = Time.time + 1f;
                }
            }
        }
    }

    void SpawnEnvironment(GameObject prefab)
    {
        // Hitung posisi spawn acak di luar kamera
        Vector2 randomCirclePoint = UnityEngine.Random.insideUnitCircle.normalized;
        float randomDistance = UnityEngine.Random.Range(spawnDistance, maxSpawnDistance);
        Vector3 spawnPosition = playerTarget.position + (Vector3)randomCirclePoint * randomDistance;

        // Buat instance environment dengan rotasi acak (Z-axis karena topdown)
        Quaternion randomRotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f));

        GameObject newEnv = Instantiate(prefab, spawnPosition, randomRotation);
        spawnedObjects.Add(newEnv);
    }

    private int GetCurrentObjectCount(GameObject prefab)
    {
        int count = 0;
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null && obj.name.Contains(prefab.name))
            {
                count++;
            }
        }
        return count;
    }
}

[Serializable]
public class SpawnableEnvironment
{
    [Tooltip("Prefab environment yang akan di-spawn.")]
    public GameObject prefab;

    [Tooltip("Waktu tunggu antar spawn (dalam detik).")]
    public float spawnRate = 10f;

    [Tooltip("Jika diaktifkan, environment ini hanya akan di-spawn satu kali.")]
    public bool spawnOnce = false;

    [Tooltip("Jumlah maksimum environment jenis ini yang bisa ada dalam scene.")]
    public int maxCount = 10;

    [HideInInspector] public float nextSpawnTime;
}
