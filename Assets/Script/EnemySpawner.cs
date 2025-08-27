using UnityEngine;
using System;
using System.Collections.Generic;
public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Tooltip("Target yang akan dijauhi saat spawn (biasanya pemain).")]
    public Transform playerTarget;

    [Tooltip("Jarak minimum dari pemain tempat musuh akan di-spawn.")]
    public float spawnDistance = 15f;

    [Tooltip("Jarak maksimum dari pemain tempat musuh bisa di-spawn.")]
    public float maxSpawnDistance = 25f;

    [Header("Spawnable Enemies")]
    public List<SpawnableEnemy> spawnableEnemies;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        if (playerTarget == null)
        {
            Debug.LogError("Player Target belum diatur di EnemySpawner!");
            enabled = false;
        }

        // Atur waktu spawn awal untuk setiap jenis musuh
        foreach (var enemy in spawnableEnemies)
        {
            enemy.nextSpawnTime = Time.time + enemy.spawnRate;
        }
    }

    void Update()
    {
        // Bersihkan daftar musuh yang sudah hancur
        spawnedEnemies.RemoveAll(enemy => enemy == null);

        // Loop melalui setiap jenis musuh yang bisa di-spawn
        for (int i = 0; i < spawnableEnemies.Count; i++)
        {
            var enemy = spawnableEnemies[i];

            // Cek apakah sudah waktunya untuk spawn
            if (Time.time >= enemy.nextSpawnTime)
            {
                // Cek apakah jumlah musuh saat ini kurang dari batas maksimum
                int currentEnemyCount = GetCurrentEnemyCount(enemy.enemyPrefab);
                if (currentEnemyCount < enemy.maxCount)
                {
                    SpawnEnemy(enemy.enemyPrefab);

                    // Atur waktu spawn berikutnya
                    if (enemy.spawnOnce)
                    {
                        // Hentikan spawn jika hanya sekali
                        spawnableEnemies.RemoveAt(i);
                        i--; // Sesuaikan indeks setelah menghapus
                    }
                    else
                    {
                        enemy.nextSpawnTime = Time.time + enemy.spawnRate;
                    }
                }
                else
                {
                    // Jika batas tercapai, tunda spawn hingga frame berikutnya
                    enemy.nextSpawnTime = Time.time + 1f;
                }
            }
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        // Hitung posisi spawn acak di luar pandangan kamera
        Vector2 randomCirclePoint = UnityEngine.Random.insideUnitCircle.normalized;
        float randomDistance = UnityEngine.Random.Range(spawnDistance, maxSpawnDistance);
        Vector3 spawnPosition = playerTarget.position + (Vector3)randomCirclePoint * randomDistance;

        // Buat instance musuh
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        spawnedEnemies.Add(newEnemy);

        //Debug.Log("Spawn musuh di posisi: " + spawnPosition);
    }

    private int GetCurrentEnemyCount(GameObject enemyPrefab)
    {
        int count = 0;
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null && enemy.name.Contains(enemyPrefab.name))
            {
                count++;
            }
        }
        return count;
    }
}

[Serializable]
public class SpawnableEnemy
{
    [Tooltip("Prefab musuh yang akan di-spawn.")]
    public GameObject enemyPrefab;

    [Tooltip("Waktu tunggu antar spawn (dalam detik).")]
    public float spawnRate = 5f;

    [Tooltip("Jika diaktifkan, musuh ini hanya akan di-spawn satu kali.")]
    public bool spawnOnce = false;

    [Tooltip("Jumlah maksimum musuh jenis ini yang bisa berada di scene pada saat yang sama.")]
    public int maxCount = 5;

    [HideInInspector] public float nextSpawnTime;
}
