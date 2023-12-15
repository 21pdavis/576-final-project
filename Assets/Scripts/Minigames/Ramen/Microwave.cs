using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script that handles microwave collision and concluding the ramen game
/// </summary>
public class Microwave : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreReportPrefab;

    private GameObject canvas;
    private bool hasWon;
    private int scoreGained;

    private void Start()
    {
        canvas = GameObject.Find("Canvas");
        hasWon = false;
        scoreGained = 0;
    }

    public IEnumerator RamenWinOrLose()
    {
        yield return new WaitForSecondsRealtime(5f);

        // TODO: shouldn't need this line except for debugging, but need it here for now because I'm manually changing the scenes
        canvas = GameObject.Find("Canvas");

        GameObject scoreReport = Instantiate(scoreReportPrefab, parent: canvas.transform);
        TextMeshProUGUI winLoseText = scoreReport.transform.Find("Win or Lose").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI scoreText = scoreReport.transform.Find("Score").GetComponent<TextMeshProUGUI>();
        Button continueButton = scoreReport.transform.Find("Continue").GetComponent<Button>();

        // necessary clean up (e.g., switching input mapping back to main)
        continueButton.onClick.AddListener(() =>
        {
            MinigameManager.Instance.RamenCleanup();
            Destroy(scoreReport);
        });

        if (hasWon)
        {
            // ramen made it into microwave (was destroyed)!
            winLoseText.text = "You Win!"; // TODO: make this more creative/specific to the ramen minigame
            scoreText.text = $"Score: {scoreGained}";
        }
        else
        {
            Ramen.RamenLiveObject.GetComponent<Rigidbody>().useGravity = true;

            // ramen never made it into microwave (still floating around)
            winLoseText.text = "You Lost Your Meal :(";
        }
    }

    /// <summary>
    /// Function called from Ramen.cs to ensure that the handle to the RamenWinOrLose coroutine is not destroyed with the Ramen GameObject
    /// </summary>
    public void BeginEndSequence()
    {
        StartCoroutine(RamenWinOrLose());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ramen"))
        {
            hasWon = true;

            // adjust resource values
            ResourceManager.Instance.Energy += 15;
            ResourceManager.Instance.Hunger -= 25;

            // add score based on distance launched
            scoreGained = (int)(100f * Vector3.Distance(transform.position, Ramen.PositionAtLaunch));
            Debug.Log($"Added {scoreGained} score to total.");
            ResourceManager.Instance.Score += scoreGained;

            Destroy(other.gameObject);
        }
    }
}