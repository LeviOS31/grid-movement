using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridstat : MonoBehaviour
{
    public int visited = -1;
    public int x = 0;
    public int y = 0;
    public bool traversable = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.name);
        traversable = false;
    }
}
