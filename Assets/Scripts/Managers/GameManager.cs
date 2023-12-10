using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public Camera currentCamera;
    public Light sun;

    private PlayerInput input;
    private Vector2Int positionInHouse;
    public enum GameState
    {
        Menu,
        DefaultRoom,
        MinigameRamen,
        Sleep,
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
        SceneManager.sceneLoaded += sceneLoadedFunction;
        Player = GameObject.FindGameObjectWithTag("Player");
        input = GetComponent<PlayerInput>();
        try {
            sun = GameObject.FindGameObjectWithTag("Sun").GetComponent<Light>();
        }
        catch (System.Exception) {
            sun = null;
        }
        
        //TransitionState(GameState.Menu);
        // TODO: make this not the default state
        TransitionState(GameState.DefaultRoom);
    }

    public void TransitionState(GameState to)
    {
        PopupManager.Instance.ClearPopups();

        switch (to)
        {
            case GameState.Menu:
                input.SwitchCurrentActionMap("Main");
                //PopupManager.Instance.InitPopupSequence("welcome");
                break;
            case GameState.MinigameRamen:
                input.SwitchCurrentActionMap("Ramen Minigame");

                // launch minigame
                Time.timeScale = 0;
                PopupManager.Instance.InitPopupSequence(
                    "ramenMinigameIntro",
                    onEndOfSequence: MinigameManager.Instance.MinigameInitFunctions["Ramen"]
                );
                break;
            case GameState.Sleep:
                MinigameManager.Instance.MinigameInitFunctions["Sleep"]();
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
                if(gridPos.x > -1 && gridPos.y > -1)
                    Player.GetComponent<PlayerFollow>().setPosition(gridPos);
                TransitionState(GameState.DefaultRoom);
                break;
            default:
                break;
        }
    }
    void Update() {
    }
}
