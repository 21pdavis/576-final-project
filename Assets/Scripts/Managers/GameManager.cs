using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public Camera currentCamera;
    public Light sun;

    private Vector2Int positionInHouse;
    public PlayerInput Input { get; private set; }
    public List<PlayerInput.ActionEvent> InputEvents { get; private set; }

    public enum GameState
    {
        Menu,
        DefaultRoom,
        MinigameRamen,
        MinigameAlarm,
        Sleep,
        WakingUp,
        EndGame
    }

    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; set; }

    public Vector2Int gridPos {
        get => positionInHouse;
        set => positionInHouse = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Input = GetComponent<PlayerInput>();
        InputEvents = Input.actionEvents.ToList();

        SceneManager.sceneLoaded += sceneLoadedFunction;
        Player = GameObject.FindGameObjectWithTag("Player");
        Input = GetComponent<PlayerInput>();
        try {
            sun = GameObject.FindGameObjectWithTag("Sun").GetComponent<Light>();
        }
        catch (System.Exception) {
            sun = null;
        }
        
        //TransitionState(GameState.Menu);
        // TODO: make this not the default state
        TransitionState(GameState.MinigameAlarm);
    }

    public void TransitionState(GameState to)
    {
        PopupManager.Instance.ClearPopups();

        switch (to)
        {
            case GameState.Menu:
                TimeController.Instance.Paused = true;
                Input.SwitchCurrentActionMap("Main");
                Input.SwitchCurrentActionMap("Main");
                //PopupManager.Instance.InitPopupSequence("welcome");
                break;
            case GameState.MinigameRamen:
                TimeController.Instance.Paused = false;
                Input.SwitchCurrentActionMap("Ramen Minigame");

                // launch minigame
                Time.timeScale = 0;
                PopupManager.Instance.InitPopupSequence(
                    "ramenMinigameIntro",
                    onEndOfSequence: MinigameManager.Instance.MinigameInitFunctions["Ramen"]
                );
                break;
            case GameState.Sleep:
                TimeController.Instance.Paused = false;
                MinigameManager.Instance.MinigameInitFunctions["Sleep"]();
                break;
            case GameState.DefaultRoom:
                TimeController.Instance.Paused = false;
                break;
            case GameState.MinigameAlarm:
                TimeController.Instance.Paused = true;
                break;
            
            case GameState.EndGame:

                break;
        }
    }

    void sceneLoadedFunction(Scene scene, LoadSceneMode mode) {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log("Handle:" + scene.handle);
        currentCamera = Camera.main;
        Player = GameObject.FindGameObjectWithTag("Player");
        try {
            sun = GameObject.FindGameObjectWithTag("Sun").GetComponent<Light>();
        }
        catch (System.Exception) {
            sun = null;
        }
        switch (scene.name){
            case "Paul":
                TransitionState(GameState.MinigameRamen);
                break;
            case "Wu":
                if (gridPos.x > -1 && gridPos.y > -1)
                    Player.GetComponent<PlayerFollow>().pos = (gridPos);
                TransitionState(GameState.DefaultRoom);
                break;
            case "Ryan":
                TransitionState(GameState.MinigameAlarm);
                break;
            default:
                break;
        }
    }
    void Update() {
    }
}
