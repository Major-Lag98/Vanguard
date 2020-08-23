using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class Tower : MonoBehaviour, IPlaceable, ISelectable, IBuyable, IUpgradeable
{

    [SerializeField]
    List<Upgrade> upgradeList;

    [SerializeField]
    GameObject Arrow;

    [SerializeField]
    float range = 5;

    [SerializeField]
    int placementType = 1;

    public int CostToPlace = 10;

    bool fired;

    Tuple<int, int> gridIndex;

    private Upgrade stats;
    private int currUpgradeLevel;

    private GameObject rank1, rank2, rank3;

    private void Start()
    {
        TimeTicker.Instance.AddOnTimeTickDelegate(Fire, 1);

        Grid.Instance.GetXY(transform.position, out var xIndex, out var yIndex);
        Place(xIndex, yIndex);

        stats = upgradeList[0];

        rank1 = transform.Find("Rank1").gameObject;
        rank2 = transform.Find("Rank2").gameObject;
        rank3 = transform.Find("Rank3").gameObject;
    }

    public void Fire(int time)
    {
        if (time % 2 == 0 || fired) // We don't want even ticks
        {
            fired = false;
            return;
        }
        
        var enemy = EnemyManager.Instance.GetClosestEnemy(transform.position, range);
        

        if (!enemy)
            return;

        GameObject projectile = Instantiate(Arrow, transform.position, Quaternion.identity);
        var arrow = projectile.GetComponent<Arrow>();
        arrow.Damage = stats.AttackDamage;
        arrow.targetPosition = enemy.transform.position;
        arrow.enemy = enemy.GetComponent<Goblin>();

        fired = true;
    }

    public bool CanBePlaced(int gridCellType)
    {
        throw new System.NotImplementedException();
    }

    public int GetPlacementType()
    {
        return placementType;
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

    public void Select()
    {
        SelectionController.Instance.Select(gameObject);
    }

    public void Deselect()
    {

    }

    private void OnMouseUp()
    {
        Select();
    }

    public int GetCost()
    {
        return CostToPlace;
    }

    public int GetUpgradeCost()
    {
        // If we can upgrade, then the cost is the next level. Otherwise return -1 to signify invalid
        var cost = CanUpgrade() ? upgradeList[currUpgradeLevel + 1].Cost : -1;
        return cost;
    }

    public bool CanUpgrade()
    {
        // True if we aren't max upgrades yet
        return currUpgradeLevel < upgradeList.Count - 1;
    }

    public bool Upgrade()
    {
        // Assign us the next upgrade
        stats = upgradeList[++currUpgradeLevel];

        switch (currUpgradeLevel)
        {
            case 1:
                rank1.SetActive(true);
                rank2.SetActive(false);
                rank3.SetActive(false);
                break;
            case 2:
                rank1.SetActive(false);
                rank2.SetActive(true);
                rank3.SetActive(false);
                break;
            case 3:
                rank1.SetActive(false);
                rank2.SetActive(false);
                rank3.SetActive(true);
                break;
        }


        return true;
    }
}
