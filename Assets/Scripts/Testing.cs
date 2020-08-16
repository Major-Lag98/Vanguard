using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing : MonoBehaviour
{

    Grid grid;

    [SerializeField]
    GameObject tower;

    [SerializeField]
    Vector2 WH = new Vector2(2,2);


    [SerializeField]
    Vector2 Origin = Vector2.zero;

    

    // Start is called before the first frame update
    void Start()
    {
        //x cell count, y cell count, cell size, origin position
        grid = new Grid((int)WH.x, (int)WH.y, 1, Origin);
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

        if (Input.GetMouseButtonDown(0))
        {
            SpawnTower();
        }

    }

    void SpawnTower()
    {
        Vector3 spawnPosition = UtilsClass.GetMouseWorldPosition();
        spawnPosition = ValidateWorldGridPosition(spawnPosition);
        spawnPosition += new Vector3(1, 1, 0) * grid.GetCellSize() * .5f;
        Instantiate(tower, spawnPosition, Quaternion.identity);
    }

    private Vector3 ValidateWorldGridPosition(Vector3 position)
    {
        grid.GetXY(position, out int x, out int y);
        return grid.GetWorldPosition(x, y);
    }
}
