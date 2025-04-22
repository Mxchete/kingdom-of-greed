using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera cm;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offSet;

    private void Start()
    {
        if (cm == null)
        {
            cm = Camera.main;
        }
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        float targetValue = Mathf.Clamp01(currentValue / maxValue);
        StopAllCoroutines();
        StartCoroutine(SmoothHealthUpdate(targetValue));
    }
    private IEnumerator SmoothHealthUpdate(float targetValue)
    {
        float startValue = slider.value;
        float elapsedTime = 0f;
        float duration = 0.3f; // Adjust for smoother transition

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            slider.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            yield return null;
        }

        slider.value = targetValue; // Ensure final value is set
    }

    void Update()
    {
        transform.rotation = cm.transform.rotation;
        transform.position = target.position + offSet;
    }
}
