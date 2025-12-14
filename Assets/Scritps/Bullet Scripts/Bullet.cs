using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected float speed;
    protected Vector3 direction;

    public virtual void Fire(Vector3 dir)
    {
        direction = dir;
    }

    protected virtual void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}