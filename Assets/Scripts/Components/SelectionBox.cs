using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{

    public float EndScale = 1.5f;
    public float TimeToScale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            //child.DOScaleX(EndScale, 1f)
            Sequence seq = DOTween.Sequence();
            seq.Append(child.DOScaleX(EndScale, TimeToScale).SetEase(Ease.InOutSine));
            seq.Append(child.DOScaleX(1f, TimeToScale).SetEase(Ease.InOutSine));
            seq.SetAutoKill(false);
            seq.SetLoops(-1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
