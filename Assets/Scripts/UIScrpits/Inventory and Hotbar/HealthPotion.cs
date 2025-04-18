using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Health Potion", menuName = "Inventory/Health Potion")]
public class HealthPotion : Item
{
    public int healthAmount = 20; // Amount of HP restored
    public float cooldown = 4f;
    //private float nextUseTime = 0f;
    private bool isOnCooldown = false;
    private PlayerStats playerController;
    public override void UseItem()
    {
        playerController = FindAnyObjectByType<PlayerStats>();
        if (!isOnCooldown && playerController.health < playerController.maxhealth)
        {
            playerController.Heal(healthAmount); // Call the player's heal function
            Debug.Log("Used " + Name + ", restored " + healthAmount + " HP!");
            Destroy(gameObject);

            isOnCooldown = true;

            GameObject.FindObjectOfType<MonoBehaviour>().StartCoroutine(Cooldown());
            //nextUseTime = Time.time + cooldown;
            
        }


        Debug.Log("Potion is on cooldown, please wait.");
        return;
        

        
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
        Debug.Log("Potion cooldown finished.");
    }
}
