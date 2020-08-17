using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Tower : MonoBehaviour
{
    [SerializeField]
    EnemyManager enemyManager;

    [SerializeField]
    GameObject Arrow;

    [SerializeField]
    float range = 1;

    private void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
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
        GameObject projectile = Instantiate(Arrow, transform.position, Quaternion.identity);
        projectile.GetComponent<Arrow>().targetPosition = GetClossestEnemy().transform.position;
        
    }

    GameObject GetClossestEnemy()
    {
        GameObject Closest = null;
        foreach (GameObject enemy in enemyManager.goblinList)
        {
            
            if (Vector3.Distance(transform.position, enemy.transform.position) <= range) //range check
            {
                if (Closest == null)
                {
                    Closest = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, Closest.transform.position)) //check if anyone else is closer
                    {
                        Closest = enemy;
                    }
                }
            }
        }
        return Closest;
    }

}
