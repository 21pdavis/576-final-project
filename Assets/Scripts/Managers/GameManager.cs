using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public Camera currentCamera;

    private PlayerInput input;

    public enum GameState
    {
        Menu,
        MinigameRamen
    }

    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; set; }

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
        input = GetComponent<PlayerInput>();

        //TransitionState(GameState.Menu);
        TransitionState(GameState.MinigameRamen);
    }

    public void TransitionState(GameState to)
    {
        PopupManager.Instance.ClearPopups();

        switch (to)
        {
            case GameState.Menu:
                //PopupManager.Instance.InitPopupSequence("welcome");
                break;
            case GameState.MinigameRamen:
                input.SwitchCurrentActionMap("Ramen Minigame");
                break;
        }
    }
}
