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

    private void Awake()
    {
        currSlam = playerController.GetCurrentSlam();
        maxSlam = playerController.GetMaxSlam();

        //Running enable and disable in awake in order to differentiate similar logic in Update function calls.
        for (int i = jumps.Length - 1; i >= 0; i--)
        {
            if (i >= jumps.Length - maxSlam)
            {
                jumps[i].enabled = true;
            }
            else
            {
                jumps[i].enabled = false;
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        //Using Getters from Player controllers to obtain jump values used in logic.
        currSlam = playerController.GetCurrentSlam();
        maxSlam = playerController.GetMaxSlam();
        float currSlamRechargeValue = playerController.GetSlamFillCharge();

       // Debug.Log($"Current Slam: {currSlam} Max Slam: {maxSlam} \n Current Slam Refill Value: {currSlamRechargeValue} Threshold value: {playerController.GetThresholdValue()}");

        
        //Will create visible icons based on the max number of jumps.
        for (int i = jumps.Length - 1 ; i >= 0 ; i--)
        {
            Debug.Log($"index is :{i} \n Jumps.Length - (maxSlam - currSlam): {jumps.Length - (maxSlam - currSlam + 1)}");

            //For items that are greater than one directly above the current jump we set the jump to inactive and fill to 100.
            
            //This is one ahead of the current slam
            
            if (i > jumps.Length - (maxSlam - currSlam + 1))
            {
                jumps[i].sprite = inactiveJump;
                jumps[i].fillAmount = 0f;
            }
            else
            {
               jumps[i].sprite = activeJump;
               jumps[i].fillAmount = 100f;
            }
            if (i == jumps.Length - (maxSlam - currSlam))
            {
                Debug.Log(i);
                jumps[i].fillAmount = currSlamRechargeValue;
            }

        }
    }


}