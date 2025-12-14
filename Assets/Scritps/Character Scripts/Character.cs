using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Components")]
    protected Rigidbody rb;
    protected Animator animator;
    protected Collider characterCollider;

    [Header("Stats")]
    public float moveSpeed = 5f;
    public int maxHealth = 100;
    protected int currentHealth;

    [SerializeField]
    private AudioClip DeadClip;

    [Header("Effects")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private ParticleSystem hitEffect;

    [Header("Shooting")]
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int poolSize = 15;

    protected ObjectPool<Bullet> bulletPool;

    [SerializeField]
    private Transform spawnPosition;



    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        characterCollider = GetComponent<Collider>();

        spawnPosition.position = transform.position;
        currentHealth = maxHealth;

        if (bulletPrefab != null)
        {
            bulletPool = new ObjectPool<Bullet>(bulletPrefab, poolSize, transform);
        }

        if (CompareTag("Player") && UIController.Instance != null)
        {
            UIController.Instance.UpdateHealth(currentHealth, maxHealth);
        }
    }

    public virtual void TakeDamage(int damage)
    {
        HitSound();

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        // ✅ Update health bar ONLY for Player
        if (CompareTag("Player") && UIController.Instance != null)
        {
            UIController.Instance.UpdateHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Shoot()
    {
        if (bulletPool == null || firePoint == null) return;

        Bullet bullet = bulletPool.Get();

        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        bullet.Fire(firePoint.forward);

        StartCoroutine(ReturnBulletAfterDelay(bullet, 2f));
    }

    private System.Collections.IEnumerator ReturnBulletAfterDelay(Bullet bullet, float time)
    {
        yield return new WaitForSeconds(time);

        if (bullet != null && gameObject.activeSelf)
        {
            bulletPool.ReturnToPool(bullet);
        }
    }

    public void PlayHitEffect(Vector3 hitPoint)
    {
        if (hitEffect == null) return;

        Vector3 spawnPoint = hitPoint;

        if (characterCollider != null)
        {
            spawnPoint = characterCollider.ClosestPoint(hitPoint);
        }

        ParticleSystem fx = Instantiate(hitEffect, spawnPoint, Quaternion.identity);
        fx.Play();

        Destroy(fx.gameObject, fx.main.duration);
    }

    protected void HitSound()
    {
        if (hitSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(hitSound);
        }
    }


    public void Respawn()
    {
        currentHealth = maxHealth;

        transform.position = spawnPosition.position;
        transform.rotation = Quaternion.identity;

        gameObject.SetActive(true);

        if (CompareTag("Player"))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            UIController.Instance.UpdateHealth(currentHealth, maxHealth);
        }
    }


    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UIController.Instance.UpdateHealth(currentHealth, maxHealth);
    }



    protected virtual void Die()
    {
        // Score update
        if (CompareTag("Enemy"))
        {
            UIController.Instance.IncreasePlayerScore();
        }
        else if (CompareTag("Player"))
        {
            AudioManager.Instance.PlaySFX(DeadClip);
            UIController.Instance.IncreaseEnemyScore();
        }

        // Tell UIController who died
        UIController.Instance.OnRoundEnd(this);

        gameObject.SetActive(false);
    }
}
