using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] PlayerFollow player;
    [Range(-1, 105)] [SerializeField] int playerCommandPriority = 0;
    [SerializeField] Cursor cursor;
    public static PlayerTasks Instance { get; private set; }

    // Start is called before the first frame update
    void Awake() {
        // Instantiate Singleton (https://en.wikipedia.org/wiki/Singleton_pattern)
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        cursor = FindAnyObjectByType<Cursor>().GetComponent<Cursor>();
        getNextTask();
    }

    public bool containsTask(int eventID) {
        foreach (Task existingTask in goalList) {
            if (existingTask.getEventId() == eventID) {
                return true;
            }
        }
        return false;
    }
    public bool containsTask(int eventID, int priority) {
        foreach (Task existingTask in goalList) {
            if (existingTask.getEventId() == eventID && existingTask.getPriority() == priority) {
                return true;
            }
        }
        return false;
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
        Animator playerAnimator = GameManager.Instance.Player.GetComponent<Animator>();
        if (goalList != null && goalList.Count != 0) {
            goalList.RemoveAt(0);
        }

        getNextTask();
        switch (id) {
            case 1:
                player.pause(5f);
                break;
            case 2:
                player.pause();
                TimeController.Instance.Paused = true;
                PopupManager.Instance.InitPopupSequence(
                    "studyMinigameIntro",
                    onEndOfSequence: () =>
                    {
                        FindAnyObjectByType<StudyMiniGameEvent>().initGame();
                    }
                );
                //FindAnyObjectByType<StudyMiniGameEvent>().initGame();
                break;
            case 4:
                player.pause();
                playerAnimator.SetTrigger("BedJump");
                ResourceController.Instance.PauseTask = true;
                GameManager.Instance.TransitionState(GameManager.GameState.Sleep);
                break;
            case 5:
                SceneManager.LoadScene("Paul");
                break;
            case 6:
                SceneManager.LoadScene("Ryan");
                break;
            case 7:
                playerAnimator.SetBool("PlayingArcade", true);
                player.pause();
                MinigameManager.Instance.MinigameInitFunctions["Arcade"]();
                //GameManager.Instance.TransitionState(GameManager.GameState.Sleep);
                break;
            case 8:
                SceneManager.LoadScene("PartyScene");
                break;
            default:
                break;
        }
    }
    public void getNextTask() {
        if (goalList != null && goalList.Count != 0) {
            Debug.Log("goal" + goalList[0].getGridCoord());
            Debug.Log("id:" + goalList[0].getEventId());
            Vector2Int gridCoord = goalList[0].getGridCoord();
            player.setGoal(gridCoord.x, gridCoord.y, goalList[0].getEventId());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            (int, int) endGoalPos = cursor.getGridPos();
            Task userTask = new Task(endGoalPos.Item1, endGoalPos.Item2, playerCommandPriority, 3);
            addTask(userTask);
        }
    }
}
