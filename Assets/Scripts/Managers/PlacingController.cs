using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;
using System.Linq;

//[System.Serializable]
[ExecuteInEditMode]
[System.Serializable]
public partial class PlacingController : MonoBehaviour
{
    public PrefabGroup[] Prefabs;

    [SerializeField]
    TextAsset PrefabFile;

    [SerializeField]
    Grid grid;
    
    [SerializeField]
    GameObject tower;

    GameObject currPlacing;

    [SerializeField]
    GameObject ghostTower;

    GameObject ghost;

    bool ghostInstantiated = false;

    public static PlacingController Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
            Instance = this;

        // Any time the game state changes, clear our placing so we can't do things like place crops at night
        GameStateManager.Instance.AddGameStateChangedDelegate((_) => SetPlacing(""));
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
            Vector3 mousePos = UtilsClass.GetMouseWorldPosition(); // Gets the world position where we clicked
            int thisGridValue = grid.GetValue(mousePos);

            if (thisGridValue == (int)Grid.GridCellType.Blocked) //blocked or somthing is already there
            {
                return;
            }

            var placingType = currPlacing.GetComponent<IPlaceable>().GetPlacementType(); // Get the grid type of the thing we're placing

            // Checks if the placingType is valid for Grid.GridCellType and that the grid allowed type equals our thing's type
            if (Enum.IsDefined(typeof(Grid.GridCellType), placingType) && thisGridValue == placingType)
            {
                var cost = currPlacing.GetComponent<IBuyable>().GetCost(); // Get the cost from the thing
                if (PlayerData.Instance.CanAfford(cost)) // Check if we can afford
                {
                    SpawnPrefab(currPlacing); //Spawn the prefab
                    PlayerData.Instance.Spend(cost); // Subtract the cost
                }
                
            }
            
        }
        else if (Input.GetMouseButtonDown(1)) // CLear our placing on right click
        {
            SetPlacing("");
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            SetPlacing("tower");
        }
        else if (Input.GetKeyDown(KeyCode.P)) // press P to get value DEBUG
        {
            Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        }

        if (currPlacing == Prefabs.FirstOrDefault(p => p.name == "tower").prefab && IsPlacing())//check if were placing a tower so we can have a ghost to show when placing
        {
            //Debug.Log("Placing Tower");
            Vector3 spawnPosition = UtilsClass.GetMouseWorldPosition();
            spawnPosition = ValidateWorldGridPosition(spawnPosition);
            spawnPosition += new Vector3(1, 1, 0) * grid.GetCellSize() * .5f;

            if (ghostInstantiated == false)
            {
                ghost = Instantiate(ghostTower);
                ghostInstantiated = true;
            }
            // ghost.SetActive(true);
            ghost.transform.position = spawnPosition;

        }
        else if (ghost)
        {
            Destroy(ghost);
            ghostInstantiated = false;
        }
    }

    public void SetPlacing(string placingName)
    {

        // If the incoming name is empty or null, set our currPlacing to null
        if (placingName.Length == 0 || placingName == null)
            currPlacing = null;
        // Otherwise try to load the group
        else
        {
            var group = Prefabs.FirstOrDefault(p => p.name == placingName);
            if (group.Equals(default(PrefabGroup))) // If the default was returned it means we didn't find a match. Set to null then
                currPlacing = null;
            else // Otherwise we try to load the prefab
            {
                currPlacing = group.prefab;
            }
        }
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

    [Serializable]
    public struct PrefabGroupHolder
    {
        public List<PrefabGroup> list;

        public PrefabGroupHolder(List<PrefabGroup> list)
        {
            this.list = list;
        }
    }

}
