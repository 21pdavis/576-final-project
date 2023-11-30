using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using static UnityEngine.InputSystem.InputAction;

public class Ramen : MonoBehaviour
{
    [SerializeField] private Collider ramenPullBackCollider;
    [SerializeField] private float launchMultiplier;
    [SerializeField] private LineRenderer leftArrowHead;
    [SerializeField] private LineRenderer rightArrowHead;

    private Rigidbody rb;
    private LineRenderer arrowStem;
    private Vector3 trajectory;
    private IEnumerator updateTrajectoryCoroutineHandle;

    private Vector3 debugCurrentPullPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        arrowStem = GetComponent<LineRenderer>();
        trajectory = Vector3.zero;
        updateTrajectoryCoroutineHandle = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(debugCurrentPullPosition, 1f);
        Gizmos.DrawLine(transform.position, debugCurrentPullPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ramen"))
        {
            Debug.Log("Win!");
            Destroy(other.gameObject);
        }
    }

    private void UpdateLineRenderer()
    {
        arrowStem.positionCount = 2;

        Vector3 start = transform.position;
        Vector3 end = transform.position + trajectory;

        // draw arrow stem
        arrowStem.SetPosition(0, start);
        arrowStem.SetPosition(1, end);

        // draw arrow heads (think of left vs right as the side of the arrow head that you would see when looking at it from above if it was pointing up)
        leftArrowHead.positionCount = 2;
        Vector3 leftEnd = end + Quaternion.Euler(0, -135f, 0) * trajectory.normalized;
        leftArrowHead.SetPositions(new Vector3[]
        {
            end + (end - leftEnd).normalized * leftArrowHead.startWidth / 2f,
            leftEnd
        });

        rightArrowHead.positionCount = 2;
        Vector3 rightEnd = end + Quaternion.Euler(0, 135f, 0) * trajectory.normalized;
        rightArrowHead.SetPositions(new Vector3[]
        {
            end + (end - rightEnd).normalized * rightArrowHead.startWidth / 2f,
            rightEnd
        });
    }

    private IEnumerator UpdateTrajectory()
    {
        while (true)
        {
            Ray ray = GameManager.Instance.currentCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: Mathf.Infinity, layerMask: 1 << ramenPullBackCollider.gameObject.layer))
            {
                debugCurrentPullPosition = hit.point;

                trajectory = transform.position - hit.point;
                trajectory = new Vector3(trajectory.x, 0f, trajectory.z);
                UpdateLineRenderer();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void Slingshot(CallbackContext context)
    {
        if (context.started)
        {
            Ray ray = GameManager.Instance.currentCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // hit.collider instead of hit.transform for gameObject is important, since transform is inherited by collision child object
                if (hit.collider.gameObject.CompareTag("Ramen"))
                {
                    updateTrajectoryCoroutineHandle = UpdateTrajectory();
                    StartCoroutine(updateTrajectoryCoroutineHandle);
                }
            }
        }
        else if (context.canceled && updateTrajectoryCoroutineHandle != null) // release and slingshot in direction if already pulling
        {
            // stop updating the trajectory to finalize it
            StopCoroutine(updateTrajectoryCoroutineHandle);
            updateTrajectoryCoroutineHandle = null; // reset to null to "exit" pull

            // clear lineRenderer points upon mouse release
            arrowStem.positionCount = 0;
            leftArrowHead.positionCount = 0;
            rightArrowHead.positionCount = 0;

            // propel Ramen
            rb.AddForce(trajectory * launchMultiplier, ForceMode.Impulse);
        }
    }
}
