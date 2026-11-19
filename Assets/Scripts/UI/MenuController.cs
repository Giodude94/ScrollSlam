using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    [Header("Panels")]
    //[SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject upgradeShopPanel;
    [SerializeField] private GameObject hudPanel;


    public void Start()
    {
        //Calls to be made at the start of the game.
        gameOverPanel.SetActive(false);
        upgradeShopPanel.SetActive(false);
        hudPanel.SetActive(true);
    }

    public void ShowShop()
    {
        GameManager.Instance.StartShop();
        //mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        hudPanel.SetActive(false);
        upgradeShopPanel.SetActive(true);

    }

    public void CloseShop() 
    {
        //if (gameOverPanel != null) { }
        gameOverPanel.SetActive(false);
        upgradeShopPanel.SetActive(false);
        hudPanel.SetActive(true);

    }
}
