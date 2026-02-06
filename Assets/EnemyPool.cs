using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [Header("Enemy Pool Settings")]
    [SerializeField] private int maxEnemy = 10;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private bool allowExpand = true;
    private Queue<GameObject> pool = new Queue<GameObject>();

    [Header("Raycast / Detection")]
    [SerializeField] private Transform rayOrigin; // if null, will use this.transform
    [SerializeField] private float rayDistance = 10f;
    [SerializeField] private float rayRadius = 0.5f; // uses SphereCast
    [SerializeField] private LayerMask targetMask; // set to Player layer
    [SerializeField] private string targetTag = "Player";

    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint; // center where enemies spawn
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private int spawnCount = 3;
    [SerializeField] private float spawnDelayBetween = 0.2f;
    [SerializeField] private bool spawnOnce = true;
    [SerializeField] private float cooldownAfterWave = 5f;
    private bool isSpawning = false;
    private bool hasSpawned = false;
    private float lockedTill = 0f;
    
    void Start()
    {
        if (enemyPrefab == null)
        {
            return;
        }
        for (int i = 0; i < maxEnemy; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.SetActive(false);
            pool.Enqueue(enemy);
        }
    }
    
   private IEnumerator SpawnWave()
    {
       isSpawning = true;
       Vector3 center = spawnPoint!=null? spawnPoint.localPosition : transform.position;
       for(int i = 0 ; i< maxEnemy ; i++)
       {
          Vector3 rnd = Random.insideUnitSphere * spawnRadius;
            rnd.y = 0;
            Vector3 spawnPos = center + rnd;
            var enemy = GetFromPool();
            if (enemy != null)
            {
                enemy.transform.position = spawnPos;
                enemy.transform.rotation = Quaternion.identity;
                enemy.SetActive(true);
            }
            yield return new WaitForSeconds(3f);
       }
         isSpawning = false;
    }

    public GameObject GetFromPool()
    {
        if (pool.Count > 0)
        {
            var obj = pool.Dequeue();
            obj.transform.SetParent(null);
            return obj;
        }
        else if (allowExpand && enemyPrefab != null)
        {
            var obj = Instantiate(enemyPrefab);
            return obj;
            
        }
        return null;
    }

    public void ReturnToPool(GameObject obj)
    {
        if (obj == null) return;
        obj.SetActive(false);
        obj.transform.SetParent(transform);
        pool.Enqueue(obj);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < lockedTill) return;
        if (isSpawning) return;
        if (spawnOnce && hasSpawned) return;
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hit;
        if(Physics.SphereCast(ray, rayRadius, out hit, rayDistance, targetMask))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                StartCoroutine(SpawnWave());
               
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (rayOrigin == null) Gizmos.color = Color.yellow;
        else Gizmos.color = Color.yellow;

        Transform origin = rayOrigin != null ? rayOrigin : transform;
        Gizmos.DrawLine(origin.position, origin.position + origin.forward * rayDistance);
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.2f);
        // approximate sphere-cast visualization by drawing spheres along the ray
        int steps = 8;
        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            Vector3 pos = origin.position + origin.forward * (t * rayDistance);
            Gizmos.DrawWireSphere(pos, rayRadius);
        }

        Gizmos.color = Color.cyan;
        Vector3 center = spawnPoint != null ? spawnPoint.position : transform.position;
        Gizmos.DrawWireSphere(center, spawnRadius);
    }
}
