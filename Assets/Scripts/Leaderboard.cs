using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [Header("Test Roll Dice")]
    [SerializeField] Text _scoreText;
    int _localScore = 0;
    PlayerScore _localPlayer;
    int _localIndex = 0;

    [Header("Scores UI")]
    [SerializeField] Text[] _scoreTextArray;
    [Header("Names UI")]
    [SerializeField] Text[] _nameTextArray;

    [Space(5)]
    [Header("Enter Name")]
    [SerializeField] InputField _localPlayerName;
    [SerializeField] Button _sendButton;

    [SerializeField] int MAX_LEADERBOARDS = 3;

    public struct PlayerScore
    {
        public string name;
        public float score;

        public PlayerScore(string pName, float pScore)
        {
            name = pName;
            score = pScore;
        }
    }

    public List<PlayerScore> scoreList = new List<PlayerScore>();

    // Update is called once per frame
    void Start()
    {
       InitializeScoreList();
       UpdateUI();

        _sendButton.interactable = false;
    }

    public void OnClickTextField()
    {
        if (_localPlayerName.text != "") _sendButton.interactable = true;
        else _sendButton.interactable = false;
    }

    public void OnClickUpdateLeaderBoard()
    {
        _localPlayer = new PlayerScore(_localPlayerName.text, _localScore);
        CheckIfHighScore();
    }


    void SetPlayerPrefsLeaderBoard()
    {
        PlayerPrefs.SetInt("ScoreCount", scoreList.Count);

        for (var i = 0; i < scoreList.Count; i++)
        {
            PlayerPrefs.SetString("Name" + i, scoreList[i].name);
            PlayerPrefs.SetFloat("Score" + i, scoreList[i].score);
            PlayerPrefs.Save();
        }
    }

    void InitializeScoreList()
    {
        /*PlayerScore lPlayerOne = new PlayerScore("Emelinator", 999);
        PlayerScore lPlayerTwo = new PlayerScore("RetardedCat", 2);
        PlayerScore lPlayerThree = new PlayerScore("Escergo", 1);

        scoreList.Add(lPlayerOne);
        scoreList.Add(lPlayerTwo);
        scoreList.Add(lPlayerThree);*/

        for (int i = 0; i < PlayerPrefs.GetInt("ScoreCount", 0); i++)
        {
            PlayerScore lPlayer = new PlayerScore(PlayerPrefs.GetString("Name" + i), PlayerPrefs.GetFloat("Score" + i));
            scoreList.Add(lPlayer);
        }

        //SetPlayerPrefsLeaderBoard();
    }

    void UpdateUI()
    {
        //PlayerPrefs.SetInt("ScoreCount", scoreList.Count);
        _sendButton.interactable = false;

        for (int i = 0; i < scoreList.Count; i++)
        {
            _nameTextArray[i].text = scoreList[i].name;
            _scoreTextArray[i].text = scoreList[i].score.ToString();
        }
    }

    /*void UpdateUI()
    {
        for(int i = 0; i < scoreList.Count; i++)
        {
            _nameTextArray[i].text = PlayerPrefs.GetString("Name" + i);
            _scoreTextArray[i].text = PlayerPrefs.GetFloat("Score" + i).ToString();
        }
    }*/

    void CheckIfHighScore()
    {
        if (scoreList.Count != 0)
        {
            if (scoreList.Count < MAX_LEADERBOARDS) scoreList.Add(_localPlayer);
            print(scoreList.Count);
            for (int i = 0; i < scoreList.Count; i++)
            {
                if (_localPlayer.score > scoreList[i].score)
                {
                    _localIndex = i;
                    UpdateLeaderBoard();
                    return;
                }
            }
        }

        else AddScore();
    }

    void UpdateLeaderBoard()
    {
        int lMaxValue;

        if (scoreList.Count < MAX_LEADERBOARDS) lMaxValue = scoreList.Count;
        else lMaxValue = MAX_LEADERBOARDS;

        for(int i =_localIndex; i < lMaxValue - 1; i++)
        {
            scoreList[i + 1] = scoreList[i];
        }

        scoreList[_localIndex] = _localPlayer;

        SetPlayerPrefsLeaderBoard();
        UpdateUI();
    }

    void AddScore()
    {
        scoreList.Add(_localPlayer);
        SetPlayerPrefsLeaderBoard();
        UpdateUI();
    }

    public void RollDice()
    {
        _localScore = Random.Range(1, 7);
        _scoreText.text = _localScore.ToString();
    }

    public void Reset()
    {

        for(int i = 0; i < PlayerPrefs.GetInt("ScoreCount"); i++)
        {
            PlayerPrefs.DeleteKey("Name" + i);
            PlayerPrefs.DeleteKey("Score" + i);
        }

        PlayerPrefs.DeleteKey("ScoreCount");
    }
}
