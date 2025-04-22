using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 1;
    public enum WeaponType { Melee = 0, Sword = 1, Axe = 2, HornClaw = 3, Bullet = 4 }
    public WeaponType weaponType;

    public GameObject[] weapons; // Array of all available weapons
    protected GameObject currentWeapon; // Currently equipped weapon

    private void Update()
    {
        // Hotkeys for equipping weapons
        if (Input.GetKeyDown(KeyCode.Z)) // Press 'Z' to equip weapon at index 0 (Melee)
        {
            EquipWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.X)) // Press 'X' to equip weapon at index 1 (Sword)
        {
            EquipWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.C)) // Press 'C' to equip weapon at index 2 (Axe)
        {
            EquipWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.V)) // Press 'V' to equip weapon at index 3 (Axe)
        {
            EquipWeapon(3);
        }
        // Add more hotkeys as needed
    }

    // Equip a specific weapon by index
    public void EquipWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length)
        {
            Debug.LogWarning("Invalid weapon index!");
            return;
        }

        // Deactivate the current weapon
        if (currentWeapon != null)
        {
            Debug.Log($"Deactivating current weapon: {currentWeapon.name}");
            currentWeapon.SetActive(false);
        }

        // Activate the new weapon
        currentWeapon = weapons[index];
        Debug.Log($"Activating new weapon: {currentWeapon.name}");
        currentWeapon.SetActive(true);

        // Update the weaponType based on the index
        switch (index)
        {
            case 0:
                weaponType = WeaponType.Melee;
                break;
            case 1:
                weaponType = WeaponType.Sword;
                break;
            case 2:
                weaponType = WeaponType.Axe;
                break;
            case 3:
                weaponType = WeaponType.HornClaw;
                break;
            case 4:
                weaponType = WeaponType.Bullet;
                break;
            default:
                Debug.LogWarning("Unknown weapon index!");
                break;
        }

        Debug.Log($"Equipped weapon: {currentWeapon.name}, WeaponType: {weaponType} ({(int)weaponType})");

        // Update animation or other logic if needed
        UpdateWeaponAnimation();
    }

    // Update animation based on the current weapon type
    private void UpdateWeaponAnimation()
    {
        Animator animator = GetComponentInParent<Animator>();
        if (animator != null)
        {
            Debug.Log($"Updating animation for weapon type: {weaponType} ({(int)weaponType})");
            animator.SetInteger("WeaponType", (int)weaponType);
        }
        else
        {
            Debug.LogWarning("Animator not found!");
        }
    }

    // Handle damage when the weapon collides with an enemy
    // In Weapon.cs (or wherever your attack logic is)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss")) // Make sure boss has "Boss" tag
        {
            Entity enemy = other.GetComponent<Entity>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"Hit {other.name} for {damage} damage");
            }
        }
    }
}