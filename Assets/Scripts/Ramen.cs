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
            Debug.Log("Pulling back from ramen!");
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
                    updateTrajectoryCoroutineHandle = UpdateTrajectory();
                    StartCoroutine(updateTrajectoryCoroutineHandle);
                }
            }
        }
        else if (context.canceled && updateTrajectoryCoroutineHandle != null) // release and slingshot in direction if already pulling
        {
            Debug.Log("Released!");
            StopCoroutine(updateTrajectoryCoroutineHandle);
            updateTrajectoryCoroutineHandle = null; // reset to null to "exit" pull
        }
    }
}
