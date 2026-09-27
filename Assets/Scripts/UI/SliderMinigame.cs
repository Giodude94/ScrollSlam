using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SliderMinigame : MonoBehaviour
{
    [Header("Indicator")]
    [SerializeField] private Slider indicator;
    [SerializeField] private float indicatorSpeed = 1f;

    [Header("Target")]
    [SerializeField] private RectTransform targetZone;
    [SerializeField] private RectTransform handleSlideArea;
    [SerializeField] private float targetSize = .5f;

    [Header("References")]
    [SerializeField] private PlayerController playerController;

    private float targetMin;
    private float targetMax;

    private bool isActive;
    private bool movingForward = true;

    private void Awake()
    {
        this.gameObject.SetActive(false);
    }
    void Update()
    {
        SetUILocation();
        //If the slider is not active then we backout of the logic immediately.
        if (!isActive) { return; }

        MoveIndicator();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckResult();
        }
    }

    public void StartMinigame()
    {
        //Making the UI object visible
        this.gameObject.SetActive(true);

        isActive = true;
        movingForward = true;

        indicator.minValue = 0f;
        indicator.maxValue = 1f;
        indicator.value = 0f;

        //Randomize target position
        targetMin = Random.Range(0f, 1f - targetSize);
        targetMax = targetMin + targetSize;
        //Update UI
        UpdateTargetVisual();
    }
    private void UpdateTargetVisual()
    {
        float width = handleSlideArea.rect.width;
        float targetWidth = (targetMax - targetMin) * width;
        float targetCenter = (targetMax + targetMin) / 2f;

        //Updating sizeDelta of values below increase the width/height of the target area.
        targetZone.sizeDelta = new Vector2(targetWidth, targetZone.sizeDelta.y);
        targetZone.anchoredPosition = new Vector2((targetCenter - 0.5f) * width, targetZone.anchoredPosition.y);

    }
    private void MoveIndicator()
    {
        float direction = movingForward ? 1f : -1f;

        indicator.value += direction * indicatorSpeed * Time.unscaledDeltaTime;

        if (indicator.value >= 1f)
        {
            indicator.value = 1f;
            movingForward = false;
        }
        else if (indicator.value <= 0f)
        {
            indicator.value = 0f;
            movingForward = true;
        }
    }
    private void CheckResult()
    {
        isActive = false;

        bool success = indicator.value >= targetMin && indicator.value <= targetMax;

        if (success)
        {
            SuccessfulHit();

        }
        else
        {
            FailedHit();
        }
        this.gameObject.SetActive(false);
    }
    private void SuccessfulHit()
    {
        Debug.Log("Successful hit.");
        playerController.ApplySuccessfulBoost(.5f);
        GameManager.Instance.EndChallengeState();


    }
    private void FailedHit()
    {
        Debug.Log("Failed hit.");
        playerController.ApplyFailedBoost(.5f);
        GameManager.Instance.EndChallengeState();

    }

    private void SetUILocation()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(playerController.transform.position + Vector3.up * 1f);
        handleSlideArea.GetComponentInParent<Transform>().position = screenPosition;
        indicator.transform.localScale = new Vector3(.25f,.25f,.25f);
        indicator.transform.position = screenPosition;
    }

}
