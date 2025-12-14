using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
  [Header("References")]
  [SerializeField] private Transform player;
  private NavMeshAgent agent;
  private float shootTimer;
  private float shootCooldown = 0.2f;

  protected override void Awake()
  {
    base.Awake();
    agent = GetComponent<NavMeshAgent>();
  }

  private void Update()
  {
    if (!player) return;


    agent.SetDestination(player.position);


    Vector3 lookDir = player.position - transform.position;
    lookDir.y = 0f;
    if (lookDir != Vector3.zero)
      transform.rotation = Quaternion.LookRotation(lookDir);


    shootTimer += Time.deltaTime;

    if (shootTimer >= shootCooldown)
    {
      Shoot();
      shootTimer = 0f;
    }
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("PlayerBullet"))
    {
      PlayHitEffect(other.transform.position);
      TakeDamage(5);
    }
  }
}
