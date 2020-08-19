using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Tower : MonoBehaviour
{

    [SerializeField]
    GameObject Arrow;

    [SerializeField]
    float range = 5;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Fire();
        //}
        Fire();
    }

    public void Fire()
    {
        
        var enemy = EnemyManager.Instance.GetClosestEnemy(transform.position, range);

        if (!enemy)
            return;

         GameObject projectile = Instantiate(Arrow, transform.position, Quaternion.identity);
        var arrow = projectile.GetComponent<Arrow>();
        arrow.targetPosition = enemy.transform.position;
    }

}
