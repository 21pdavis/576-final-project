using UnityEngine;

public class Microwave : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ramen"))
        {
            // adjust resource values
            ResourceManager.Instance.Energy += 25;
            ResourceManager.Instance.Hunger -= 25;

            // add score based on distance launched
            Debug.Log($"Added {(int)(100f * Vector3.Distance(transform.position, Ramen.PositionAtLaunch))} score to total.");
            ResourceManager.Instance.Score += (int)(100f * Vector3.Distance(transform.position, Ramen.PositionAtLaunch));

            // necessary clean up (e.g., switching input mapping back to main)
            MinigameManager.Instance.MinigameCleanupFunctions["Ramen"]();

            Destroy(other.gameObject);
        }
    }
}