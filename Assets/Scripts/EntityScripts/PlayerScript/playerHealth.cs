using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{

    //private float health = 0f;

    //[SerializeField] private float maxHealth = 100f;
    [Header("Player's Health Manager")]
    [SerializeField] private Slider HealthSlider;

    [SerializeField] private float healthRegenRate = 5f;
    [SerializeField] private float delayBeforeRegen = 2f;

    private bool isRegenerating = false;
    private float timeSinceLastDamage = 0f;

    private void Start()
    {
        //PlayerStats.Instance.health = maxhealth;
        HealthSlider.maxValue = PlayerStats.Instance.maxhealth;
        HealthSlider.value = PlayerStats.Instance.maxhealth;
    }

    public void UpdateHealth(float mod)
    {
        PlayerStats.Instance.health += mod;

        if (PlayerStats.Instance.health > PlayerStats.Instance.maxhealth)
        {
            PlayerStats.Instance.health = PlayerStats.Instance.maxhealth;
        }
        else if (PlayerStats.Instance.health <= 0)
        {
            PlayerStats.Instance.health = 0;
            HealthSlider.value = 0f;
            gameObject.SetActive(false);

        }
        else
        {
            timeSinceLastDamage = 0f;
            if (!isRegenerating)
            {
                StartCoroutine(RegenerateHealth());
            }
        }
    }



    private void OnGUI()
    {
        float t = Time.deltaTime / 0.5f;
        HealthSlider.value = Mathf.Lerp(HealthSlider.value, PlayerStats.Instance.health, t);
    }

    private IEnumerator RegenerateHealth()
    {
        isRegenerating = true;

        while (PlayerStats.Instance.health < PlayerStats.Instance.maxhealth)
        {
            timeSinceLastDamage += Time.deltaTime;

            if (timeSinceLastDamage >= delayBeforeRegen)
            {
                PlayerStats.Instance.health += healthRegenRate * Time.deltaTime;
                if (PlayerStats.Instance.health > PlayerStats.Instance.maxhealth)
                {
                    PlayerStats.Instance.health = PlayerStats.Instance.maxhealth;
                }
            }
            yield return null;
        }

        isRegenerating = false;
    }

}
