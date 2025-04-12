using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    private TMP_Text _statsText;
    // Start is called before the first frame update
    private void Awake()
    {
        _statsText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _statsText.text = $"Strength: {PlayerStats.Instance.strength.ToString()}\n" +
                   $"Agility: {PlayerStats.Instance.agility.ToString()}\n" +
                   $"Constitution: {PlayerStats.Instance.constitution.ToString()}";

    }
}
