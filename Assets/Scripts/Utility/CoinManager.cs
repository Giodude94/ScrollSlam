using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    private int currentRunCoins = 0;
    private int totalCoins;

    private void Start()
    {
        //PlayerPrefs.DeleteKey("TotalCoins");
        Instance = this;
        currentRunCoins = 0;
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);

    }
    public int GetCurrentCoins() { return currentRunCoins; }
    public int GetTotalCoins() { return totalCoins; }
    public void SetCurrentCoins(int enemyCoinValue) { currentRunCoins += enemyCoinValue; }
    public void SetCurrentCoinsToStash()
    {
        PlayerPrefs.SetInt("TotalCoins", currentRunCoins + totalCoins);
    }
    public bool TrySpendCoins(int costOfItem)
    {
        if (costOfItem <= totalCoins)
        {
            //Performing arithmetic for purchasing item.
            totalCoins = totalCoins - costOfItem;
            return true;
        }
        return false;
    }
}
