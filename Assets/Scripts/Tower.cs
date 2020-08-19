using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Tower : MonoBehaviour
{

    [SerializeField]
    GameObject Arrow;

    [SerializeField]
    float range = 1;

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

}
