using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{

    public float EndScale = 1.5f;
    public float TimeToScale = 1f;
    public bool DoX = true;
    public bool DoY = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            if(DoX && DoY)
            {
                Sequence seq = DOTween.Sequence();
                seq.Append(child.DOScale(EndScale, TimeToScale).SetEase(Ease.InOutSine));
                seq.Append(child.DOScale(1f, TimeToScale).SetEase(Ease.InOutSine));
                seq.SetAutoKill(false);
                seq.SetLoops(-1);
            }
            //child.DOScaleX(EndScale, 1f)
            else if (DoX)
            {
                Sequence seq = DOTween.Sequence();
                seq.Append(child.DOScaleX(EndScale, TimeToScale).SetEase(Ease.InOutSine));
                seq.Append(child.DOScaleX(1f, TimeToScale).SetEase(Ease.InOutSine));
                seq.SetAutoKill(false);
                seq.SetLoops(-1);
            }else if (DoY)
            {

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
