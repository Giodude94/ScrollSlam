using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInvisWhileRunning : MonoBehaviour
{
    // Start is called before the first frame update
    public void InvisibleWhileRunning()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Running)
        {
            this.gameObject.SetActive(false);
        }
    }
}
