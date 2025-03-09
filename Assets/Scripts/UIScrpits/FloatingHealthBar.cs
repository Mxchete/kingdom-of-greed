using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{

    [SerializeField] private Slider slider;
    [SerializeField] private Camera cm;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offSet;


    public void UpdateHealthBar(float currentValue, float maxValue){
        slider.value = currentValue / maxValue;

    }

    void Update()
    {
        transform.rotation = cm.transform.rotation;
        transform.position = target.position + offSet;
    }


}
