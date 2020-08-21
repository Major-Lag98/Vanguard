using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Crop : MonoBehaviour, IPlaceable, IBuyable
{
    public Grid.GridCellType PlacementType = Grid.GridCellType.Crops;
    public int CreditsAtHarvest = 10;
    public int CostToPlace = 10;

    private bool reserved;
    private Tuple<int, int> gridIndex;

    // Start is called before the first frame update
    void Start()
    {
        CropManager.Instance.AddCrop(this);
        var x = Mathf.Round(transform.position.x * 2) * 0.5f;
        var y = Mathf.Round(transform.position.y * 2) * 0.5f;

        transform.position = new Vector3(x, y, -1); // Snaps this crop to the grid since unity's grid snapping sucks

        Grid.Instance.GetXY(transform.position, out var xIndex, out var yIndex);
        Place(xIndex, yIndex);
    }

    public void SetReserved(bool value)
    {
        // If we're trying to reserve but it's already reserved
        if (value && IsReserved())
            return;

        reserved = true;
    }

    public bool IsReserved()
        => reserved;

    public bool CanBePlaced(int gridCellType)
    {
        throw new NotImplementedException();
    }

    public int GetPlacementType()
    {
        return (int)PlacementType;
    }

    public void Place(int x, int y)
    {
        // Hold our grid index for later
        gridIndex = new Tuple<int, int>(x, y);

        // Set the grid spot as taken
        Grid.Instance.SetValue(x, y, (int)Grid.GridCellType.Blocked);
    }

    public void Remove(int x, int y)
    {
        // Set the grid back to our original type.
        Grid.Instance.SetValue(x, y, GetPlacementType());
    }

    private void OnDestroy()
    {
        Remove(gridIndex.Item1, gridIndex.Item2);
    }

    public int GetCost()
    {
        return CostToPlace;
    }
}
