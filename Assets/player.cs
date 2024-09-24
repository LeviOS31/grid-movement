using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;

public class player : MonoBehaviour
{
    public int currentx = 2;
    public int currenty = 3;
    public bool moving = false;
    public List<GameObject> currentpath =  new List<GameObject>();
    public int maxdistance = 6;

    //public int targetx = 0;
    //public int targety = 0;

    public GameObject gridcontroller;
    void Start()
    {
        
    }

    void Update()
    {
        if(!moving){

            gridcontroller.GetComponent<GridBehaviour>().visualmaxdistance(maxdistance);

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    ClickedGridSquare(hit.collider.gameObject);
                }
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if(gridcontroller.GetComponent<GridBehaviour>().endx > -1 || gridcontroller.GetComponent<GridBehaviour>().endy > -1)
                {
                    moving = true;
                    currentpath = gridcontroller.GetComponent<GridBehaviour>().path;
                    move();
                }
            }
        }
        else{
            gridcontroller.GetComponent<GridBehaviour>().Clearavailable();
        }
    }

    async void move()
    {
        currentpath.Remove(currentpath.Last());        
        
        while(currentpath.Count > 0)
        {
            GameObject dest = currentpath.Last();
            Vector3 destpos = dest.transform.position;
            await MoveObject(transform.position, destpos, 0.25f);

            currentx = dest.GetComponent<Gridstat>().x;
            currenty = dest.GetComponent<Gridstat>().y;

            currentpath.Remove(currentpath.Last());    
        }
        
        gridcontroller.GetComponent<GridBehaviour>().ClearPath();
        moving = false;
    }

    async Task MoveObject(Vector3 start, Vector3 end, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            await Task.Yield();  // This yields execution until the next frame
        }

        // Ensure the final position is the end point
        transform.position = end;
    }

    void ClickedGridSquare(GameObject clickedObj)
    {
        gridcontroller.GetComponent<GridBehaviour>().endx = clickedObj.GetComponent<Gridstat>().x;
        gridcontroller.GetComponent<GridBehaviour>().endy = clickedObj.GetComponent<Gridstat>().y;

        gridcontroller.GetComponent<GridBehaviour>().setDistance(maxdistance);
    }

}
