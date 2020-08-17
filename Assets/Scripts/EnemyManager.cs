using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;
    private void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    public List<GameObject> goblinList = new List<GameObject>();

    public GameObject GetClosestEnemy(Vector3 fromPosition, float range)
    {
        GameObject closest = null;
        float lastDist = float.MaxValue; // Our cached distance for the last closest enemy. start at max value 

        foreach (GameObject enemy in goblinList)
        {
            // Check with Vector2 because the Z values throw off the distance calculation.
            // Could also check with Vector3 if the Z is equalized for both.
            var dist = Vector2.Distance(new Vector2(fromPosition.x, fromPosition.y), new Vector2(enemy.transform.position.x, enemy.transform.position.y));
            if (dist <= range) //range check
            {
                if(dist < lastDist) // If the dist to the enemy is closer than the last distance
                {
                    closest = enemy; // assign our new closest enemy
                    lastDist = dist; // cache our last distance to the current
                }
            }
        }
        return closest;
    }
}
