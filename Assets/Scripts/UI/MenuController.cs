using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public enum PreviousScreen
    {
        MainMenu,
        GameOver
    }

    [Header("Panels")]
    //[SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject upgradeShopPanel;
    [SerializeField] private GameObject hudPanel;

    private PreviousScreen previousScreen;


    public void Start()
    {
        //Calls to be made at the start of the game.
        gameOverPanel.SetActive(false);
        upgradeShopPanel.SetActive(false);
        hudPanel.SetActive(true);
    }

    public void OpenShopFromMainMenu()
    {
        previousScreen = PreviousScreen.MainMenu;
        ShowShop();
    }

    public void OpenShopFromGameOver()
    {
        previousScreen = PreviousScreen.GameOver;
        ShowShop();
    }

    private void ShowShop()
    {
        //mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        hudPanel.SetActive(false);
        upgradeShopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        upgradeShopPanel.SetActive(false);

        if (previousScreen == PreviousScreen.MainMenu)
        {
            //mainMenuPanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(true);
        }
    }
}
