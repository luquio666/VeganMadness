using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public Spawner[] Spawners;
    public float MaxSpawnSpeed, MinSpawnSpeed = 3f;
    public float MaxSpawnTime, MinSpawnTime;
    public GameObject[] Items;
    bool _gameStarted;
    Coroutine _spawnItemsCo;

    public void StartGame()
    {
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
            float rndSpawnTime = Random.Range(MinSpawnTime, MaxSpawnTime);
            yield return new WaitForSeconds(rndSpawnTime);

            var randomSpeed = Random.Range(MinSpawnSpeed, MaxSpawnSpeed);
            var randomItem = Items[Random.Range(0, Items.Length)];
            var randomSpawner = Spawners[Random.Range(0, Spawners.Length)];

            randomSpawner.SpawnItem(randomItem, randomSpeed);
        }
    }
}
