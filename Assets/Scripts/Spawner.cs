using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    EnemyManager enemyManager;

    public GameObject prefab;
    public bool Spawning;
    public int SpawnTickRate = 4;
    public int NumberToSpawn = 10;

    private int counter;
    private Pathmaker pathmaker;

    // Start is called before the first frame update
    void Start()
    {
        pathmaker = GetComponent<Pathmaker>();
        //enemyManager = GetComponent<EnemyManager>();
        TimeTicker.Instance.AddOnTimeTickDelegate(SpawnGoblin, SpawnTickRate);
    }

    public void SpawnGoblin(int time)
    {
        var obj = Instantiate(prefab);
        obj.transform.position = pathmaker.points[0];
        obj.GetComponent<Goblin>().SetPoints(pathmaker.points.GetRange(1, pathmaker.points.Count-1).ToArray());
        counter++;

        
        enemyManager.goblinList.Add(obj);

        if(counter > NumberToSpawn)
            TimeTicker.Instance.RemoveOnTimeTickDelegate(SpawnGoblin, SpawnTickRate);
    }

}
