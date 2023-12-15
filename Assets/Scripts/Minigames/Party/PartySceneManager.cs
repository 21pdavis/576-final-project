using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartySceneManager : MonoBehaviour {

    public static PartySceneManager PM;
    public float gameSecondsLength;
    public GameObject boidPrefab;
    public int numBoids = 20;
    public GameObject[] allBoids;
    public Vector3 boidLimits = new Vector3(5.0f, 5.0f, 5.0f);
    public Vector3 goalPos = Vector3.zero;
    public GameObject scoreReportPrefab;

    public int numBoidsHitByPlayer = 0;
    public float startTime { private get; set; }
    private bool gameOver;

    [Header("Boid Settings")]
    [Range(0.0f, 5.0f)] public float minSpeed;
    [Range(0.0f, 5.0f)] public float maxSpeed;
    [Range(1.0f, 10.0f)] public float neighbourDistance;
    [Range(1.0f, 5.0f)] public float rotationSpeed;

    void Start() {
        startTime = Mathf.Infinity;
        gameOver = false;

        PopupManager.Instance.InitPopupSequence("PartyMinigame",
            onEndOfSequence: MinigameManager.Instance.PartyInit
        );
    }

    public void InitBoids() {       
        allBoids = new GameObject[numBoids];

        for (int i = 0; i < numBoids; ++i) {

            Vector3 pos = this.transform.position + new Vector3(
                Random.Range(0, boidLimits.x),
                Random.Range(-boidLimits.y, boidLimits.y),
                Random.Range(-boidLimits.z, boidLimits.z));

            allBoids[i] = Instantiate(boidPrefab, pos, Quaternion.identity);
        }

        PM = this;
        goalPos = this.transform.position;
    }

    void Update() {
        if (Time.time > startTime + gameSecondsLength && !gameOver)
        {
            gameOver = true;
            GameManager.Instance.SetGamePaused(true);

            GameObject scoreReport = Instantiate(scoreReportPrefab, parent: GameObject.Find("Canvas").transform);
            TextMeshProUGUI scoreText = scoreReport.transform.Find("Score").GetComponent<TextMeshProUGUI>();
            Button continueButton = scoreReport.transform.Find("Continue").GetComponent<Button>();

            // necessary clean up (e.g., switching input mapping back to main)
            continueButton.onClick.AddListener(() =>
            {
                Destroy(scoreReport);
                MinigameManager.Instance.PartyCleanup();
            });

            // adjust score
            int score = Mathf.Max(0, 500 - numBoidsHitByPlayer * 10);
            scoreText.text = $"Score: {score}";

            // adjust resources
            ResourceManager.Instance.Stress -= score / 5;
            ResourceManager.Instance.minigameScores["Party"].Add(score);
        }

        if (Random.Range(0, 100) < 10) {

            goalPos = this.transform.position + new Vector3(
                Random.Range(-boidLimits.x, boidLimits.x),
                Random.Range(-boidLimits.y, boidLimits.y),
                Random.Range(-boidLimits.z, boidLimits.z));
        } else {
            //set goal position to be players position
            goalPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        }
    }
}