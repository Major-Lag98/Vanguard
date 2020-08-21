using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTicker : MonoBehaviour
{

    public delegate void OnTimeTickDelegate(int currTime);

    private Dictionary<int, OnTimeTickDelegate> delegateMap = new Dictionary<int, OnTimeTickDelegate>();

    private float lastTime;

    public static TimeTicker Instance;

    private float counter;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if(counter >= 1) // Every second that passes
        {
            lastTime = Time.time; // Set last time to the current time
            CallDelegates(Time.time, delegateMap); // Call our delegates
            counter -= 1;
        }
    }

    void CallDelegates(float time, Dictionary<int, OnTimeTickDelegate> map)
    {
        List<int> keyList = new List<int>(map.Keys);

        foreach (int tick in keyList) // For each 'tick' in the map
        {
            var t = (int)time;
            if (t % tick == 0) // If it's time to call
                map[tick]?.Invoke((int)time); // call
        }
    }

    public void AddOnTimeTickDelegate(OnTimeTickDelegate del, int tick)
    {
        if (delegateMap.ContainsKey(tick))
            delegateMap[tick] += del;
        else
        {
            OnTimeTickDelegate d = delegate (int time) { }; // Create the new delegate
            d += del; // Add the delegate
            delegateMap[tick] = d; // Add it to the map
        }
    }

    public void RemoveOnTimeTickDelegate(OnTimeTickDelegate del, int tick)
    {
        if (delegateMap.ContainsKey(tick))
            delegateMap[tick] -= del;
    }

    public void SetTimeScale(int scale)
        => Time.timeScale = scale;
}
