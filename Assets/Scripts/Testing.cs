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

    GameObject currPlacing;

    // Start is called before the first frame update
    void Start()
    {
        //x cell count, y cell count, cell size, origin position
        //grid = new Grid(29, 22, 1, new Vector3(-24, -11));
    }


    /*
     * GridValues:
     * == 0, nothing there and can place
     * == 1, somthings already there
     * == 3, blocked cannot place
     */

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && IsPlacing()) //left click to place
        {
            Vector3 mousePos = UtilsClass.GetMouseWorldPosition();
            int thisGridValue = grid.GetValue(mousePos);

            if (thisGridValue == 3 || thisGridValue == 1) //blocked or somthing is already there
            {
                return;
            }
            if (thisGridValue == 0)
            {
                grid.SetValue(mousePos, 1);
                SpawnTower(currPlacing);
            }
            
        }
        else if (Input.GetMouseButtonDown(1))
        {
            SetPlacing(null);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            SetPlacing(tower);
        }
        else if (Input.GetKeyDown(KeyCode.P)) // press P to get value DEBUG
        {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }

    }

    public void SetPlacing(GameObject obj)
    {
        currPlacing = obj;
    }

    public bool IsPlacing() => currPlacing != null;


    private void SpawnTower(GameObject obj)
    {
        Vector3 spawnPosition = UtilsClass.GetMouseWorldPosition();
        spawnPosition = ValidateWorldGridPosition(spawnPosition);
        spawnPosition += new Vector3(1, 1, 0) * grid.GetCellSize() * .5f;

        Instantiate(obj, spawnPosition, Quaternion.identity);
    }

    private Vector3 ValidateWorldGridPosition(Vector3 position)
    {
        grid.GetXY(position, out int x, out int y);
        return grid.GetWorldPosition(x, y);
    }
}
