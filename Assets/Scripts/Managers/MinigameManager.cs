using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
            { "Ramen", RamenInit }
        };

    }

    private void RamenInit()
    {
        GameObject player = GameManager.Instance.Player;
        Animator playerAnimator = player.GetComponent<Animator>();

        Time.timeScale = 1f;
        StartCoroutine(Helpers.ExecuteWithDelay(slowMotionDelay, () =>
        {
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
}
