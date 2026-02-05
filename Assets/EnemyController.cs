using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Attack,
        Chase // ruot duoi
    }

    private EnemyState _enemyCurrentState;
    EnemyZone _enemyZone;
    Animator _animatorEnemy;
    public string ATTACK_ANIM;
    public string IDLE_ANIM;
    public string CHASE_ANIM;
    private float _lockedTill; // Biến dùng để khóa logic chuyển đổi trong thời gian ngắn
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _enemyZone = GetComponent<EnemyZone>();
        _animatorEnemy = GetComponent<Animator>();
    }

    void Start()
    { 
        _enemyCurrentState = EnemyState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
       ChangeState();
       switch (_enemyCurrentState)
       {
           case EnemyState.Idle:
               Pending();
               break;
           case EnemyState.Attack:
               AttackPlayer();
               break;
           case EnemyState.Chase:
               ChaseToPlayer();
               break;
       }
    }

    void ChangeState()
    {
        if (_enemyZone.isAttack)
        {
            _enemyCurrentState = EnemyState.Attack;
        }

        if (_enemyZone.isHit)
        {
            _enemyCurrentState = EnemyState.Chase;
        }
        else
        {
            _enemyCurrentState = EnemyState.Idle;
        }
    }

    void ChaseToPlayer()
    {
        _animatorEnemy.CrossFade(CHASE_ANIM, 0.01f);
       
    }

    void AttackPlayer()
    {
     
            _animatorEnemy.CrossFade(ATTACK_ANIM, 0.01f);
            LockState(1f);
       
        
    }

    void Pending()
    {
        _animatorEnemy.CrossFade(IDLE_ANIM, 0.01f);
    }
    //khoa chuyen doi animation
    void LockState(float duration)
    {
        _lockedTill = Time.time + duration;
    }
}
