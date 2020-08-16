using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    public float TimePerMove = 1f;

    private Vector3[] points;
    private int currIter;

    enum StateEnum {Moving, Attacking, TakingPlant, Leaving}

    private StateEnum currState = StateEnum.Moving;

    // Start is called before the first frame update
    void Start()
    {
        TimeTicker.Instance.AddOnTimeTickDelegate(MoveAlongPath, 2);
    }

    void MoveAlongPath(int time)
    {
        if (currIter < points.Length)
        {
            var target = points[currIter];
            transform.DOMove(target, 0.5f).SetEase(Ease.OutExpo);
            currIter++;
        }
        else
        {
            TimeTicker.Instance.RemoveOnTimeTickDelegate(MoveAlongPath, 2);
            TimeTicker.Instance.AddOnTimeTickDelegate(MoveToPlant, 2);
            currState = StateEnum.TakingPlant; // When we get to the end, take a plant
        }
    }

    void MoveToPlant(int time)
    {
        if (currIter < points.Length)
        {
            var target = points[currIter];
            transform.DOMove(target, 0.5f).SetEase(Ease.OutExpo);
            currIter++;
        }
        else
        {
            TimeTicker.Instance.RemoveOnTimeTickDelegate(MoveAlongPath, 2);
            TimeTicker.Instance.AddOnTimeTickDelegate(MoveToPlant, 2);
            currState = StateEnum.TakingPlant; // When we get to the end, take a plant
        }
    }

    void MoveOffScreen(int time)
    {

    }

    public void SetPoints(Vector3[] points)
    {
        this.points = points;
    }

    private void OnDestroy()
    {
        TimeTicker.Instance.RemoveOnTimeTickDelegate(MoveAlongPath, 2);
    }
}
