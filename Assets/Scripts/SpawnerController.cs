using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public Spawner[] Spawners;
    float _maxSpawnSpeed, _minSpawnSpeed, _maxSpawnTime, _minSpawnTime;
    public GameObject[] Items;
    bool _gameStarted;
    Coroutine _spawnItemsCo;

    public void StartGame(Waves wave)
    {
        _maxSpawnSpeed = wave.MaxSpawnSpeed;
        _minSpawnSpeed = wave.MinSpawnSpeed;
        _maxSpawnTime = wave.MaxSpawnTime;
        _minSpawnTime = wave.MinSpawnTime;

        _gameStarted = true;
        _spawnItemsCo = StartCoroutine(SpawnItemsCo());
    }

    public void EndGame()
    {
        _gameStarted = false;
        StopCoroutine(_spawnItemsCo);
    }

    IEnumerator SpawnItemsCo()
    {
        while (_gameStarted)
        {
            float rndSpawnTime = Random.Range(_minSpawnTime, _maxSpawnTime);
            yield return new WaitForSeconds(rndSpawnTime);

            var randomSpeed = Random.Range(_minSpawnSpeed, _maxSpawnSpeed);
            var randomItem = Items[Random.Range(0, Items.Length)];
            var randomSpawner = Spawners[Random.Range(0, Spawners.Length)];

            randomSpawner.SpawnItem(randomItem, randomSpeed);
        }
    }
}
