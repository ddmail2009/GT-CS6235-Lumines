﻿/*
 *  Lumines clone in Unity
 *
 *  Copyright (C) 2016 Zizheng Wu <me@zizhengwu.com>
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Gameover : MonoBehaviour {
    #region Singleton
    private static Gameover _instance = null;
    public static Gameover Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<Gameover>();
            }
            return _instance;
        }
    }
    private void Awake() {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }
    #endregion

    public Text Score;
    public GameObject AchiveHighscore;

    public void Start() {
        gameObject.SetActive(false);
    }

    public void ToggleEndMenu() {
        SoundManager.Instance.CmdStopTheme();
        gameObject.SetActive(true);

        int score = GameStatusSyncer.Instance.GameScore;
        Score.text = string.Format("Score          {0}", score.ToString());
        int oldHighscore = PlayerPrefs.GetInt("highscore", 0);
        if (score > oldHighscore) {
            AchiveHighscore.SetActive(true);
            PlayerPrefs.SetInt("highscore", score);
        }
    }

    public void BackToStartScreen() {
        NetworkManager ntmanager = FindObjectOfType<NetworkManager>();

        if (FindObjectOfType<GameManager>() != null)
            ntmanager.StopHost();
        else
            ntmanager.StopClient();
        

        Time.timeScale = 1;
    }
}