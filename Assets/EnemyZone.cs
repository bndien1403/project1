using System;
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
     
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        

       if( Physics.SphereCast(transform.position, radius, transform.forward, out hit, maxDistance, playerMask)){
            Vector3 target = hit.transform.position;
            distance = Vector3.Distance(transform.position, target);
            transform.LookAt(target);
            transform.position = Vector3.MoveTowards(transform.position, target, speedEnemy * Time.deltaTime);
            isRun = true;
            if (distance <= 1.6f)
            {
                isAttack = true;
                isRun = false;
            }
            else 
            {
                isAttack = false;
                isRun = true;
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