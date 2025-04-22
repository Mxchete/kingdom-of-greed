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
    private PlayerStats PlayerManager;
    private bool isRegenerating = false;
    private float timeSinceLastDamage = 0f;

    private void Start()
    {
        //PlayerStats.Instance.health = maxhealth;
        PlayerManager = FindAnyObjectByType<PlayerStats>();
        HealthSlider.maxValue = PlayerManager.maxhealth;                
        HealthSlider.value = PlayerManager.health;         

    }

    public void UpdateHealth(float mod)
    {
        PlayerManager.health += mod;

        if (PlayerManager.health > PlayerManager.maxhealth)
        {
            PlayerManager.health = PlayerManager.maxhealth;
        }
        else if (PlayerManager.health <= 0)
        {
            PlayerManager.health = 0;
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

        // Update health slider value here as well
        HealthSlider.value = PlayerManager.health;
    }




    private void OnGUI()
    {
        float t = Time.deltaTime / 0.5f;
        HealthSlider.value = Mathf.Lerp(HealthSlider.value, PlayerManager.health, t);
    }

    private IEnumerator RegenerateHealth()
    {
        isRegenerating = true;

        while (PlayerManager.health < PlayerManager.maxhealth)
        {
            timeSinceLastDamage += Time.deltaTime;

            if (timeSinceLastDamage >= delayBeforeRegen)
            {
                PlayerManager.health += healthRegenRate * Time.deltaTime;
                if (PlayerManager.health > PlayerManager.maxhealth)
                {
                    PlayerManager.health = PlayerManager.maxhealth;
                }
            }
            yield return null;
        }

        isRegenerating = false;
    }

}
