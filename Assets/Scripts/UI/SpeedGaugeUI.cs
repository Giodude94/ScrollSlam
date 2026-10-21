using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedGaugeUI : MonoBehaviour
{

    [SerializeField] public Rigidbody2D player;
    [SerializeField] public float maxSpeed = 100.0f;

    [SerializeField] public float minSpeedArrowAngle;
    [SerializeField] public float maxSpeedArrowAngle;
    private float speed = 0.0f;

    [Header("UI")]
    public TextMeshProUGUI speedLabel;
    public RectTransform arrow;

    private void Update()
    {
        if (player != null){
            speed = player.velocity.magnitude;
        }

        if (speedLabel != null){
            speedLabel.text = ((int)speed) + "m/s";
        }

        if (arrow != null) {
            arrow.localEulerAngles = new Vector3 (0, 0, Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / maxSpeed));
        }

    }

}
