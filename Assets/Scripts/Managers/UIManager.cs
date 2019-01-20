using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform _gameOverScreen;
    [SerializeField] RectTransform _pauseScreen;
    [SerializeField] RectTransform _titleScreen;
    [SerializeField] RectTransform _tutorialScreen;

    [Header("Leaderboard")]
    [SerializeField] Text _localScore;
    [Header("- Scores")]
    [SerializeField] Text[] _scoreTextArray;
    [Header("- Names")]
    [SerializeField] Text[] _nameTextArray;
    [Header("Enter Name")]
    public InputField localPlayerName;

    [SerializeField] Button _sendButton;

    [Header("Leaderboard Menu")]
    [Header("- Scores")]
    [SerializeField] Text[] _scoreMenuTextArray;
    [Header("- Names")]
    [SerializeField] Text[] _nameMenuTextArray;

    [Header("FIRST BUTTON SELECTION")]
    [SerializeField] Button _creditsButton;
    [SerializeField] Button _leaderboardButton;
    [SerializeField] Button _tutorialButton;

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
        EventManager.StartListening(EventManager.TUTORIAL_EVENT, Tutorial);

        _gameOverScreen.gameObject.SetActive(false);
        _pauseScreen.gameObject.SetActive(false);
        _sendButton.interactable = false;
    }

    public void Play()
    {
        StartCoroutine(StaticFunctions.FadeOut(result => _gameOverScreen.GetComponent<CanvasGroup>().alpha = result, 0.5f));
        _gameOverScreen.gameObject.SetActive(false);
        Cursor.visible = false;
    }

    void Tutorial()
    {
        EventSystem.current.firstSelectedGameObject = _tutorialButton.gameObject;
        EventSystem.current.SetSelectedGameObject(_tutorialButton.gameObject);

        StartCoroutine(StaticFunctions.FadeIn(result => _tutorialScreen.GetComponent<CanvasGroup>().alpha = result, 0.2f));
        _tutorialScreen.gameObject.SetActive(true);

        Cursor.visible = true;
    }

    public void OnClickEndTutorial()
    {
        StartCoroutine(StaticFunctions.FadeOut(result => _tutorialScreen.GetComponent<CanvasGroup>().alpha = result, 0.5f, () => _tutorialScreen.gameObject.SetActive(false)));

        Cursor.visible = false;

        EventManager.TriggerEvent(EventManager.END_TUTORIAL_EVENT);
    }

    private void Pause()
    {
        _pauseScreen.gameObject.SetActive(true);
        StartCoroutine(StaticFunctions.FadeIn(result => _pauseScreen.GetComponent<CanvasGroup>().alpha = result, 0.5f));
        Cursor.visible = true;
    }

    private void Resume()
    {
        StartCoroutine(StaticFunctions.FadeOut(result => _pauseScreen.GetComponent<CanvasGroup>().alpha = result, 0.2f, () => _pauseScreen.gameObject.SetActive(false)));
        Cursor.visible = false;
    }

    void GameOver()
    {
        UpdateUI();
        _gameOverScreen.gameObject.SetActive(true);
        StartCoroutine(StaticFunctions.FadeIn(result => _gameOverScreen.GetComponent<CanvasGroup>().alpha = result, 0.5f));
        _localScore.text = ScoreManager.manager.score.ToString();
        Cursor.visible = true;
    }

    void Menu()
    {
        _gameOverScreen.gameObject.SetActive(false);
        _pauseScreen.gameObject.SetActive(false);
        _titleScreen.gameObject.SetActive(true);

        Cursor.visible = true;
    }

    public void OnClickBack()
    {
        SelectionManager.manager.SetModeMenu();
    }

    public void OnClickLeaderBoard()
    {
        EventSystem.current.firstSelectedGameObject = _leaderboardButton.gameObject;
        EventSystem.current.SetSelectedGameObject(_leaderboardButton.gameObject);

        for (int i = 0; i < Leaderboard.scoreList.Count; i++)
        {
            _nameMenuTextArray[i].text = Leaderboard.scoreList[i].name;
            _scoreMenuTextArray[i].text = Leaderboard.scoreList[i].score.ToString();
        }
    }

    public void OnClickCredits()
    {
        EventSystem.current.firstSelectedGameObject = _creditsButton.gameObject;
        EventSystem.current.SetSelectedGameObject(_creditsButton.gameObject);
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
      if(Input.GetButtonDown("Cancel") && !_titleScreen.gameObject.activeInHierarchy && !_gameOverScreen.gameObject.activeInHierarchy && !_tutorialScreen.gameObject.activeInHierarchy)
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
        EventManager.StopListening(EventManager.TUTORIAL_EVENT, Tutorial);
    }
}
