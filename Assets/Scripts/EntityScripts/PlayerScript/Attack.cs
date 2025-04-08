using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    [Header("Melee Object Reference")]

    public GameObject Melee;
    bool isAttacking = false;
    float atkDuration = 0.1f;
    float atkTimer = 0.25f;

    [Header("Aim Reference")]
    public Transform Aim;

    [Header("Bullet Settings")]
    public GameObject bullet; // Prefabs for range attacks
    public float fireForce = 10f;
    float shootCoolDown = 0.25f;
    float shootTimer = 0.5f;

    [Header("Other Reference")]
    public Player player;
    public PlayerMovement playerMovement;

    public Weapon weapon;

    Weapon currentWeapon;

    // Update is called once per frame
    void Update()
    {
        // Check Melee Timer
        CheckMeleeTimer();

        shootTimer += Time.deltaTime;

        // If statements for attack keys
        //If 'e' or left mouse clicker is pressed
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            // Attack
            OnAttack();

        }

        //If 'q' or right mouse clicker is pressed
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1))
        {
            // Attack Range
            OnShoot();


        }

    }

    void OnShoot()
    {
        if (shootTimer > shootCoolDown)
        {
            shootTimer = 0;
            GameObject intBullet = Instantiate(bullet, Aim.position, Aim.rotation);
            intBullet.GetComponent<Rigidbody2D>().AddForce(-Aim.up * fireForce, ForceMode2D.Impulse);
            Destroy(intBullet, 2f);
        }
    }
    void OnAttack()
    {
        if (!isAttacking)
        {
            isAttacking = true;

            // Trigger attack animation
            // PlayerMovement playerMovement = GetComponent<PlayerMovement>();
            // Player player = GetComponent<Player>();
            if (playerMovement != null && player != null)
            {
                player.Attack();


            }
            else{
                Debug.LogWarning("Missing Player or PlayerMovement componment on object with attack.cs");
            }

        }
    }

    void CheckMeleeTimer()
    {
        if (isAttacking)
        {
            atkTimer += Time.deltaTime;
            if (atkTimer >= atkDuration)
            {
                atkTimer = 0;
                isAttacking = false;
            }
        }
    }

}