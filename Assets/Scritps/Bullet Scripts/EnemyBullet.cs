using UnityEngine;

public class EnemyBullet : Bullet
{
    private void Awake()
    {
        speed = 23f;
    }

    public override void Fire(Vector3 dir)
    {
        base.Fire(dir);
    }
}