using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform _gameOverScreen;
    [SerializeField] RectTransform _pauseScreen;

    private static UIManager _manager;
    public static UIManager manager {
        get {
            return _manager;
        }
    }

    private void Awake()
    {
        if (_manager == null) _manager = this;

        else if (_manager != this) Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(EventManager.GAME_OVER_EVENT, GameOver);
        EventManager.StartListening(EventManager.RESUME_EVENT, Resume);
        EventManager.StartListening(EventManager.PAUSE_EVENT, Pause);
        EventManager.StartListening(EventManager.PLAY_EVENT, Play);

        _gameOverScreen.gameObject.SetActive(false);
        _pauseScreen.gameObject.SetActive(false);
    }

    public void Play()
    {
        _gameOverScreen.gameObject.SetActive(false);
    }

    private void Pause()
    {
        _pauseScreen.gameObject.SetActive(true);
    }

    private void Resume()
    {
        _pauseScreen.gameObject.SetActive(false);
    }

    void GameOver()
    {
        _gameOverScreen.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventManager.TriggerEvent(EventManager.PAUSE_EVENT);
        }

    }

    void OnDisable()
    {
        EventManager.StopListening(EventManager.GAME_OVER_EVENT, GameOver);
        EventManager.StopListening(EventManager.PAUSE_EVENT, Pause);
        EventManager.StopListening(EventManager.RESUME_EVENT, Resume);
        EventManager.StopListening(EventManager.PLAY_EVENT, Play);
    }
}
