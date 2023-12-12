using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableInteractible : Interactable 
{
    public PlayerTasks tasks;
    // Start is called before the first frame update
    void Start()
    {
        tasks = FindAnyObjectByType<PlayerTasks>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void onClick() {
        base.onClick();
        tasks.addTask(new Task(30, 50, 100, 2));
    }

}
