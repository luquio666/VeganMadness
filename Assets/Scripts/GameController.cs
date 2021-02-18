using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject GameStartPanel;
    public GameObject GameEndPanel;
    [Space]
    public float LevelTime = 180f;
    public Image TimeBar;
    public SpawnerController spawnerController;
    public Player player;
    bool _gameStarted;
    float _currentTimer;

    private void Start()
    {
        GameStartPanel.SetActive(true);
    }

    [ContextMenu("StartGame")]
    public void StartGame()
    {
        GameStartPanel.SetActive(false);
        _currentTimer = LevelTime;
        _gameStarted = true;
        spawnerController.StartGame();
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
        if (_gameStarted == false)
            return;

        if (_currentTimer > 0)
            _currentTimer -= Time.deltaTime;

        if (_currentTimer < 0)
        {
            _currentTimer = 0;
            EndGame();
        }

        TimeBar.fillAmount = _currentTimer/LevelTime;
    }


}
