using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleUIHide : MonoBehaviour, IHideableUI
{

    public bool StartHidden;

    private void Start()
    {
        if (StartHidden)
            Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Toggle()
    {
        throw new System.NotImplementedException();
    }
}
