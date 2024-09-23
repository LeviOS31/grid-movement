using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public int currentx = 2;
    public int currenty = 3;

    //public int targetx = 0;
    //public int targety = 0;

    public GameObject gridcontroller;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ClickedGridSquare(hit.collider.gameObject);
            }
        }
    }

    void ClickedGridSquare(GameObject clickedObj)
    {
        gridcontroller.GetComponent<GridBehaviour>().endx = clickedObj.GetComponent<Gridstat>().x;
        gridcontroller.GetComponent<GridBehaviour>().endy = clickedObj.GetComponent<Gridstat>().y;

        gridcontroller.GetComponent<GridBehaviour>().setDistance();
    }

}
