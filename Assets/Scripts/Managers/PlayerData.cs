using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public TextMeshProUGUI CreditText;
    public float TweenTime = 0.5f;

    public static PlayerData Instance;

    [SerializeField]
    private int credits = 0;

    public int Credits { get => credits; private set => credits = value; }

    private void Start()
    {
        if (Instance == null) //THERE I MADE A SINGLETON HAPPY?
        {
            Instance = this;
        }

        CreditText.text = credits.ToString();
    }


    public bool CanAfford(int amount)
    {
        if ((Credits - amount) < 0)
        {
            return false;
        }
        else return true;
    }

    public void AddCredits(int amount)
    {
        Credits += amount;
        DOTween.To(() => Convert.ToInt32(CreditText.text), (x) => CreditText.text = x.ToString(), Credits, 0.5f);
    }

    public void Spend(int amount)
    {
        Credits -= amount;
        DOTween.To(() => Convert.ToInt32(CreditText.text), (x) => CreditText.text = x.ToString(), Credits, 0.5f);
    }

}
