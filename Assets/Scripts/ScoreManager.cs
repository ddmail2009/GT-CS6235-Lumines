﻿using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    public Text Score;
    public Text CurrentGameTime;
    public Text Clear;
    public Text Highscore;
    public Gameover Gameover;
    private float _gameStartTime;

    // Use this for initialization
    private void Start() {
        _gameStartTime = GameManager.GameTime;
        Highscore.text = PlayerPrefs.GetInt("highscore", 0).ToString();
    }

    // Update is called once per frame
    private void Update() {
        float gameOngoingTime = GameManager.GameTime - _gameStartTime;
        CurrentGameTime.text = string.Format("{0}:{1:00}", (int)gameOngoingTime / 60, (int)gameOngoingTime % 60);
    }

    public void AddScore(int score) {
        GetComponent<TextMesh>().text = (int.Parse(GetComponent<TextMesh>().text) + score).ToString();
        int preScore = int.Parse(Score.text);
        int afterScore = preScore + score;
        UpdateProgress(preScore, afterScore);
        if (afterScore > int.Parse(Highscore.text)) {
            Highscore.text = afterScore.ToString();
        }
        Score.text = afterScore.ToString();
    }

    public void ToZero() {
        GetComponent<TextMesh>().text = "0";
    }

    private void UpdateProgress(int preScore, int afterScore) {
        Clear.text = string.Format("{0} %", (afterScore % 50) * 2);
        if ((preScore / 50) != (afterScore / 50)) {
            ThemeManager.Instance.RandomTheme();
            GameManager.Instance.ChangeThemeDuringVoyage();
        }
    }

    public void GameOver() {
        Time.timeScale = 0;
        Gameover.ToggleEndMenu(int.Parse(Score.text));
    }
}