using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

//[System.Serializable]
[ExecuteInEditMode]
[System.Serializable]
public class PlacingController : MonoBehaviour
{
    [SerializeField]
    Grid grid;
    
    [SerializeField]
    GameObject tower;

    GameObject currPlacing;

    // Start is called before the first frame update
    void Start()
    {
        
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

            if (thisGridValue == (int)Grid.GridCellType.Blocked) //blocked or somthing is already there
            {
                return;
            }

            var placingType = currPlacing.GetComponent<IPlaceable>().PlacementType();
            if (Enum.IsDefined(typeof(Grid.GridCellType), placingType) && thisGridValue == placingType)
            {
                SpawnPrefab(currPlacing);
                Grid.Instance.SetValue(mousePos, (int)Grid.GridCellType.Blocked); // Mark the grid cell as blocked
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


    private void SpawnPrefab(GameObject obj)
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
