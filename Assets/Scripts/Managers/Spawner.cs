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

    private string[] levels; // Holds the seprate levels
    private string[] waves; // The string array to hold seprate waves (set for each new level)
    private char[] waveSpawnOrder; // The order of spawns within a wave

    private int spawnCounter; // How many enemies we've spawned so far
    private int waveIndex = -1; // The index to access the waves
    private int levelIndex = -1; // The index for the level

    private Pathmaker pathmaker;

    public static Spawner Instance;

    // Start is called before the first frame update
    void Start()
    {
        pathmaker = GetComponent<Pathmaker>(); // Get our pathmaker
        TimeTicker.Instance.AddOnTimeTickDelegate(SpawnGoblin, SpawnTickRate); // Plug us into the time ticker event

        levels = SpawnFile.text.Split(new string[] { "\r\n\r\n" },
                               StringSplitOptions.RemoveEmptyEntries);

        if(Instance == null)
            Instance = this;
    }

    public void SpawnGoblin(int time)
    {
        if (!Spawning || spawnCounter >= waveSpawnOrder.Length)
            return;

        if(!int.TryParse(waveSpawnOrder[spawnCounter].ToString(), out int spawnType)){
            Spawning = false;
            return;
        }

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
        waveIndex++;

        if(waveIndex < waves.Length)
        {
            // Trim the wave of newlines and turn into a char array. This will be our IDs for which enemy to spawn
            var str = waves[waveIndex].Replace(Environment.NewLine, String.Empty);
            waveSpawnOrder = str.ToCharArray();
            NumberToSpawn = waveSpawnOrder.Length; // This is how many to spawn
            spawnCounter = 0; // Reset our spawn counter
            return true;
        }

        return false;
    }

    /// <summary>
    /// Loads the next level of waves. If it was loaded successfully, returns true. Otherwise false
    /// </summary>
    /// <returns>True if loaded. False otherwise</returns>
    public bool LoadNextLevel()
    {
        levelIndex++;

        if (levelIndex < levels.Length)
        {
            // Get the current level text and split it by newline. That will be our wave array to access when we start spawning
            waves = levels[levelIndex].Split(Environment.NewLine.ToCharArray());
            waves = waves.Where(s => s.Length > 0).ToArray();
            waveIndex = -1;
            return true; // Return that we are good
        }

        return false;
    }

    /// <summary>
    /// </summary>
    /// <returns>True if there is another level past the current. False otherwise</returns>
    public bool HasNextLevel()
     => levelIndex + 1 < levels.Length;

    /// <summary>
    /// </summary>
    /// <returns>True if there is another wave past the current, false otherwise.</returns>
    public bool HasNextWave()
     => waveIndex < waves.Length;
}
