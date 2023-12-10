using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] bool isPaused = false;
    //The amount the bars are affected by per "tick"
    [SerializeField] private int stressBuildUp = 1;
    [SerializeField] private int hungerBuildUp = -1;
    [SerializeField] private int energyBuildUp = -1;
    //updates the statuses every updateTime seconds
    [SerializeField] private float updateTime = 2f;
    float timer = 0;
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (!isPaused) {
            timer += Time.deltaTime;
            if(timer >= updateTime) {
                timer--;
                ResourceManager.Instance.Stress += stressBuildUp;
                ResourceManager.Instance.Hunger += hungerBuildUp;
                ResourceManager.Instance.Energy += energyBuildUp;
                if (ResourceManager.Instance.Hunger >= 90) {

                    if (PlayerTasks.Instance != null && !PlayerTasks.Instance.containsTask(5)) {
                        FindAnyObjectByType<FoodInteractable>().onClick();
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

    public float Tick {
        get => updateTime;
        set {
            updateTime = value;
        }
    }
}
