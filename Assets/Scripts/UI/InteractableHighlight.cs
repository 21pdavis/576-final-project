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
    //private float endY;

    // Start is called before the first frame update
    void Start()
    {
        //line = GetComponent<LineRenderer>();
        bounceRadians = 0f;
        startY = transform.position.y + indicatorYOffset;
        //endY = startY + indicatorLength;
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

        //if (line.positionCount > 0)
        //{
        //    Vector3 pos0 = line.GetPosition(0);
        //    Vector3 pos1 = line.GetPosition(1);

        //    bounceRadians = (bounceRadians + Time.deltaTime * bounceSpeed * Mathf.PI / 180) % (2 * Mathf.PI);

        //    pos0.y += Mathf.Sin(bounceRadians);
        //    pos1.y += Mathf.Sin(bounceRadians);

        //    line.SetPositions(new Vector3[] 
        //    {
        //        pos0,
        //        pos1
        //    });
        //}
    }

    private void OnMouseEnter()
    {
        Debug.Log("Instantiating");
        liveIndicator = Instantiate(
            indicatorPrefab,
            new Vector3(transform.parent.position.x, startY, transform.parent.position.z),
            Quaternion.identity,
            transform.parent
        );

        //line.positionCount = 2;
        //line.SetPositions(new Vector3[]
        //{
        //    new Vector3(transform.position.x, startY, transform.position.z),
        //    new Vector3(transform.position.x, endY, transform.position.z)
        //});
    }

    private void OnMouseExit()
    {
        Debug.Log("Destroying");
        Destroy(liveIndicator);

        //line.positionCount = 0;
        //bounceRadians = 0f;
    }
}
