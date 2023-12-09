using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGridFromScene : MonoBehaviour
{
    [SerializeField] MapGrid grid;
    [SerializeField] bool drawHitboxes;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 100; i++) {
            for(int j = 0; j < 100; j++) {
                Ray ray = new Ray(grid.gridToWorldPoint(i, j) + new Vector3(0, -10, 0), Vector3.up);
                RaycastHit hit;
                LayerMask mask = LayerMask.GetMask("Object");
                if (Physics.Raycast(ray, out hit, 100, mask)) {
                    grid.addObject(i, j, -2, hit.transform.parent.gameObject);
                    if (drawHitboxes) {
                        GameObject createdObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        createdObject.transform.position = grid.gridToWorldPoint(i, j) + Vector3.up;
                        createdObject.transform.localScale = new Vector3(.8f, 1, .8f);
                        createdObject.GetComponent<Renderer>().material.color = Color.red;
                    }
                    
                } else {
                    
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
