using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Task {
    [SerializeField] Vector2Int gridCoord;
    [SerializeField] int priority;
    [SerializeField] int eventID;
    public Task() {
        gridCoord = new Vector2Int();
        priority = 2;
        eventID = 1;

    }

    public Task(int x, int z, int priority, int eventID) {
        gridCoord = new Vector2Int(x, z);
        this.priority = priority;
        this.eventID = eventID;
    }

    public Vector2Int getGridCoord() {
        return gridCoord;
    }

    public int getPriority() {
        return priority;
    }

    public int getEventId() {
        return eventID;
    }
    public int compare(Task task) {
        int compareValue = priority - task.priority;
        if(compareValue == 0) {

        }
        return compareValue;
    }
}

public class PlayerTasks : MonoBehaviour
{
    [SerializeField] List<Task> goalList; //x and z represents the grid coordinates, while y represents the priority
    [SerializeField] float stress = 0;
    [SerializeField] float hunger = 0;
    [SerializeField] float sleepyness = 0;
    [SerializeField] PlayerFollow player;
    [Range(-1, 100)] [SerializeField] int playerCommandPriority = 0;
    [SerializeField] Cursor cursor;
    // Start is called before the first frame update
    void Start()
    {
        cursor = FindAnyObjectByType<Cursor>().GetComponent<Cursor>();
        getNextTask();
    }
    public void addTask(Task task) {
        int count = 0;
        bool inserted = false;
        foreach(Task existingTask in goalList) {
            if(task.compare(existingTask) >= 0) {
                goalList.Insert(count, task);
                inserted = true;
                if(count == 0) {
                    Vector2Int gridCoord = task.getGridCoord();
                    player.setGoal(gridCoord.x, gridCoord.y, task.getEventId());
                }
                break;
            }
            count++;
        }
        if (goalList == null || goalList.Count == 0) {
            Vector2Int gridCoord = task.getGridCoord();
            player.setGoal(gridCoord.x, gridCoord.y, task.getEventId());
        }
        if (!inserted) {
            goalList.Add(task);
        }
    }
    public void finTask(int id) {
        if (goalList != null && goalList.Count != 0) {
            goalList.RemoveAt(0);
        }
        switch (id) {
            case 1:
                stress = -10;
                player.pause(5f);
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }
    }
    public void getNextTask() {
        if (goalList != null && goalList.Count != 0) {
            Vector2Int gridCoord = goalList[0].getGridCoord();
            player.setGoal(gridCoord.x, gridCoord.y, goalList[0].getEventId());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            (int, int) endGoalPos = cursor.getGridPos();
            Task userTask = new Task(endGoalPos.Item1, endGoalPos.Item2, playerCommandPriority, 0);
            addTask(userTask);
            //player.setGoal(endGoalPos.Item1, endGoalPos.Item2);

        }
        float prevStress = stress;
        float prevHunger = hunger;
        float prevSleepyness = sleepyness;

        stress += Time.deltaTime;
        hunger += Time.deltaTime;
        sleepyness += Time.deltaTime;
        if (stress >= 10 && prevStress < 10) {
            Task stressTask = new Task(40, 40, 50, 1);
            addTask(stressTask);
        }
    }
}
