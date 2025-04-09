using System.Collections;
using UnityEngine;

public class EntBoss : Entity
{
    [Header("Movement Settings")]
    [SerializeField] private float chaseDistance = 8f;

    [Header("Attack Settings")]
    [SerializeField] private float stompDamage = 15f;
    [SerializeField] private float stompRadius = 4f;
    [SerializeField] private float vineDamage = 10f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float timeBetweenAttacks = 2f;

    [Header("Vine Settings")]
    [SerializeField] private int vineCount = 6;
    [SerializeField] private float spawnRadius = 2f;
    [SerializeField] private float vineActiveDuration = 1f;

    [Header("References")]
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private GameObject vinePrefab;
    [SerializeField] private ParticleSystem stompDustParticles;
    [SerializeField] private Transform[] summonPoints;
    [SerializeField] private Transform[] vinePoints;

    private bool faceRight = true;
    private bool isAttacking = false;
    private Transform player;
    private Coroutine behaviorRoutine;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        behaviorRoutine = StartCoroutine(BossBehavior());
    }

    private void Update()
    {
        if (!isAttacking)
        {
            FacePlayer();
            MoveTowardsPlayer();
        }
    }

    #region Movement
    private void FacePlayer()
    {
        bool playerIsRight = player.position.x > transform.position.x;
        if (playerIsRight != faceRight) Flip();
    }

    private void Flip()
    {
        faceRight = !faceRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void MoveTowardsPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) > chaseDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
    #endregion

    #region Attacks
    private IEnumerator BossBehavior()
    {
        while (health > 0)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);

            if (!isAttacking)
            {
                int attackPattern = Random.Range(0, 5);

                switch (attackPattern)
                {
                    case 0: yield return LaughTaunt(); break;
                    case 1: yield return VineStompAttack(); break;
                    case 2: yield return RegularStompAttack(); break;
                    case 3: yield return SummonMinions(); break;
                }
            }
        }
    }

    private IEnumerator LaughTaunt()
    {
        isAttacking = true;
        animator.SetTrigger("Laugh");
        yield return new WaitForSeconds(2.5f);
        isAttacking = false;
    }

    private IEnumerator VineStompAttack()
    {
        isAttacking = true;
        animator.SetTrigger("VineStomp");
        yield return new WaitForSeconds(0.5f); // Wait for animation to hit impact frame
        isAttacking = false;
    }

    private IEnumerator RegularStompAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Stomp");
        yield return new WaitForSeconds(0.4f); // Wait for impact frame
        isAttacking = false;
    }

    private IEnumerator SummonMinions()
    {
        isAttacking = true;
        animator.SetTrigger("Summon");
        yield return new WaitForSeconds(0.8f); // Wait for summon animation

        foreach (Transform point in summonPoints)
        {
            Instantiate(minionPrefab, point.position, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1f);
        isAttacking = false;
    }
    #endregion

    #region Animation Events
    public void AE_SpawnStompVines()
    {
        float angleStep = 360f / vineCount;

        for (int i = 0; i < vineCount; i++)
        {
            float angle = i * angleStep;
            Vector2 spawnPos = (Vector2)transform.position + new Vector2(
                Mathf.Sin(angle * Mathf.Deg2Rad) * spawnRadius,
                Mathf.Cos(angle * Mathf.Deg2Rad) * spawnRadius
            );
            SpawnVine(spawnPos);
        }

        // Apply stomp damage at center
        ApplyAreaDamage(transform.position, stompRadius, stompDamage);

        // Visual effects
        if (stompDustParticles != null)
            Instantiate(stompDustParticles, transform.position, Quaternion.identity);
    }

    public void OnStompImpact()
    {
        ApplyAreaDamage(transform.position, stompRadius, stompDamage);
    }
    #endregion

    #region Helpers
    private void SpawnVine(Vector2 position)
    {
        GameObject vine = Instantiate(vinePrefab, position, Quaternion.identity);
        vine.GetComponent<VineController>().Initialize(vineDamage);
    }

    private void ApplyAreaDamage(Vector2 center, float radius, float damage)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius, playerLayer);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                hit.GetComponent<Player>().TakeDamage(damage);

                // Optional knockback
                Vector2 knockbackDir = (hit.transform.position - transform.position).normalized;
                hit.GetComponent<Rigidbody2D>().AddForce(knockbackDir * 5f, ForceMode2D.Impulse);
            }
        }
    }
    #endregion

    protected override void Die()
    {
        StopCoroutine(behaviorRoutine);
        animator.SetTrigger("Die");
        GetComponent<Collider2D>().enabled = false;
        rb.simulated = false;
        enabled = false;
        Destroy(gameObject, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stompRadius);
    }
}