using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject prefab;
    public bool Spawning;
    public int SpawnTickRate = 4;
    public int NumberToSpawn = 10;

    private float counter;
    private Pathmaker pathmaker;

    // Start is called before the first frame update
    void Start()
    {
        pathmaker = GetComponent<Pathmaker>();
        TimeTicker.Instance.AddOnTimeTickDelegate(SpawnGoblin, SpawnTickRate);
    }


    public void SpawnGoblin(int time)
    {
        var obj = Instantiate(prefab);
        obj.transform.position = transform.position;
        obj.GetComponent<Goblin>().SetPoints(pathmaker.points.ToArray());
        counter++;

        if(counter > NumberToSpawn)
            TimeTicker.Instance.RemoveOnTimeTickDelegate(SpawnGoblin, SpawnTickRate);
    }

}
