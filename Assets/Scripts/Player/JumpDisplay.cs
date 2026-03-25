using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpDisplay : MonoBehaviour
{
    public int currJump;
    public int maxJump;

    public Sprite inactiveJump;
    public Sprite activeJump;
    public Image[] jumps;

    public PlayerController playerController;

    // Update is called once per frame
    void Update()
    {
        currJump = playerController.GetCurrentJump();
        maxJump = playerController.GetMaxJump();

        //Will create visible icons based on the max number of jumps.
        for (int i = 0; i < jumps.Length; i++) 
        {
            if (i < currJump)
            {
                jumps[i].sprite = activeJump;
            }
            else
            {
                jumps[i].sprite = inactiveJump;
            }
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
