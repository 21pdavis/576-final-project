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
    public GameObject ramenPrefab;

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

    // Start is called before the first frame update
    void Start()
    {
        //MinigameInitFunctions = new()
        //{
        //    { "Ramen", RamenInit }
        //};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RamenInit()
    {
        GameObject player = GameManager.Instance.Player;

        Time.timeScale = 1; // TODO: maybe make 0.5 for slow-motion?

        // put ramen in player's hands
        GameObject ramen = Instantiate(ramenPrefab, player.transform.position, Quaternion.identity, player.transform);

        // switch input map to ramen minigame
        List<PlayerInput.ActionEvent> events = GameManager.Instance.gameObject.GetComponent<PlayerInput>().actionEvents.ToList();
        events[1].AddListener(ramen.GetComponent<Ramen>().Slingshot);

        // make player jump
        player.GetComponent<Animator>().SetTrigger("ramenJump");
    }
}
