using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceable
{
    bool CanBePlaced(int gridCellType);

    int PlacementType();


}
