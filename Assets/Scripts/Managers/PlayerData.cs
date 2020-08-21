using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    private void Start()
    {
        if (Instance == null) //THERE I MADE A SINGLETON HAPPY?
        {
            Instance = this;
        }
    }

    public int Credits = 0;


    public bool CanAfford(int amount)
    {
        if ((Credits - amount) < 0)
        {
            return false;
        }
        else return true;

        
    }

    public void Spend(int amount)
    {
        Credits -= amount;
    }

}
