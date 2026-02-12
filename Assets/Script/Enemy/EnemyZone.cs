using System;
using SazenGames.Skeleton;
using UnityEngine;

public class EnemyZone : MonoBehaviour
{
    [Header("SphereCast Setting")]
    public float radius = 1.0f;
    public  float maxDistance = 10.0f;
    public LayerMask playerMask;
    [Header("Enemy Setting")]
    public float speedEnemy = 4f;
    public bool isAttack = false;
    public bool isRun ;
    public float distance;
    
    //setting
    private PlayerController _player;
     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _player = FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Tìm player trong vùng (không phụ thuộc hướng forward) – SphereCast theo forward dễ miss khi enemy chưa quay về player
        if(_player._isDead) return;
        
        Collider[] hits = Physics.OverlapSphere(transform.position, maxDistance, playerMask);
        if (hits.Length > 0)
        {
            // Lấy player gần nhất
            Transform target = hits[0].transform;
            float nearest = Vector3.Distance(transform.position, target.position);
            for (int i = 1; i < hits.Length; i++)
            {
                float d = Vector3.Distance(transform.position, hits[i].transform.position);
                if (d < nearest)
                {
                    nearest = d;
                    target = hits[i].transform;
                }
            }

            distance = nearest;
          

            if (distance <= 2f)
            {
                isAttack = true;
                isRun = false;
            }
            else
            { 
                isRun = true;
                isAttack = false;
                transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
                transform.position = Vector3.MoveTowards(transform.position, target.position, speedEnemy * Time.deltaTime);
            }
        }
        else
        {
            isRun = false;
            isAttack = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawWireSphere(transform.position + transform.forward* maxDistance , radius);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward* maxDistance);
    }
} 