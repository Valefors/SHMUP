﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform _gameOverScreen;
    [SerializeField] RectTransform _pauseScreen;
    [SerializeField] RectTransform _titleScreen;

    [Header("Leaderboard")]
    [SerializeField] Text _localScore;
    [Header("- Scores")]
    [SerializeField] Text[] _scoreTextArray;
    [Header("- Names")]
    [SerializeField] Text[] _nameTextArray;
    [Header("Enter Name")]
    public InputField localPlayerName;
    [SerializeField] Button _sendButton;

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
        EventManager.StartListening(EventManager.MENU_EVENT, Menu);
        EventManager.StartListening(EventManager.SEND_SCORE_EVENT, UpdateUI);

        _gameOverScreen.gameObject.SetActive(false);
        _pauseScreen.gameObject.SetActive(false);
        _sendButton.interactable = false;
    }

    public void Play()
    {
        StartCoroutine(StaticFunctions.FadeOut(result => _gameOverScreen.GetComponent<CanvasGroup>().alpha = result, 0.5f));
        _gameOverScreen.gameObject.SetActive(false);
    }

    private void Pause()
    {
        _pauseScreen.gameObject.SetActive(true);
        StartCoroutine(StaticFunctions.FadeIn(result => _pauseScreen.GetComponent<CanvasGroup>().alpha = result, 0.5f));
    }

    private void Resume()
    {
        StartCoroutine(StaticFunctions.FadeOut(result => _pauseScreen.GetComponent<CanvasGroup>().alpha = result, 0.2f, () => _pauseScreen.gameObject.SetActive(false)));
    }

    void GameOver()
    {
        UpdateUI();
        _gameOverScreen.gameObject.SetActive(true);
        StartCoroutine(StaticFunctions.FadeIn(result => _gameOverScreen.GetComponent<CanvasGroup>().alpha = result, 0.5f));
        _localScore.text = ScoreManager.manager.score.ToString();
    }

    void Menu()
    {
        _gameOverScreen.gameObject.SetActive(false);
        _pauseScreen.gameObject.SetActive(false);
        _titleScreen.gameObject.SetActive(true);
    }

    #region Leaderboard

    public void OnClickTextField()
    {
        if (localPlayerName.text != "") _sendButton.interactable = true;
        else _sendButton.interactable = false;
    }

    public void OnClickSendName()
    {
        EventManager.TriggerEvent(EventManager.SEND_NAME_EVENT);
    }

    void UpdateUI()
    {
        for (int i = 0; i < Leaderboard.scoreList.Count; i++)
        {
            _nameTextArray[i].text = Leaderboard.scoreList[i].name;
            _scoreTextArray[i].text = Leaderboard.scoreList[i].score.ToString();
        }
    }

    #endregion

    private void Update()
    {
      //  if (Input.GetKeyDown(KeyCode.Escape))
      if(Input.GetButtonDown("Cancel"))
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
        EventManager.StopListening(EventManager.MENU_EVENT, Menu);
        EventManager.StopListening(EventManager.SEND_SCORE_EVENT, UpdateUI);
    }
}
