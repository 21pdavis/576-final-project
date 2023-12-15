using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHouseInteractible : Interactable
{
    // Start is called before the first frame update
    public PlayerTasks tasks;
    public MapGrid grid;
    // Start is called before the first frame update
    void Start() {
        tasks = FindAnyObjectByType<PlayerTasks>();
        grid = FindAnyObjectByType<MapGrid>();
    }

    // Update is called once per frame
    void Update() {

    }
    public override void onClick() {
        base.onClick();
        foreach (Transform child in transform.parent) {
            if (child.name.Contains("Entrance")) {
                (int, int) gridPos = grid.worldToGridPoint(child.transform.position.x, child.transform.position.z);
                tasks.addTask(new Task(gridPos.Item1, gridPos.Item2, 95, 8));
            }
        }
    }
}
