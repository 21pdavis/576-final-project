using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using static UnityEngine.InputSystem.InputAction;

public class Ramen : MonoBehaviour
{
    [SerializeField] private Collider RamenPullBackCollider;
    [SerializeField] private float launchMultiplier;

    private Rigidbody rb;
    private Vector3 trajectory;
    private Vector3 initialPullPosition;
    private IEnumerator updateTrajectoryCoroutineHandle;

    private Vector3 debugCurrentPullPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trajectory = Vector3.zero;
        updateTrajectoryCoroutineHandle = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(debugCurrentPullPosition, 1f);
        Gizmos.DrawLine(transform.position, debugCurrentPullPosition);
    }

    private IEnumerator UpdateTrajectory()
    {
        while (true)
        {
            Ray ray = GameManager.Instance.currentCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance: Mathf.Infinity, layerMask: 1 << RamenPullBackCollider.gameObject.layer))
            {
                debugCurrentPullPosition = hit.point;

                trajectory = transform.position - hit.point;
                trajectory = new Vector3(trajectory.x, 0f, trajectory.z);
                //trajectory.Normalize();
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
                    Debug.Log($"ray hit object {hit.transform.gameObject.name}");
                    initialPullPosition = hit.transform.position;
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

            // propel Ramen
            rb.AddForce(trajectory * launchMultiplier, ForceMode.Impulse);
        }
    }
}
