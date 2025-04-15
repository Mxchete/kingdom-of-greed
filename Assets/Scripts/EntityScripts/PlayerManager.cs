using UnityEngine;
using Yarn;
using Yarn.Unity;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;  // Singleton instance

    public float health = 100;
    public int maxhealth = 100;
    public int gold = 0;
    public int experience = 0;
    public int favorability = 0;

    // Actual stats
    public int strength = 2;
    public int agility = 2;
    public int defense = 2;
    public int constitution = 2;

    //enemy counters
    public int entKillCount = 0;

    public InMemoryVariableStorage yarnVariables;

    void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initializing health
            health = maxhealth;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Find Yarn's variable storage
        yarnVariables = FindObjectOfType<InMemoryVariableStorage>();

        if (yarnVariables != null)
        {
            // Initialize Yarn variables
            yarnVariables.SetValue("$favorability", favorability);
            yarnVariables.SetValue("$gold", gold);
            yarnVariables.SetValue("$entKillCount", entKillCount);
            yarnVariables.SetValue("$killRequirement", 999);

        }
        else
        {
            Debug.LogWarning("InMemoryVariableStorage not found!");
        }
    }

    
    [YarnCommand("addGold")]
    public void AddGold(int amount)
    {
        gold += amount;
        favorability += amount; // Fixed spelling mistake
        yarnVariables.SetValue("$gold", gold);
        yarnVariables.SetValue("$favorability", favorability);
        Debug.Log("Gold is now: " + gold);
    }

    
    [YarnCommand("buy")]
    public void Buy(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            Debug.Log("Gold is now: " + gold);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }

 
    [YarnCommand("takeDamage")]
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health < 0) health = 0;
        Debug.Log("Health is now: " + health);
    }

    
    [YarnCommand("heal")]
    public void Heal(int amount)
    {
        health += amount;
        if (health > maxhealth)
        {
            health = maxhealth; 
        }
        Debug.Log("Player healed! Current health: " + health);
    }

    public void KillEnt()
    {
        entKillCount++; 

        if (yarnVariables != null)
        {
            yarnVariables.SetValue("$entKillCount", entKillCount); 
        }

        Debug.Log("Ents killed: " + entKillCount);
    }


}
