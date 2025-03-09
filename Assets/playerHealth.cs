using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{

    private float health = 0f;

    [SerializeField] private float maxHealth = 100f;

    [SerializeField] private Slider HealthSlider;

    [SerializeField] private float healthRegenRate = 5f;
    [SerializeField] private float delayBeforeRegen = 2f;

    private bool isRegenerating = false;
    private float timeSinceLastDamage = 0f;




    private void Start()
    {
        health = maxHealth;
        HealthSlider.maxValue = maxHealth;
        HealthSlider.value = health;
    }

    public void UpdateHealth(float mod){
        health += mod;

        if(health > maxHealth){
            health = maxHealth;
        }
        else if(health <= 0){
            health = 0f;
            HealthSlider.value = 0f;
            Destroy(gameObject);
        }
        else{
            timeSinceLastDamage = 0f;
            if(!isRegenerating){
                StartCoroutine(RegenerateHealth());
            }
        }
    }

    private void OnGUI()
    {
        float t = Time.deltaTime / 0.5f;
        HealthSlider.value = Mathf.Lerp(HealthSlider.value, health, t);
    }

    private IEnumerator RegenerateHealth(){
        isRegenerating = true;
        
        while(health < maxHealth){
            timeSinceLastDamage += Time.deltaTime;

            if(timeSinceLastDamage >= delayBeforeRegen){
                health += healthRegenRate * Time.deltaTime;
                if(health > maxHealth){
                    health = maxHealth;
                }
            }
            yield return null;
        }

        isRegenerating = false;
    }

}
