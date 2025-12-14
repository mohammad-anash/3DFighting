using UnityEngine;

public class PlayerBullet : Bullet
{
    private void Awake()
    {
        speed = 25f;
    }

    public override void Fire(Vector3 dir)
    {
        base.Fire(dir);
    }
}