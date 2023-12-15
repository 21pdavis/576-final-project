using UnityEngine;

public class Microwave : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ramen"))
        {
            ResourceManager.Instance.Energy += 15;
            ResourceManager.Instance.Hunger -= 25;
            Destroy(other.gameObject);
        }
    }
}