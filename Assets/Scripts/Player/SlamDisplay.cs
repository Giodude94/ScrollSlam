using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlamDisplay : MonoBehaviour
{
    [Header("Jump Variables")]
    public int currSlam;
    public int maxSlam;

    [Header("Images")]
    public Sprite inactiveJump;
    public Sprite activeJump;
    public Image[] jumps;

    [Header("References")]
    public PlayerController playerController;

    // Update is called once per frame
    void Update()
    {
        //Using Getters from Player controllers to obtain jump values used in logic.
        currSlam = playerController.GetCurrentSlam();
        maxSlam = playerController.GetMaxSlam();
        float currSlamRechargeValue = playerController.GetSlamFillCharge();

        Debug.Log($"Current Slam: {currSlam} Max Slam: {maxSlam} \n Current Slam Refill Value: {currSlamRechargeValue} Threshold value: {playerController.GetThresholdValue()}");

        //Will update jump icons to deplete in desired top-down order.
        if (currSlam < maxSlam) 
        { 
            jumps[maxSlam - currSlam - 1].sprite = inactiveJump;
            jumps[maxSlam - currSlam - 1].fillAmount = 0f;
        }
        else if(currSlam > maxSlam)
        {
            jumps[maxSlam - currSlam].fillAmount = 0f;
        }

        //Code will not run when curr jump is equal to max jump. ie. when the slam gauge is full
        if(currSlam <= maxSlam)
        {
            if (playerController.GetSlamFillCharge() > 0)
            {
                jumps[maxSlam - currSlam - 1].fillAmount = playerController.GetSlamFillCharge();
            }
        }

        //Will create visible icons based on the max number of jumps.
        for (int i = 0; i < jumps.Length; i++)
        {   
            if (i < maxSlam)
            {
                jumps[i].enabled = true;
                if (i < currSlam + 1)
                {
                    Debug.Log($"index is: {i}");

                }
            }
            else
            {
                jumps[i].enabled = false;
            }
           //if ( i > currSlam)
            //{
             //   Debug.Log($"index is: {i}");
            //}
            //if (jumps[i] != jumps[maxSlam - currSlam]) 
            //{
            //    jumps[maxSlam - currSlam - 1].sprite = inactiveJump;
            //    jumps[maxSlam - currSlam - 1].fillAmount = 0f;
            //}
        }
    }


}