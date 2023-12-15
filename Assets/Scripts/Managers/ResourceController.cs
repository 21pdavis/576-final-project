using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public static ResourceController Instance { get; private set; }
    [SerializeField] bool isPaused = false;
    [SerializeField] bool[] specificPause = { false, false, false };
    [SerializeField] bool taskPause = false;
    //The amount the bars are affected by per "tick"
    [SerializeField] private int stressBuildUp = 1;
    [SerializeField] private int hungerBuildUp = -1;
    [SerializeField] private int energyBuildUp = -1;
    //updates the statuses every updateTime seconds
    [SerializeField] private float updateTime = 2f;
    [SerializeField] private float defaultUpdateTime = 2f;
    float timer = 0;
    // Start is called before the first frame update
    private void Awake() {
        // Instantiate Singleton (https://en.wikipedia.org/wiki/Singleton_pattern)
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    void changeStats() {
        if (!specificPause[0]) {
            ResourceManager.Instance.Stress += stressBuildUp;
        }
        if (!specificPause[1]) {
            ResourceManager.Instance.Hunger += hungerBuildUp;
        }
        if (!specificPause[2]) {
            ResourceManager.Instance.Energy += energyBuildUp;
        }
    }

    // Update is called once per frame
    void Update() {
        if (!isPaused) {
            timer += Time.deltaTime;
            if(timer >= updateTime) {
                timer-=updateTime;
                changeStats();
                if (ResourceManager.Instance.Hunger >= 90) {
                    if (PlayerTasks.Instance != null && !PlayerTasks.Instance.containsTask(5) && !taskPause) {
                        FindAnyObjectByType<FoodInteractable>().onClick();
                    }
                }
                if (ResourceManager.Instance.Energy == 0) {
                    if (PlayerTasks.Instance != null && !PlayerTasks.Instance.containsTask(4) && !taskPause) {
                        FindAnyObjectByType<BedInteractable>().onClick();
                    }
                }
            }
        }
    }

    public bool Paused {
        get => isPaused;
        set {
            isPaused = value;
        }
    }
    public bool PauseTask {
        get => taskPause;
        set {
            taskPause = value;
        }
    }
    public void PauseSpecific(int index, bool pause) {
        specificPause[index] = pause;
    }

    public float Tick {
        get => updateTime;
        set {
            if(updateTime != value) {
                changeStats();
                timer = 0;
            }
            updateTime = value;
        }
    }
    public float DefaultTick{
        get => defaultUpdateTime;
    }
}
