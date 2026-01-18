using UnityEditor;
using UnityEngine;

public class Player : Character
{

    private float rotationSpeed = 90f;
    private float attackRate = 0.2f;
    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= attackRate && Input.GetKey(KeyCode.K))
        {
            Attack();
            timer = 0f;
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            animator.SetBool("IsAttack", false);
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void Attack()
    {
        animator.SetBool("IsAttack", true);
        Shoot();
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Vertical");
        float turnInput = Input.GetAxisRaw("Horizontal");

        Vector3 move = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        float turn = turnInput * rotationSpeed * Time.fixedDeltaTime;
        Quaternion rotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * rotation);


        if (Mathf.Abs(turnInput) < 0.01f)
        {
            rb.angularVelocity = Vector3.zero;
        }

        animator.SetBool("IsRun", moveInput != 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyBullet"))
        {
            PlayHitEffect(other.transform.position);
            TakeDamage(5);
        }
    }
}
