using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class Tower : MonoBehaviour, IPlaceable
{

    [SerializeField]
    GameObject Arrow;

    [SerializeField]
    float range = 5;

    [SerializeField]
    int placementType = 1;

    bool fired;

    Tuple<int, int> gridIndex;

    private void Start()
    {
        TimeTicker.Instance.AddOnTimeTickDelegate(Fire, 1);

        Grid.Instance.GetXY(transform.position, out var xIndex, out var yIndex);
        Place(xIndex, yIndex);
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
        arrow.targetPosition = enemy.transform.position;
        arrow.enemy = enemy.GetComponent<IDamageable>();

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
}
