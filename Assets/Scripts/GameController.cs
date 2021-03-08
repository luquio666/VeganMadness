using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Waves
{
    public float Time;
    public float MaxSpawnSpeed; // 5
    public float MinSpawnSpeed; // 4
    public float MaxSpawnTime; // 2
    public float MinSpawnTime; // 1
}

public class GameController : MonoBehaviour
{
    public Waves[] Wave;
    public int WaveIndex = 0;
    float _waveTime;
    bool _waitingNextWave = false;
    [Space]
    public GameObject GameStartPanel;
    public GameObject GameEndPanel;
    [Space]
    public Image TimeBar;
    public float TotalWaveTime;
    public float _currentWaveTime;
    [Space]
    public SpawnerController spawnerController;
    public Player player;
    bool _gameStarted;
    float _currentTimer;
    [Space]
    public Image[] Lives;
    public int TotalLives = 3;

    private void OnEnable()
    {
        GameEvents.OnPlayerGetsHit += PlayerGetsHit;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerGetsHit -= PlayerGetsHit;
    }

    private void Start()
    {
        TotalLives = 3;
        GameStartPanel.SetActive(true);
        foreach (var item in Wave)
        {
            TotalWaveTime += item.Time;
        }
    }

    void PlayerGetsHit()
    {
        if (TotalLives > 0)
        {
            TotalLives--;
            StartCoroutine(LoseLive(TotalLives));
        }
        if (TotalLives == 0)
            EndGame();
    }

    IEnumerator LoseLive(int index)
    {
        while (Lives[index].fillAmount > 0)
        {
            Lives[index].fillAmount -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        
        yield return null;
    }

    

    [ContextMenu("StartGame")]
    public void StartGame()
    {
        GameStartPanel.SetActive(false);
        _gameStarted = true;

        // Config
        _waveTime = Wave[WaveIndex].Time;
        _currentTimer = _waveTime;
        spawnerController.StartGame(Wave[WaveIndex]);
        WaveIndex++;

        // Start game
        player.StartGame();
    }

    public void EndGame()
    {
        GameEndPanel.SetActive(true);
        spawnerController.EndGame();
        player.EndGame();
        _gameStarted = false;
    }

    private void Update()
    {
        if (_waitingNextWave == true)
            return;
        if (_gameStarted == false )
            return;

        if (_currentWaveTime < TotalWaveTime)
            _currentWaveTime += Time.deltaTime;
        else
            _currentWaveTime = TotalWaveTime;

        TimeBar.fillAmount = 1 - ((TotalWaveTime - _currentWaveTime)/TotalWaveTime);


        if (_currentTimer > 0)
            _currentTimer -= Time.deltaTime;

        if (_currentTimer < 0)
        {
            _currentTimer = 0;
            if (Wave.Length - 1 >= WaveIndex)
                //StartGame();
                StartCoroutine(StartGameCo());
            else
                EndGame();
        }
        
    }

    IEnumerator StartGameCo()
    {
        _waitingNextWave = true;
        yield return new WaitForSeconds(1f);
        StartGame();
        _waitingNextWave = false;
    }

}
