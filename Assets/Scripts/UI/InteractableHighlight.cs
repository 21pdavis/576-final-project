using UnityEngine;

public class InteractableHighlight : MonoBehaviour
{
    [SerializeField]
    private GameObject indicatorPrefab;

    [SerializeField]
    private float indicatorYOffset;

    [SerializeField]
    private float bounceSpeed;

    [SerializeField]
    private float indicatorLength;

    //private LineRenderer line;
    private GameObject liveIndicator;
    private float bounceRadians;
    private float startY;

    [SerializeField] bool useCursor = false;

    // Start is called before the first frame update
    void Start()
    {
        bounceRadians = 0f;
        startY = transform.position.y + indicatorYOffset;
    }

    private void Update()
    {
        if (liveIndicator != null)
        {
            bounceRadians = (bounceRadians + Time.deltaTime * bounceSpeed * (Mathf.PI / 180)) % (2 * Mathf.PI);
            liveIndicator.transform.position = new Vector3(
                liveIndicator.transform.position.x,
                startY + Mathf.Sin(bounceRadians),
                liveIndicator.transform.position.z
            );
        }
    }

    private void OnMouseEnter()
    {
        if (!useCursor) {
            Debug.Log("Instantiating");
            liveIndicator = Instantiate(
                indicatorPrefab,
                new Vector3(transform.position.x, startY, transform.position.z),
                Quaternion.identity,
                transform.parent
            );
        }
    }

    private void OnMouseExit()
    {
        if (!useCursor) {
            Debug.Log("Destroying");
            Destroy(liveIndicator);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.transform.name);
        if (useCursor && other.CompareTag("Cursor")) {
            Debug.Log("Instantiating");
            liveIndicator = Instantiate(
                indicatorPrefab,
                new Vector3(transform.position.x, startY, transform.position.z),
                Quaternion.identity,
                transform.parent
            );
        }
    }

    private void OnTriggerExit(Collider other) {
        if (useCursor && other.CompareTag("Cursor")) {
            Debug.Log("Destroying");
            Destroy(liveIndicator);
        }
    }
}
