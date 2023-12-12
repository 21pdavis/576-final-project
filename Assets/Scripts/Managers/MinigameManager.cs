using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }

    public Dictionary<string, Action> MinigameInitFunctions;

    [Header("Ramen")]
    [SerializeField]
    private GameObject ramenPrefab;

    [SerializeField]
    private float slowMotionDelay;

    [SerializeField, Range(0f, 1f)]
    private float slowMotionSpeed;
    
    [Header("Alarm")]
    [SerializeField]
    private Camera alarmCamera;

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

        MinigameInitFunctions = new()
        {
            { "Ramen", RamenInit },
            { "Alarm", AlarmInit },
            { "Sleep", SleepInit }
        };

    }

    private void RamenInit() {
        GameObject player = GameManager.Instance.Player;
        Animator playerAnimator = player.GetComponent<Animator>();

        Time.timeScale = 1f;
        StartCoroutine(Helpers.ExecuteWithDelay(slowMotionDelay, () => {
            Time.timeScale = slowMotionSpeed;
            player.GetComponent<PlayerAnimationEvents>().OnTimeSlow();
        }));

        // put ramen in player's hands
        Vector3 spawnToSideOfPlayer = player.transform.position + player.GetComponent<MeshFilter>().mesh.bounds.size.x * (-0.5f * player.transform.right);
        GameObject ramen = Instantiate(ramenPrefab, position: spawnToSideOfPlayer, rotation: Quaternion.identity, parent: player.transform);

        // switch input map to ramen minigame
        List<PlayerInput.ActionEvent> events = GameManager.Instance.gameObject.GetComponent<PlayerInput>().actionEvents.ToList();
        // TODO: un-hardcode this
        events.FirstOrDefault((e) => e.actionName.Contains("Slingshot")).AddListener(ramen.GetComponent<Ramen>().Slingshot);
        //events[1].AddListener(ramen.GetComponent<Ramen>().Slingshot);

        // make player jump
        playerAnimator.SetTrigger("ramenJump");
    }

    private void AlarmInit() {
        GameObject player = GameManager.Instance.Player;
        Animator playerAnimator = player.GetComponent<Animator>();

        //switch camera to alarm camera
        alarmCamera.enabled = true;

        Camera.main.gameObject.SetActive(false);

        Debug.Log("Alarm minigame init");
        GameObject alarmController = GameObject.Find("AlarmMiniGameController");
        alarmController.GetComponent<AlarmClock>().clockStopped = false;

    }


    private void SleepInit() {
        if (ResourceManager.Instance.Time >= 1260) {
            IEnumerator newDay() {
                yield return new WaitForSeconds(5f);
                ResourceManager.Instance.Time = 0;
                ResourceManager.Instance.Date += 1;
                GameManager.Instance.gridPos = new Vector2Int(56, 48);
                SceneManager.LoadScene("Wu");
                FindAnyObjectByType<TimeDisplay>().unDimScreen();
            }

            FindAnyObjectByType<TimeDisplay>().dimScreen(1f);
            StartCoroutine(newDay());
        } else {
            IEnumerator sleepFor1Hours() {
                float startTime = ResourceManager.Instance.Time;
                TimeController.Instance.Paused = true;
                yield return new WaitForSeconds(1f);
                while (ResourceManager.Instance.Time <= startTime + 60) {
                    yield return new WaitForSeconds(.1f);
                    ResourceManager.Instance.Time += 5;
                }
                
                GameManager.Instance.Player.GetComponent<Animator>().SetTrigger("WakeUp");
                FindAnyObjectByType<TimeDisplay>().unDimScreen();
                yield return new WaitForSeconds(1f);
                GameManager.Instance.Player.GetComponent<PlayerFollow>().unpause();
                TimeController.Instance.Paused = false;
            }
            FindAnyObjectByType<TimeDisplay>().dimScreen(.75f);

            StartCoroutine(sleepFor1Hours());
        }
    }
}
