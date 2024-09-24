using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridBehaviour : MonoBehaviour
{
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0,0,0);
    public GameObject[,] gridarray;
    public int startx = 1;
    public int starty = 1;
    public int endx = -1;
    public int endy = -1;
    public List<GameObject> path = new List<GameObject>();
    //public Material pathMat;
    //public Material gridMat;
    //public Material availableMat;
    public GameObject playerchar;
    // Start is called before the first frame update
    private void Awake() {

        gridarray = new GameObject[columns, rows];

        if(gridPrefab){
            GenerateGrid();
        }   
        else{
            Debug.Log("ow ow this is wrong");
        }
    }
    
    void Start()
    {
        initalsetup();
        //setDistance();
    }
    
    void Update()
    {
        
    }

    void GenerateGrid()
    {
        for( int i = 0; i < columns; i++){
            for (int j = 0; j < rows; j++){
                GameObject obj = Instantiate(gridPrefab, new Vector3 ( leftBottomLocation.x + scale * i, 0, leftBottomLocation.z + scale * j), Quaternion.identity);
                obj.SetActive(true);
                obj.transform.SetParent(gameObject.transform);
                obj.GetComponent<Gridstat>().x = i;
                obj.GetComponent<Gridstat>().y = j;
                gridarray[i,j] = obj;
            }
        }
    }

    public void setDistance(int maxdistance){
        initalsetup();

        for (int step = 1; step < maxdistance; step++)
        {
            bool pathFound = false; // Add a flag to break the loop when path is found

            foreach (GameObject obj in gridarray)
            {
                Gridstat stats = obj.GetComponent<Gridstat>();

                if (stats.visited == step - 1) // Decrement outside the condition
                {
                    if (stats.x == endx && stats.y == endy)
                    {
                        //Debug.Log(stats.x + " " + stats.y);
                        Debug.Log("found path");
                        pathFound = true; // Mark path found
                        break; // Exit early when path is found
                    }
                    TestEightDirections(stats.x, stats.y, step);
                }
                
               //Debug.Log(stats.x + " " + stats.y + " " + stats.visited);
            }

            if (pathFound){
                break; // Exit outer loop when the path is found
            }

        }
        setpath();
    }

    void setpath(){
        int step;
        int x = endx;
        int y = endy;
        List<GameObject> templist = new List<GameObject>();

        path.Clear();

        if(gridarray[x,y] && gridarray[x,y].GetComponent<Gridstat>().visited > 0){
            path.Add(gridarray[x,y]);
            step = gridarray[x,y].GetComponent<Gridstat>().visited - 1;
        }
        else{
            Debug.Log("this is very very bad");
            return;
        }

        while (step >= 0)
        {
            if (Testdirection(x, y, step, 1))
            {
                y += 1; // Move up
            }
            else if (Testdirection(x, y, step, 3))
            {
                x += 1; // Move right
            }
            else if (Testdirection(x, y, step, 5))
            {
                y -= 1; // Move down
            }
            else if (Testdirection(x, y, step, 7))
            {
                x -= 1; // Move left
            }
            else if (Testdirection(x, y, step, 2))
            {
                x += 1;
                y += 1; // Move up-right
            }
            else if (Testdirection(x, y, step, 4))
            {
                x += 1;
                y -= 1; // Move down-right
            }
            else if (Testdirection(x, y, step, 6))
            {
                x -= 1;
                y -= 1; // Move down-left
            }
            else if (Testdirection(x, y, step, 8))
            {
                x -= 1;
                y += 1; // Move up-left
            }
            else
            {
                Debug.Log("No valid direction found");
                break;
            }

            // Add the new grid position to the path
            path.Add(gridarray[x, y]);

            // Update the step to the next point's visited value
            step--;
        }

        Debug.Log(path.Count);
        foreach(GameObject obj in gridarray)
        {
            obj.GetComponent<Renderer>().material.color = new Color(0f,0f,1f,0.5f);
        }

        foreach(GameObject obj in path)
        {
            Debug.Log(obj.GetComponent<Gridstat>().x + " " + obj.GetComponent<Gridstat>().y);

            obj.GetComponent<Renderer>().material.color = new Color(1f,0f,0f,0.5f);
        }
    }

    void initalsetup(){

        startx = playerchar.GetComponent<player>().currentx;
        starty = playerchar.GetComponent<player>().currenty;

        GameObject objpos = gridarray[startx, starty];
        playerchar.transform.position = objpos.transform.position;

        foreach(GameObject obj in gridarray)
        {
            obj.GetComponent<Gridstat>().visited = -1;
        }

        gridarray[startx, starty].GetComponent<Gridstat>().visited = 0;
    }

    public void Clearavailable(){
        foreach(GameObject obj in gridarray)
        {
            obj.GetComponent<Renderer>().material.color = new Vector4(0f,0f,1f,0.5f);
        }
    }

    public void ClearPath(){
        path.Clear();

        foreach (GameObject obj in gridarray) {
            obj.GetComponent<Renderer>().material.color = new Vector4(0f,0f,1f,0.5f);
        }
    }

    bool Testdirection(int x, int y, int step, int direction)
    {
        // 4 direction: 1 = up, 3 = right, 5 = down, 7 = left 
        // 8 direction: 1 = up, 2 = up-right, 3 = right, 4 = down-right, 5 = down ,6 = down-left ,7 = left ,8 = up-left

        switch(direction){
            case 1:
                if(y + 1 < rows && gridarray[x,y+1] && gridarray[x,y+1].GetComponent<Gridstat>().visited == step){
                    return true;
                }
                else{
                    return false;
                }

            case 2:
                if(y + 1 < rows && x + 1 < columns && gridarray[x + 1,y + 1] && gridarray[x + 1,y + 1].GetComponent<Gridstat>().visited == step){
                    return true;
                }
                else{
                    return false;
                }
                
            case 3:
                if(x + 1 < columns && gridarray[x + 1,y] && gridarray[x + 1,y].GetComponent<Gridstat>().visited == step){
                    return true;
                }
                else{
                    return false;
                }

            case 4:
                if(y - 1 > -1 && x + 1 < columns && gridarray[x + 1,y - 1] && gridarray[x + 1,y - 1].GetComponent<Gridstat>().visited == step){
                    return true;
                }
                else{
                    return false;
                }

            case 5:
                if(y - 1 > -1 && gridarray[x,y-1] && gridarray[x,y-1].GetComponent<Gridstat>().visited == step){
                    return true;
                }
                else{
                    return false;
                }

            case 6:
                if(y - 1 > -1 && x - 1 > -1 && gridarray[x - 1,y - 1] && gridarray[x - 1,y - 1].GetComponent<Gridstat>().visited == step){
                    return true;
                }
                else{
                    return false;
                }

            case 7:
                if(x - 1 > -1 && gridarray[x-1,y] && gridarray[x-1,y].GetComponent<Gridstat>().visited == step){
                    return true;
                }
                else{
                    return false;
                }

            case 8:
                if(y + 1 < rows && x - 1 > -1 && gridarray[x - 1,y + 1] && gridarray[x - 1,y + 1].GetComponent<Gridstat>().visited == step){
                    return true;
                }
                else{
                    return false;
                }

            default:
                return false;
                
        }
    }

    void TestEightDirections(int x, int y, int step){
        if(Testdirection(x, y, -1, 1)){
            setvisited(x, y + 1, step);
        }
        if(Testdirection(x, y, -1, 2)){
            setvisited(x + 1, y + 1, step);
        }
        if(Testdirection(x, y, -1, 3)){
            setvisited(x +1, y, step);
        }
        if(Testdirection(x, y, -1, 4)){
            setvisited(x + 1, y - 1, step);
        }
        if(Testdirection(x, y, -1, 5)){
            setvisited(x, y - 1, step);
        }
        if(Testdirection(x, y, -1, 6)){
            setvisited(x - 1, y - 1, step);
        }
        if(Testdirection(x, y, -1, 7)){
            setvisited(x - 1, y, step);
        }
        if(Testdirection(x, y, -1, 8)){
            setvisited(x - 1, y + 1, step);
        }
    }

    void setvisited (int x, int y, int step){
        if(gridarray[x,y]){
            gridarray[x,y].GetComponent<Gridstat>().visited = step;
        }
    }

    public void visualmaxdistance(int maxdistance)
    {
        maxdistance = maxdistance - 1;
        //initalsetup();
        int x = playerchar.GetComponent<player>().currentx;
        int y = playerchar.GetComponent<player>().currenty;

        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {

                int distance = Mathf.Abs(i - x) + Mathf.Abs(j - y);

                // Check if this grid square falls within the max distance
                if (distance <= maxdistance)
                {
                    if(gridarray[i,j].GetComponent<Renderer>().material.color == new Color(1f,0f,0f,0.5f)){
                        continue;
                    }
                    gridarray[i, j].GetComponent<Renderer>().material.color = new Color(0f,1f,0f,0.5f);
                }
                else{
                    gridarray[i, j].GetComponent<Renderer>().material.color = new Color(0f,0f,1f,0.5f);
                }
            }
        }
    }
}
