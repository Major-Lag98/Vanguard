using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTicker : MonoBehaviour
{

    public delegate void OnTimeTickDelegate(int currTime);

    private Dictionary<int, OnTimeTickDelegate> delegateMap = new Dictionary<int, OnTimeTickDelegate>();

    public static TimeTicker Instance;

    private float counter;
    private Observable<int> tick = new Observable<int>();

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        tick.OnChanged += (obj, oldTick, newTick) => CallDelegates(newTick, delegateMap);
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime*2;
        tick.Value = (int)counter;
    }

    void CallDelegates(int currGameTick, Dictionary<int, OnTimeTickDelegate> map)
    {
        List<int> keyList = new List<int>(map.Keys);

        foreach (int tick in keyList) // For each 'tick' in the map
        {
            if (currGameTick % tick == 0) // If it's time to call
                map[tick]?.Invoke(currGameTick); // call
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
