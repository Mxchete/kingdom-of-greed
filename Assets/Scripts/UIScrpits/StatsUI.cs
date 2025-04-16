using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    private TMP_Text _statsText;
    private PlayerStats PlayerManager;
    // Start is called before the first frame update
    private void Awake()
    {
        _statsText = GetComponent<TMP_Text>();
        PlayerManager = FindAnyObjectByType<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        _statsText.text = $"Strength: {PlayerManager.strength.ToString()}\n" +
                   $"Agility: {PlayerManager.agility.ToString()}\n" +
                   $"Constitution: {PlayerManager.constitution.ToString()}";

    }
}
