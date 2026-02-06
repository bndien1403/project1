using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Pending,
        Attack,
        Chase // ruot duoi
    }

    public EnemyState _enemyCurrentState;
    EnemyZone _enemyZone;
    Animator _animatorEnemy;
    public string ATTACK_ANIM;
    public string PENDING_ANIM;
    public string CHASE_ANIM;
    private float _lockedTill; // Biến dùng để khóa logic chuyển đổi trong thời gian ngắn
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
       
    }

    void Start()
    { 
        _enemyZone = GetComponent<EnemyZone>();
        _animatorEnemy = GetComponent<Animator>();
        _enemyCurrentState = EnemyState.Pending;
    }

    // Update is called once per frame
    void Update()
    {
       ChangeState();
       switch (_enemyCurrentState)
       {
           case EnemyState.Pending:
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
        if (_enemyZone == null) return;
        if (_enemyZone.isAttack)
        {
            _enemyCurrentState = EnemyState.Attack;
        }
        else if (_enemyZone.isRun)
        {
            _enemyCurrentState = EnemyState.Chase;
        }
        else
        {
            _enemyCurrentState = EnemyState.Pending;
        }
    }

    void ChaseToPlayer()
    {
        if(_animatorEnemy== null ) return;
        _animatorEnemy.CrossFade(CHASE_ANIM, 0.01f);
       
    }

    void AttackPlayer()
    {
        if(_animatorEnemy== null ) return;
        if (!IsPlaying(ATTACK_ANIM))
        {
            _animatorEnemy.CrossFade(ATTACK_ANIM, 0.01f);
            LockState(0.25f); // Khóa 0.25s để Animator kịp chuyển sang Attack
        }
            
        
        
    }

    void Pending()
    {
        if(_animatorEnemy== null ) return;
        _animatorEnemy.CrossFade(PENDING_ANIM, 0.01f);
    }
    //khoa chuyen doi animation
    void LockState(float duration)
    {
        _lockedTill = Time.time + duration;
    }
    bool IsPlaying(string animName)
    {
        return _animatorEnemy.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }
}
