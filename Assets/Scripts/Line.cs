﻿using UnityEngine;

public class Line : MonoBehaviour {

    // Use this for initialization
    private void Start() {
    }

    // Update is called once per frame
    private void Update() {
        Vector3 previousPosition = transform.position;
        transform.position = new Vector3(0 + (16 - 0) * (GameManager.GameTime % 4) / 4, previousPosition.y, previousPosition.z);
        // x is from 0 to 15
        int x = (int)transform.position.x;
        if ((int)transform.position.x != (int)previousPosition.x) {
            Grid.Instance.JudgeInsideClearanceAtColumn(x);
            Grid.Instance.ClearBeforeColumn(x);
        }
    }
}