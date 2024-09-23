using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class player : MonoBehaviour
{
    public int currentx = 2;
    public int currenty = 3;
    public bool moving = false;
    public List<GameObject> currentpath =  new List<GameObject>();

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

        if (Input.GetKey(KeyCode.Space) && !moving)
        {
            if(gridcontroller.GetComponent<GridBehaviour>().endx > -1 || gridcontroller.GetComponent<GridBehaviour>().endy > -1)
            {
                moving = true;
                currentpath = gridcontroller.GetComponent<GridBehaviour>().path;
                move();
            }
        }
    }

    void move()
    {
        currentpath.Remove(currentpath.Last());
        Debug.Log(currentpath);
        while(currentpath.Count > 0)
        {
            
        }
    }

    void ClickedGridSquare(GameObject clickedObj)
    {
        gridcontroller.GetComponent<GridBehaviour>().endx = clickedObj.GetComponent<Gridstat>().x;
        gridcontroller.GetComponent<GridBehaviour>().endy = clickedObj.GetComponent<Gridstat>().y;

        gridcontroller.GetComponent<GridBehaviour>().setDistance();
    }

}
