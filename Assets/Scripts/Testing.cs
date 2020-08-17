using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

//[System.Serializable]
[ExecuteInEditMode]
[System.Serializable]
public class Testing : MonoBehaviour
{
    [SerializeField]
    Grid grid;
    
    [SerializeField]
    GameObject tower;

    // Start is called before the first frame update
    void Start()
    {
        //x cell count, y cell count, cell size, origin position
        //grid = new Grid(29, 22, 1, new Vector3(-24, -11));
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

    private void SpawnTower()
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
