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

    private bool hasWon;
    private int score;

    private void Start()
    {
        hasWon = false;
        score = 0;
    }

    public IEnumerator RamenWinOrLose()
    {
        yield return new WaitForSecondsRealtime(2.5f);

        GameObject canvas = GameObject.Find("Canvas");
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
            winLoseText.text = "Your Ramen was Delicious!";
            scoreText.text = $"Score: {score}";
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
            ResourceManager.Instance.Energy += 50;
            ResourceManager.Instance.Hunger -= 50;

            // add score based on distance launched
            score = (int)(100f * Vector3.Distance(transform.position, Ramen.PositionAtLaunch));
            Debug.Log($"Added {score} score to total.");
            ResourceManager.Instance.minigameScores["Ramen"].Add(score);

            Destroy(other.gameObject);
        }
    }
}