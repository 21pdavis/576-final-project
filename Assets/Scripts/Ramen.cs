using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Ramen : MonoBehaviour
{
    private Vector3 trajectory;
    private Vector3 initialPullPosition;
    private IEnumerator updateTrajectoryCoroutineHandle;

    // Start is called before the first frame update
    void Start()
    {
        trajectory = Vector3.zero;
        initialPullPosition = Vector3.zero;
        updateTrajectoryCoroutineHandle = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrawGizmos()
    {
        
    }

    private void PropelRamen()
    {

    }

    private IEnumerator UpdateTrajectory()
    {
        while (true)
        {
            //Debug.Log("Pulling back from ramen!");
            Vector3 mouseWorldPosition = GameManager.Instance.currentCamera.ScreenToWorldPoint(Input.mousePosition);

            // put position level with initial pull position
            //mouseWorldPosition = new Vector3(mouseWorldPosition.x, initialPullPosition.y, mouseWorldPosition.z);

            trajectory = (initialPullPosition - mouseWorldPosition).normalized * 10f;
            Debug.Log(trajectory);
            Debug.DrawRay(initialPullPosition, trajectory, Color.cyan, 0.1f, false);
            //Debug.DrawRay(initialPullPosition, Vector3.up * 100f, Color.cyan, 1f);

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
                if (hit.transform.gameObject.CompareTag("Ramen"))
                {
                    initialPullPosition = hit.transform.position;
                    updateTrajectoryCoroutineHandle = UpdateTrajectory();
                    StartCoroutine(updateTrajectoryCoroutineHandle);
                }
            }
        }
        else if (context.canceled && updateTrajectoryCoroutineHandle != null) // release and slingshot in direction if already pulling
        {
            //Debug.Log("Released!");
            StopCoroutine(updateTrajectoryCoroutineHandle);
            updateTrajectoryCoroutineHandle = null; // reset to null to "exit" pull
        }
    }
}
