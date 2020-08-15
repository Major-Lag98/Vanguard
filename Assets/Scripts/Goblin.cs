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
        StartCoroutine("Move");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Move()
    {
        while (currIter < points.Length)
        {
            var target = points[currIter];
            transform.DOMove(target, 0.5f).SetEase(Ease.OutExpo);
            currIter++;

            yield return new WaitForSeconds(TimePerMove);
        }


        yield break;
    }

    public void SetPoints(Vector3[] points)
    {
        this.points = points;
    }
}
