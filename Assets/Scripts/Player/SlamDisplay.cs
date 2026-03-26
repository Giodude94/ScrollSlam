using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlamDisplay : MonoBehaviour
{
    [Header("Jump Variables")]
    public int currJump;
    public int maxJump;

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
        currJump = playerController.GetCurrentJump();
        maxJump = playerController.GetMaxJump();
        
        //Will update jump icons to deplete in desired top-down order.
        if (currJump < maxJump) { jumps[currJump].sprite = inactiveJump; }
        

        //Will create visible icons based on the max number of jumps.
        for (int i = 0; i < jumps.Length; i++)
        {
            /*
            if (i < currJump)
            {
                jumps[currJump].sprite = activeJump;
            }
            else
            {
                jumps[currJump].sprite = inactiveJump;
            }
            */
            if (i < maxJump)
            {
                jumps[i].enabled = true;
            }
            else
            {
                jumps[i].enabled = false;
            }
        }
    }


}