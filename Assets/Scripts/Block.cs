﻿using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

    public enum State {
        Normal,
        ToBeErased,
        ToBeErasedWhileFallingDown,
        InsideCurrentStreak
    };

    public bool GoDown = false;
    public int DownTarget;
    public int Type = -1;
    private State status = State.Normal;

    public State Status {
        get { return status; }
        set {
            if (value != status) {
                if (value == State.ToBeErased) {
                    if (GoDown) {
                        status = State.ToBeErasedWhileFallingDown;
                    }
                    else {
                        status = State.ToBeErased;
                        ChangeSprite(State.ToBeErased);
                    }
                }
                else if (value == State.Normal) {
                    ChangeSprite(State.Normal);
                }
            }
        }
    }

    private void ChangeSprite(State value) {
        if (value == State.ToBeErased) {
            gameObject.GetComponent<SpriteRenderer>().sprite = ThemeManager.CurrentTheme.ToBeErased;
        }
        else if (value == State.InsideCurrentStreak) {
            gameObject.GetComponent<SpriteRenderer>().sprite = ThemeManager.CurrentTheme.InsideClearance;
        }
        else if (value == State.Normal) {
            switch (Type) {
                case 0:
                    gameObject.GetComponent<SpriteRenderer>().sprite = ThemeManager.CurrentTheme.Block1;
                    gameObject.GetComponent<Block>().Type = 0;
                    break;
                case 1:
                    gameObject.GetComponent<SpriteRenderer>().sprite = ThemeManager.CurrentTheme.Block2;
                    gameObject.GetComponent<Block>().Type = 1;
                    break;
            }
        }
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (GoDown) {
	        DownToDepth();
	    }
	}

    public bool IsSameType(Block other) {
        return Type == other.Type;
    }

    private void DownToDepth() {
        Vector3 position = transform.position;
        Vector2 roundedPosition = Grid.RoundVector2(transform.position);
        if (position.y - 0.5 > DownTarget) {
            transform.position = new Vector3(position.x, position.y - 0.1f, position.z);
        }
        else {
            transform.position = new Vector3(position.x, roundedPosition.y + 0.5f, position.z);
            GoDown = false;
            if (status == State.ToBeErasedWhileFallingDown) {
                status = State.ToBeErased;
                ChangeSprite(State.ToBeErased);
            }
        }
    }
}
