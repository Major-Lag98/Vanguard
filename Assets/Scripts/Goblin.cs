using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    public float TimePerMove = 1f;

    private Vector3[] points;
    private int currIter;

    // Start is called before the first frame update
    void Start()
    {
        TimeTicker.Instance.AddOnTimeTickDelegate(Move, 2);
    }

    void Move(int time)
    {
        var target = points[currIter];
        transform.DOMove(target, 0.5f).SetEase(Ease.OutExpo);
        currIter++;

    }

    public void SetPoints(Vector3[] points)
    {
        this.points = points;
    }

    private void OnDestroy()
    {
        TimeTicker.Instance.RemoveOnTimeTickDelegate(Move, 2);
    }
}
