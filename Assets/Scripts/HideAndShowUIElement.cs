using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndShowUIElement : MonoBehaviour
{
    public bool ShowDuringDay;
    public bool ShowDuringNight;
    public bool ShowDuringAttacking;

    public bool InstantHide = true;
    public bool InstantShow = true;

    public Vector3 PositionToShowAt;
    public Vector3 PostionToHideAt;

    public float TimeToTransitionIfNotInstance = 0.4f;

    bool hiding;

    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        GameStateManager.Instance.AddGameStateChangedDelegate(GameStateChanged);
    }

    void GameStateChanged(GameStateManager.State state)
    {
        switch (state)
        {
            case GameStateManager.State.Day:
                if (ShowDuringDay && hiding)
                    Show();
                else if (!ShowDuringDay && !hiding)
                    Hide();
                break;
            case GameStateManager.State.Night:
                if (ShowDuringNight && hiding)
                    Show();
                else if (!ShowDuringNight && !hiding)
                    Hide();
                break;
            case GameStateManager.State.Attacking:
                if (ShowDuringAttacking && hiding)
                    Show();
                else if (!ShowDuringAttacking && !hiding)
                    Hide();
                break;
        }

    }

    void Show()
    {
        if (InstantShow)
            gameObject.SetActive(true);
        else
        {
            var t = ((RectTransform)transform);
            DOTween.To(() => t.anchoredPosition.y, (y) => t.anchoredPosition = new Vector2(t.anchoredPosition.x, y), PositionToShowAt.y, TimeToTransitionIfNotInstance).SetEase(Ease.OutSine);
        }

        hiding = false;
    }

    void Hide()
    {
        if (InstantHide)
            gameObject.SetActive(false);
        else
        {
            var t = ((RectTransform)transform);
            DOTween.To(() => t.anchoredPosition.y, (y) => t.anchoredPosition = new Vector2(t.anchoredPosition.x, y), PostionToHideAt.y, TimeToTransitionIfNotInstance).SetEase(Ease.InSine);
        }

        hiding = true;
    }
}
