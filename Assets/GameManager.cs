using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu
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
        TransitionState(GameState.Menu);
    }

    public void TransitionState(GameState to)
    {
        PopupManager.Instance.ClearPopups();

        switch (to)
        {
            case GameState.Menu:
                //PopupManager.Instance.InitPopupSequence("welcome");
                break;
        }
    }
}
