using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{

    Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        //x cell count, y cell count, cell size, origin position
        grid = new Grid(29, 22, 1, new Vector3(-24, -11));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
           grid.SetValue(UtilsClass.GetMouseWorldPosition(), 10);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }

    }
}
