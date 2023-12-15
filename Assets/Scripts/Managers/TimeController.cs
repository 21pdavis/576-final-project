using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance { get; private set; }
    [SerializeField] bool isPaused = false;
    [SerializeField] private float timeSpeed = 1f;
    [SerializeField] private float defaultSpeed = 1f;
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

    // Update is called once per frame
    void Update()
    {
        if (!isPaused) {
            ResourceManager.Instance.Time += timeSpeed * Time.deltaTime;
        }
        if(GameManager.Instance.sun != null) {
            GameManager.Instance.sun.transform.eulerAngles = new Vector3(180 * ((ResourceManager.Instance.Time - 360) / 770), -30, 0);
        }
    }

    public bool Paused{
        get => isPaused;
        set {
            isPaused = value;
        }
    }

    public float TimeSpeed {
        get => timeSpeed;
        set {
            timeSpeed = value;
        }
    }
    public float DefaultSpeed {
        get => defaultSpeed;
    }
}
