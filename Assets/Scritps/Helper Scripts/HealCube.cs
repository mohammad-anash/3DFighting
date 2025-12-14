using UnityEngine;
using System.Collections;

public class HealCubeManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject healCubePrefab;

    [Header("Spawn Range")]
    [SerializeField] private float minX = -20f;
    [SerializeField] private float maxX = 20f;
    [SerializeField] private float minZ = 0f;
    [SerializeField] private float maxZ = 40f;
    [SerializeField] private float groundY = 0.5f;

    [Header("Heal")]
    [SerializeField] private int minHeal = 45;
    [SerializeField] private int maxHeal = 50;
    [SerializeField] private float pickupDistance = 1.2f;
    [SerializeField] private float respawnDelay = 5f;
    private GameObject currentCube;
    private bool isRespawning;

    [SerializeField]
    private AudioClip HealSound;

    private void Start()
    {
        SpawnCube();
    }

    private void Update()
    {
        if (currentCube == null || isRespawning)
            return;

        float distance = Vector3.Distance(player.position, currentCube.transform.position);

        if (distance <= pickupDistance)
        {
            HealPlayer();
        }
    }

    private void HealPlayer()
    {
        Character character = player.GetComponent<Character>();
        if (character == null)
            return;

        int healAmount = Random.Range(minHeal, maxHeal + 1);
        character.Heal(healAmount);

        AudioManager.Instance.PlaySFX(HealSound);

        Destroy(currentCube);
        StartCoroutine(RespawnCube());
    }

    private IEnumerator RespawnCube()
    {
        isRespawning = true;
        yield return new WaitForSeconds(respawnDelay);
        SpawnCube();
        isRespawning = false;
    }

    private void SpawnCube()
    {
        Vector3 pos = new Vector3(
            Random.Range(minX, maxX),
            groundY,
            Random.Range(minZ, maxZ)
        );

        currentCube = Instantiate(healCubePrefab, pos, Quaternion.identity);
    }
}
