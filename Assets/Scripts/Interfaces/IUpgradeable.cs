using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradeable
{
    int GetUpgradeCost();
    bool CanUpgrade();
    bool Upgrade();
}
