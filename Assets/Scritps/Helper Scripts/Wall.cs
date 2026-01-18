using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    [Header("Wall Prefab")]
    [SerializeField] private GameObject wallPrefab;

    [Header("Placement")]
    [SerializeField] private Transform player;
    [SerializeField] private float spawnDistance = 1.5f;
    [SerializeField] private float wallLifeTime = 5f;

    [Header("Limit")]
    [SerializeField] private int limit = 3;

    private GameObject spawnedWall;
    private bool isRuntimeWall = false;

    private void Update()
    {
        if (!isRuntimeWall && Input.GetKeyDown(KeyCode.F) && limit > 0)
        {
            SpawnWall();
            limit--;
        }
    }

    private void SpawnWall()
    {
        Vector3 spawnPos =
            player.position + player.forward * spawnDistance;

        spawnPos.y += 1f;

        spawnedWall = Instantiate(
            wallPrefab,
            spawnPos,
            player.rotation
        );

        Wall wallLogic = spawnedWall.AddComponent<Wall>();
        wallLogic.isRuntimeWall = true;
        wallLogic.wallLifeTime = wallLifeTime;

        StartCoroutine(DestroyWallAfterTime(spawnedWall));
    }

    private IEnumerator DestroyWallAfterTime(GameObject wall)
    {
        yield return new WaitForSeconds(wallLifeTime);

        if (wall != null)
            Destroy(wall);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isRuntimeWall)
            return;

        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}