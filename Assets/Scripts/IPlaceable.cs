using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceable
{

    bool CanBePlaced(int gridCellType);

    /// <summary>
    /// Gets the placement type of the IPlaceable. This should sync up to the Grid.GridCellType for placing
    /// stuff on the Grid.
    /// </summary>
    /// <returns>An integer representing its type on the Grid</returns>
    int PlacementType();

    /// <summary>
    /// Called when the object is placed on the Grid. Use the X and Y corrdinates to manipulate
    /// the Grid as needed
    /// </summary>
    /// <param name="x">The X index of the Grid where placed</param>
    /// <param name="y">The Y index of the grid where placed</param>
    void Place(int x, int y);

    /// <summary>
    /// Called when the object is removed from the Grid at an X/Y location. Use the X/Y to manipulate the Grid
    /// as needed
    /// </summary>
    /// <param name="x">The X index of the Grid where removed</param>
    /// <param name="y">The Y index of the Grid where removed</param>
    void Remove(int x, int y);
}
