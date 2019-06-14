using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{ 
    PlayerScore _localPlayer;

    [SerializeField] int MAX_LEADERBOARDS = 5;

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

    public static List<PlayerScore> scoreList = new List<PlayerScore>();

    // Update is called once per frame
    void Start()
    {
       InitializeScoreList();
       EventManager.StartListening(EventManager.SEND_NAME_EVENT, NameEnter);
    }

    public void NameEnter()
    {
        _localPlayer = new PlayerScore(UIManager.manager.localPlayerName.text, ScoreManager.manager.score);
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
        for (int i = 0; i < PlayerPrefs.GetInt("ScoreCount", 0); i++)
        {
            PlayerScore lPlayer = new PlayerScore(PlayerPrefs.GetString("Name" + i), PlayerPrefs.GetFloat("Score" + i));
            scoreList.Add(lPlayer);
        }

        //SetPlayerPrefsLeaderBoard();
    }

    void CheckIfHighScore()
    {
        bool lIsInHighScore = false;

        if(scoreList.Count < MAX_LEADERBOARDS) { 

            if (scoreList.Count == 0)
            {
                scoreList.Add(_localPlayer);

                UpdateLeaderBoard();

                return;
            }

            else
            {
                for (int i = 0; i < scoreList.Count; i++)
                {
                    if (_localPlayer.score > scoreList[i].score)
                    {
                        lIsInHighScore = true;
                        scoreList.Insert(i, _localPlayer);

                        UpdateLeaderBoard();

                        return;
                    }
                }

                if(!lIsInHighScore)
                {
                    scoreList.Add(_localPlayer);

                    UpdateLeaderBoard();

                    lIsInHighScore = false;
                    return;
                }
            }
        }

        else { 
            for (int i = 0; i < scoreList.Count; i++)
            {
                if (_localPlayer.score > scoreList[i].score)
                {
                    scoreList.Insert(i, _localPlayer);
                    scoreList.RemoveAt(scoreList.Count - 1);

                    UpdateLeaderBoard();

                    return;
                }
            }
        }
    }

    void UpdateLeaderBoard()
    {
        SetPlayerPrefsLeaderBoard();
        EventManager.TriggerEvent(EventManager.SEND_SCORE_EVENT);
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

    private void OnDisable()
    {
        EventManager.StopListening(EventManager.SEND_NAME_EVENT, NameEnter);
    }
}
