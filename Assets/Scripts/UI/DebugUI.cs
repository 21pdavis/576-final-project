using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GameState = GameManager.GameState;

public class DebugUI : MonoBehaviour
{
    public Button Menu;
    public Button DefaultRoom;
    public Button Ramen;
    public Button Alarm;
    public Button Sleep;
    public Button WakingUp;
    public Button EndGame;

    // Start is called before the first frame update
    void Start()
    {
        Menu.onClick.AddListener(() => GameManager.Instance.TransitionState(GameState.Menu));
        DefaultRoom.onClick.AddListener(() => SceneManager.LoadScene("Wu"));
        Ramen.onClick.AddListener(() => SceneManager.LoadScene("Paul"));
        Alarm.onClick.AddListener(() => SceneManager.LoadScene("Ryan"));
        Sleep.onClick.AddListener(() => GameManager.Instance.TransitionState(GameState.Sleep));
        WakingUp.onClick.AddListener(() => GameManager.Instance.TransitionState(GameState.WakingUp));
        EndGame.onClick.AddListener(() => GameManager.Instance.TransitionState(GameState.EndGame));
    }
}
