﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager _instance;

    public static SoundManager Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<SoundManager>();

                //Tell unity not to destroy this object when loading a new scene!
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private AudioSource left;
    private AudioSource right;
    private AudioSource theme;
    private AudioSource clockwise;
    private AudioSource anticlockwise;
    private AudioSource hit;
    private List<AudioSource> clear;
    private List<AudioSource>.Enumerator clearIterator;
    private float lastClear = GameManager.GameTime;

    public enum Sound {
        Left,
        Right,
        Theme,
        Clockwise,
        AntiClockwise,
        Hit,
        Clear
    }

    private void Awake() {
        //Check if instance already exists
        if (_instance == null)

            //if not, set instance to this
            _instance = this;

        //If instance already exists and it's not this:
        else if (_instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    public void HandleThemeChanged(object sender, EventArgs args) {
        if (theme) {
            theme.Stop();
        }
        if (!left) {
            left = gameObject.AddComponent<AudioSource>();
        }
        if (!right) {
            right = gameObject.AddComponent<AudioSource>();
        }
        if (!theme) {
            theme = gameObject.AddComponent<AudioSource>();
            theme.volume = 0.4f;
        }
        if (!clockwise) {
            clockwise = gameObject.AddComponent<AudioSource>();
        }
        if (!anticlockwise) {
            anticlockwise = gameObject.AddComponent<AudioSource>();
        }
        if (!hit) {
            hit = gameObject.AddComponent<AudioSource>();
        }
        clear = new List<AudioSource>();

        for (int i = 1; i <= 5; i++) {
            clear.Add(gameObject.AddComponent<AudioSource>());
        }

        string themePathPrefix = "Themes/" + ThemeManager.Instance.CurrentThemeName + "/Sound/";

        left.clip = Resources.Load(themePathPrefix + "left") as AudioClip;
        right.clip = Resources.Load(themePathPrefix + "right") as AudioClip;
        theme.clip = Resources.Load(themePathPrefix + "theme") as AudioClip;
        clockwise.clip = Resources.Load(themePathPrefix + "clockwise") as AudioClip;
        anticlockwise.clip = Resources.Load(themePathPrefix + "anticlockwise") as AudioClip;
        hit.clip = Resources.Load(themePathPrefix + "hit") as AudioClip;
        for (int i = 1; i <= 5; i++) {
            clear[i - 1].clip = Resources.Load(themePathPrefix + i.ToString()) as AudioClip;
        }
        GetNewClearIterator();
        PlaySound(Sound.Theme);
    }

    public void PlaySound(Sound sound) {
        if (sound == Sound.Left) {
            left.Play();
        }
        if (sound == Sound.Right) {
            right.Play();
        }
        if (sound == Sound.Clockwise) {
            clockwise.Play();
        }
        if (sound == Sound.AntiClockwise) {
            anticlockwise.Play();
        }
        if (sound == Sound.Hit) {
            hit.Play();
        }
        if (sound == Sound.Theme) {
            theme.Play();
        }
        if (sound == Sound.Clear) {
            if (GameManager.GameTime - lastClear >= 2) {
                GetNewClearIterator();
                clearIterator.Current.Play();
                clearIterator.MoveNext();
            }
            else {
                clearIterator.Current.Play();
                if (!clearIterator.MoveNext()) {
                    GetNewClearIterator();
                }
            }
            lastClear = GameManager.GameTime;
        }
    }

    public void GetNewClearIterator() {
        clearIterator = clear.GetEnumerator();
        clearIterator.MoveNext();
    }

    // Use this for initialization
    private void Start() {
    }

    // Update is called once per frame
    private void Update() {
    }
}