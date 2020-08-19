using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndShowUIElement : MonoBehaviour
{
    public bool ShowDuringDay;
    public bool ShowDuringNight;
    public bool ShowDuringAttacking;

    [Tooltip("If true, the object is enabled on show. If false, a transition is used to move the object to PositionToShowAt")]
    public bool InstantShow = true;
    [Tooltip("If true, the object is disabled on hide. If false, a transition is used to move the object to PositionToHideAt")]
    public bool InstantHide = true;

    [Tooltip("The position to transition to if InstantShow is false")]
    public Vector3 PositionToShowAt;
    [Tooltip("The position to trainstion to if InstantHide is false")]
    public Vector3 PostionToHideAt;

    [Tooltip("The time the transition takes (if applicable)")]
    public float TimeToTransitionIfNotInstance = 0.4f;

    bool hiding;

    private void Start()
    {
        GameStateManager.Instance.AddGameStateChangedDelegate(GameStateChanged);
    }

    void GameStateChanged(GameStateManager.State state)
    {
        // Switch on the state that has changed
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
        // If instant, just set active
        if (InstantShow)
            gameObject.SetActive(true);
        else // Otherwise we transition
        {
            var t = ((RectTransform)transform);
            DOTween.To(() => t.anchoredPosition.y, (y) => t.anchoredPosition = new Vector2(t.anchoredPosition.x, y), PositionToShowAt.y, TimeToTransitionIfNotInstance).SetEase(Ease.OutSine);
        }

        hiding = false;
    }

    void Hide()
    {
        // If instance, just disable
        if (InstantHide)
            gameObject.SetActive(false);
        else // Otherwise transition
        {
            var t = ((RectTransform)transform);
            DOTween.To(() => t.anchoredPosition.y, (y) => t.anchoredPosition = new Vector2(t.anchoredPosition.x, y), PostionToHideAt.y, TimeToTransitionIfNotInstance).SetEase(Ease.InSine);
        }

        hiding = true;
    }
}
