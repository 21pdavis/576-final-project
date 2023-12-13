using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Ramen : MonoBehaviour
{
    public static GameObject RamenLiveObject;
    public static Vector3 PositionAtLaunch;

    [SerializeField] private Microwave microwave;
    [SerializeField] private Collider ramenPullBackCollider;
    [SerializeField] private float launchMultiplier; 
    [SerializeField] private LineRenderer leftArrowHead;
    [SerializeField] private LineRenderer rightArrowHead;

    private Rigidbody rb;
    private LineRenderer arrowStem;
    private Vector3 trajectory;
    // handle must be a class field instead of a var local to Slingshot because on mouse release, Slingshot is called again
    private IEnumerator updateTrajectoryCoroutineHandle;

    // Start is called before the first frame update
    private void Start()
    {
        microwave = GameObject.Find("Microwave").GetComponent<Microwave>();
        rb = GetComponent<Rigidbody>();
        arrowStem = GetComponent<LineRenderer>();
        trajectory = Vector3.zero;
        updateTrajectoryCoroutineHandle = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ramen"))
        {
            // TODO: Make this more sophisticated, this is fine for now as we determine what transitioning in/out of minigames looks like
            Debug.Log("Win!");
            Destroy(other.gameObject);
        }
    }

    private void VisualizeTrajectory()
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
            end + (end - leftEnd).normalized * leftArrowHead.startWidth / 2f, // eliminate gap at head of arrow by slightly shifting start point
            leftEnd
        });

        rightArrowHead.positionCount = 2;
        Vector3 rightEnd = end + Quaternion.Euler(0, 135f, 0) * trajectory.normalized;
        rightArrowHead.SetPositions(new Vector3[]
        {
            end + (end - rightEnd).normalized * rightArrowHead.startWidth / 2f, // eliminate gap at head of arrow by slightly shifting start point
            rightEnd
        });
    }

    private IEnumerator UpdateTrajectory()
    {
        while (true)
        {
            Ray ray = GameManager.Instance.currentCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(
                    ray,
                    out RaycastHit hit,
                    maxDistance: Mathf.Infinity,
                    layerMask: 1 << ramenPullBackCollider.gameObject.layer
                )
            )
            {
                trajectory = Vector3.ClampMagnitude(transform.position - hit.point, 2.5f);
                trajectory = new Vector3(trajectory.x, 0f, trajectory.z);
                VisualizeTrajectory();
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void Slingshot(CallbackContext context)
    {
        // player clicks in mouse button
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

        // player releases mouse button
        else if (context.canceled && updateTrajectoryCoroutineHandle != null) // release and slingshot in direction if already pulling
        {
            // stop updating the trajectory to finalize it
            StopCoroutine(updateTrajectoryCoroutineHandle);
            updateTrajectoryCoroutineHandle = null; // reset to null to "exit" pull

            // referenced in microwave.cs for calculating score
            PositionAtLaunch = transform.position;

            // clear lineRenderer points upon mouse release (erase arrow)
            arrowStem.positionCount = 0;
            leftArrowHead.positionCount = 0;
            rightArrowHead.positionCount = 0;

            // propel Ramen and detach from player
            transform.parent = null;
            rb.AddForce(trajectory * launchMultiplier, ForceMode.Impulse);
            if (Time.timeScale < 1f)
            {
                Time.timeScale = 1f;
            }

            // start ticking down timer for the game to conclude
            microwave.BeginEndSequence();
        }
    }
}
