using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxEntrance : MonoBehaviour
{
    public Vector3 TargetPosition;
    public TextWriter WriterToStart;
    public float TransitionTime = 1f;
    public float DelayToStartAfterTransition = 1f;

    // Start is called before the first frame update
    void Start()
    {
        var seq = DOTween.Sequence();
        var rect = (RectTransform)transform;
        seq.AppendInterval(1f);
        seq.Append(DOTween.To(() => rect.anchoredPosition.y,
            (y) => rect.anchoredPosition = new Vector2(0, y), TargetPosition.y, TransitionTime).SetEase(Ease.OutBack))
            .AppendInterval(DelayToStartAfterTransition)
            .OnComplete(() => WriterToStart.StartWriting());

        seq.SetUpdate(true);
        seq.timeScale = 1;
    }

}
