using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    //[SerializeField] private GameObject buttonOne, buttonTwo, buttonThree, buttonFour;
    [Header("MENU NAVIGATION")]
    [SerializeField] private GameObject[] _menuButtonsAray;
    [SerializeField] private GameObject[] _menuPositionsButtonsAray;
    [Space(5)]
    [SerializeField] private GameObject _menuSelectionHighlightItem;

    [Space(10)]
    [Header("GAME OVER NAVIGATION")]
    [SerializeField] private GameObject[] _gameOverButtonsArray;
    [SerializeField] private GameObject[] _gameOverPositionsButtonsArray;
    [Space(5)]
    [SerializeField] private GameObject _gameOverSelectionHighlightItem;

    [Space(10)]
    [Header("PAUSE NAVIGATION")]
    [SerializeField] private GameObject[] _pauseButtonsArray;
    [SerializeField] private GameObject[] _pausePositionsButtonsArray;
    [Space(5)]
    [SerializeField] private GameObject _pauseSelectionHighlightItem;

    delegate void DelAction();
    DelAction selectionUpdate;

    private static SelectionManager _manager;
    public static SelectionManager manager {
        get {
            return _manager;
        }
    }

    private void Awake()
    {
        if (_manager == null) _manager = this;

        else if (_manager != this) Destroy(gameObject);
    }

    void Start()
    {
        EventManager.StartListening(EventManager.GAME_OVER_EVENT,SetModeGameOver);
        EventManager.StartListening(EventManager.PAUSE_EVENT, SetModePause);
        EventManager.StartListening(EventManager.MENU_EVENT, SetModeMenu);

        SetModeMenu();
    }

    void SetModePause()
    {
        EventSystem.current.firstSelectedGameObject = _pauseButtonsArray[0];
        EventSystem.current.SetSelectedGameObject(_pauseButtonsArray[0]);
        selectionUpdate = DoUpdatePause;
    }

    void DoUpdatePause()
    {
        for (int i = 0; i < _pauseButtonsArray.Length; i++)
        {
            if (_pauseButtonsArray[i] == EventSystem.current.currentSelectedGameObject)
            {
                AkSoundEngine.PostEvent("Select", gameObject);
                _pauseSelectionHighlightItem.transform.position = _pausePositionsButtonsArray[i].transform.position;
            }
        }
    }

    //System D "public"
    public void SetModeMenu()
    {
        EventSystem.current.firstSelectedGameObject = _menuButtonsAray[0];
        EventSystem.current.SetSelectedGameObject(_menuButtonsAray[0]);
        selectionUpdate = DoUpdateMenu;
    }

    void DoUpdateMenu()
    {
        for (int i = 0; i < _menuButtonsAray.Length; i++)
        {
            if (_menuButtonsAray[i] == EventSystem.current.currentSelectedGameObject)
            {
                AkSoundEngine.PostEvent("Select", gameObject);
                _menuSelectionHighlightItem.transform.position = _menuPositionsButtonsAray[i].transform.position;
            }
        }
    }

    void SetModeGameOver()
    {
        EventSystem.current.firstSelectedGameObject = _gameOverButtonsArray[0];
        EventSystem.current.SetSelectedGameObject(_gameOverButtonsArray[0]);
        selectionUpdate = DoUpdateGameOver;
    }

    void DoUpdateGameOver()
    {
        for (int i = 0; i < _gameOverButtonsArray.Length; i++)
        {
            if (_gameOverButtonsArray[i] == EventSystem.current.currentSelectedGameObject)
            {
                AkSoundEngine.PostEvent("Select", gameObject);
                _gameOverSelectionHighlightItem.transform.position = _gameOverPositionsButtonsArray[i].transform.position;
            }
        }
    }

    private void Update()
    {
        if (selectionUpdate != null) selectionUpdate();
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventManager.GAME_OVER_EVENT, SetModeGameOver);
        EventManager.StopListening(EventManager.PAUSE_EVENT, SetModePause);
        EventManager.StopListening(EventManager.MENU_EVENT, SetModeMenu);
    }
}
