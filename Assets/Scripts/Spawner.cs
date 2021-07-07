using System;
using UnityEngine;
using Mirror;
public class Spawner : NetworkBehaviour
{
    float _nextSpawnTime;

    [SerializeField] float _delay = 2f;
    [SerializeField] Transform[] _spawnPoints;
    [SerializeField] Enemy[] _enemies;

    void Update()
    {
        if (isServer == false)
            return;

        if (ShouldSpawn())
            Spawn();
    }

    void Spawn()
    {
        _nextSpawnTime = Time.time + _delay;
        Transform spawnPoint = ChooseSpawnPoint();
        Enemy enemyPrefab = ChooseEnemy();
        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        NetworkServer.Spawn(enemy.gameObject);
    }

    Enemy ChooseEnemy()
    {
        int randomIndex = UnityEngine.Random.Range(0, _enemies.Length);
        var enemy = _enemies[randomIndex];
        return enemy;
    }

    Transform ChooseSpawnPoint()
    {
        int randomIndex = UnityEngine.Random.Range(0, _spawnPoints.Length);
        var spawnPoint = _spawnPoints[randomIndex];
        return spawnPoint;
    }

    bool ShouldSpawn()
    {
        return Time.time >= _nextSpawnTime;
    }
}