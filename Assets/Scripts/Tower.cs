using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Tower : MonoBehaviour, IPlaceable
{

    [SerializeField]
    GameObject Arrow;

    [SerializeField]
    float range = 5;

    [SerializeField]
    int placementType = 1;

    bool fired;

    private void Start()
    {
        TimeTicker.Instance.AddOnTimeTickDelegate(Fire, 1);
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

        fired = true;
    }

    public bool CanBePlaced(int gridCellType)
    {
        throw new System.NotImplementedException();
    }

    public int PlacementType()
    {
        return placementType;
    }

    public void Place(int x, int y)
    {
        
    }

    public void Remove(int x, int y)
    {
        
    }
}
