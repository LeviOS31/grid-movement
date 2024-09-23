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
    public int endx = 5;
    public int endy = 5;
    public List<GameObject> path = new List<GameObject>();
    // Start is called before the first frame update
    private void Awake() {

        gridarray = new GameObject[columns, rows];

        if(gridPrefab){
            GenerateGrid();
        }   
        else{
            print("ow ow this is wrong");
        }
    }
    
    void Start()
    {
        initalsetup();    
    }
    // Update is called once per frame
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

    void setDistance(){
        initalsetup();
        int x = startx;
        int y = starty;

        int[] testarray =  new int[rows*columns];
        for(int step = 1; step < rows*columns; step++){
            foreach(GameObject obj in gridarray){
                if(gridarray[obj.GetComponent<Gridstat>().x, obj.GetComponent<Gridstat>().y] == gridarray[endx, endy])
                if(obj.GetComponent<Gridstat>().visited == step --){
                    TestEightDirections(obj.GetComponent<Gridstat>().x,obj.GetComponent<Gridstat>().y,step);
                }
            }
        }
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
            print("this is very very bad");
            return;
        }

        for(int i = step; step > -1; step--)
        {
            if(Testdirection(x,y,step, 1)){
                templist.Add(gridarray[x,y+1]);
                break;
            }
            if(Testdirection(x,y,step, 2)){
                templist.Add(gridarray[x+1,y+1]);
                break;
            }
            if(Testdirection(x,y,step, 3)){
                templist.Add(gridarray[x+1,y]);
                break;
            }
            if(Testdirection(x,y,step, 4)){
                templist.Add(gridarray[x+1,y-1]);
                break;
            }
            if(Testdirection(x,y,step, 5)){
                templist.Add(gridarray[x,y-1]);
                break;
            }
            if(Testdirection(x,y,step, 6)){
                templist.Add(gridarray[x-1,y-1]);
                break;
            }
            if(Testdirection(x,y,step, 7)){
                templist.Add(gridarray[x-1,y]);
                break;
            }
            if(Testdirection(x,y,step, 8)){
                templist.Add(gridarray[x-1,y+1]);
                break;
            }
        }
    }

    void initalsetup(){
        foreach(GameObject obj in gridarray)
        {
            obj.GetComponent<Gridstat>().visited = -1;
        }

        gridarray[startx, starty].GetComponent<Gridstat>().visited = 0;
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
}
