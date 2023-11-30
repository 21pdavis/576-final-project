using UnityEngine;

public class Microwave : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ramen"))
        {
            Debug.Log("Win!");
            Destroy(other.gameObject);
        }
    }
}