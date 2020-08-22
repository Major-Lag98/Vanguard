using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<PrefabGroup> prefabList;
    public bool Spawning;
    public int SpawnTickRate = 4;
    public int NumberToSpawn = 10;
    public TextAsset SpawnFile;

    private int spawnCounter;
    private string[] waveSpawnOrder; // The string array to hold waves
    private int spawnWaveIndex = -1; // The index to access the waves
    private char[] spawnOrder; // The order of spawns within a wave

    private Pathmaker pathmaker;

    // Start is called before the first frame update
    void Start()
    {
        pathmaker = GetComponent<Pathmaker>(); // Get our pathmaker
        TimeTicker.Instance.AddOnTimeTickDelegate(SpawnGoblin, SpawnTickRate); // Plug us into the time ticker event

        waveSpawnOrder = SpawnFile.text.Split('\n');
    }

    public void SpawnGoblin(int time)
    {
        if (!Spawning)
            return;

        var spawnType = Convert.ToInt32(spawnOrder[spawnCounter].ToString());
        var group = prefabList.FirstOrDefault(p => p.id == spawnType);

        if (group.Equals(default(PrefabGroup)))
            return;

        if (group.prefab == null)
            Debug.LogWarning("Prefab is null for the Spawner group. This shouldn't happen");

        var obj = Instantiate(group.prefab, pathmaker.points[0], Quaternion.identity);
        obj.GetComponent<Goblin>().SetPoints(pathmaker.points.GetRange(1, pathmaker.points.Count-1).ToArray());
        spawnCounter++;

        EnemyManager.Instance.goblinList.Add(obj);

        if (spawnCounter >= NumberToSpawn)
            Spawning = false;
    }

    /// <summary>
    /// Prepares the next wave for spawning
    /// </summary>
    /// <returns>True if the wave was prepared, false if there are no more waves available</returns>
    public bool LoadNextWave()
    {
        spawnWaveIndex++;

        if(spawnWaveIndex < waveSpawnOrder.Length)
        {
            // The spawn order is the characters from the text line. We make sure to remove any trailing new line characters; \n \r
            spawnOrder = waveSpawnOrder[spawnWaveIndex].TrimEnd(Environment.NewLine.ToCharArray()).ToCharArray();
            //TODO Fully implement this
            NumberToSpawn = spawnOrder.Length;
            spawnCounter = 0;
            return true;
        }

        return false;
    }

}
