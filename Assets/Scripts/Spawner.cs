using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject prefab;
    public bool Spawning;
    public float DelayPerSpawn = 1f;
    public int NumberToSpawn = 10;

    private float counter;
    private Pathmaker pathmaker;

    // Start is called before the first frame update
    void Start()
    {
        pathmaker = GetComponent<Pathmaker>();
        TimeTicker.Instance.AddOnTimeTickDelegate(SpawnGoblin, 6);
    }


    public void SpawnGoblin(int time)
    {
        var obj = Instantiate(prefab);
        obj.transform.position = transform.position;
        obj.GetComponent<Goblin>().SetPoints(pathmaker.points.ToArray());
        counter -= DelayPerSpawn;
    }

}
