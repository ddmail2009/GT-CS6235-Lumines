﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Group : MonoBehaviour {

    // Use this for initialization
    void Start() {
        Grid.CurrentGroup = transform;
    }

    bool GroupIsValidGridPosition(Vector3 GroupVector) {
        Vector2[] children = {new Vector2(GroupVector.x + 0.5f, GroupVector.y + 0.5f), new Vector2(GroupVector.x + 0.5f, GroupVector.y + 1.5f), new Vector2(GroupVector.x + 1.5f, GroupVector.y + 1.5f), new Vector2(GroupVector.x + 1.5f, GroupVector.y + 0.5f)};
        foreach (Vector2 child in children) {
            Vector2 v = Grid.RoundVector2(child);

            // Not inside Border?
            if (!Grid.InsideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (Grid.grid[(int)v.x, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    bool GroupIsValidGridPosition() {
        foreach (Transform child in transform) {
            Vector2 v = Grid.RoundVector2(child.position);

            // Not inside Border?
            if (!Grid.InsideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (Grid.grid[(int)v.x, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }

    private void UpdateGrid() {
        // Remove old children from grid
        for (int y = 0; y < Grid.Height; ++y)
            for (int x = 0; x < Grid.Width; ++x)
                if (Grid.grid[x, y] != null)
                    if (Grid.grid[x, y].parent == transform)
                        Grid.grid[x, y] = null;

        // Add new children to grid
        foreach (Transform child in transform) {
            Vector2 v = Grid.RoundVector2(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
        }
    }

    // Update is called once per frame
    void Update() {
        // Move Left
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            if (GroupIsValidGridPosition(transform.position + new Vector3(-1, 0, 0))) {
                transform.position += new Vector3(-1, 0, 0);
                // It's valid. Update grid.
                UpdateGrid();
            }
            else {

            }
        }

        // Move Right
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            if (GroupIsValidGridPosition(transform.position + new Vector3(1, 0, 0))) {
                transform.position += new Vector3(1, 0, 0);
                // It's valid. Update grid.
                UpdateGrid();
            }
            else {

            }
        }

        // Rotate
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            foreach (Transform child in transform) {
                Vector3 v = child.localPosition;

                if (v.x == 0.5 && v.y == 1.5) {
                    child.localPosition = new Vector3(1.5f, 1.5f, v.z);
                }
                else if (v.x == 1.5 && v.y == 1.5) {
                    child.localPosition = new Vector3(1.5f, 0.5f, v.z);
                }
                else if (v.x == 1.5 && v.y == 0.5) {
                    child.localPosition = new Vector3(0.5f, 0.5f, v.z);
                }
                else if (v.x == 0.5 && v.y == 0.5) {
                    child.localPosition = new Vector3(0.5f, 1.5f, v.z);
                }
                else {
                    throw new System.Exception();
                }
            }
            foreach (Transform child in transform) {
                Vector2 v = Grid.RoundVector2(child.position);
                Grid.grid[(int)v.x, (int)v.y] = child;
            }
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            if (GroupIsValidGridPosition(transform.position + new Vector3(0, -1, 0))) {
                transform.position += new Vector3(0, -1, 0);
                // It's valid. Update grid.
                UpdateGrid();
            }
            else {
                enabled = false;
                FindObjectOfType<Spawner>().spawnNext();

                int[] columnsHeight = Grid.ColumnFullUntilHeight();
                foreach (Transform child in transform) {
                    Vector3 v = child.localPosition;
                    Vector2 gridV = Grid.RoundVector2(child.position);
                    Grid.grid[(int)gridV.x, (int)gridV.y] = null;
                    if (v.x == 0.5 && v.y == 0.5) {
                        child.gameObject.GetComponent<Block>().DownTarget = columnsHeight[(int)gridV.x];
                        Grid.grid[(int) gridV.x, (int) columnsHeight[(int) gridV.x]] = child;
                        child.gameObject.GetComponent<Block>().GoDown = true;
                    }
                    else if (v.x == 0.5 && v.y == 1.5) {
                        child.gameObject.GetComponent<Block>().DownTarget = columnsHeight[(int)gridV.x] + 1;
                        Grid.grid[(int)gridV.x, (int)columnsHeight[(int)gridV.x] + 1] = child;
                        child.gameObject.GetComponent<Block>().GoDown = true;
                    }
                    else if (v.x == 1.5 && v.y == 0.5) {
                        child.gameObject.GetComponent<Block>().DownTarget = columnsHeight[(int)gridV.x];
                        Grid.grid[(int)gridV.x, (int)columnsHeight[(int)gridV.x]] = child;
                        child.gameObject.GetComponent<Block>().GoDown = true;
                    }
                    else if (v.x == 1.5 && v.y == 1.5) {
                        child.gameObject.GetComponent<Block>().DownTarget = columnsHeight[(int)gridV.x] + 1;
                        Grid.grid[(int)gridV.x, (int)columnsHeight[(int)gridV.x] + 1] = child;
                        child.gameObject.GetComponent<Block>().GoDown = true;
                    }
                    else {
                        throw new System.Exception();
                    }
                }
            }

        }
    }
}
