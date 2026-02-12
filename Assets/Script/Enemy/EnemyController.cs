using System;
using SazenGames.Skeleton;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Pending,
        Attack,
        Chase // ruot duoi
    }
    public enum PendingState {
        Idle ,
        Patrol
        
    }

    public EnemyState _enemyCurrentState;
    public PendingState _pendingCurrentState;
    EnemyZone _enemyZone;
   
    Animator _animatorEnemy;
    
    public string ATTACK_ANIM;
    public string IDLE_ANIM;
    public string RUN_ANIM;
    private float _lockedTill; // Biến dùng để khóa logic chuyển đổi trong thời gian ngắn
    [Header("Timming")]
    [SerializeField] private float minStateTime = 2f;
    [SerializeField] private float maxStateTime = 5f;
    private float stateTimer;

    private PlayerController _player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Awake()
    { 
        _enemyZone = GetComponent<EnemyZone>();
        _animatorEnemy = GetComponent<Animator>();
        _player = FindAnyObjectByType<PlayerController>();
        _enemyCurrentState = EnemyState.Pending;
    }

    // Update is called once per frame
    void Update()
    {
      
        UpdateState();
       
    }
    void ChangeState(EnemyState newState)
    {
        _enemyCurrentState = newState;
        switch (_enemyCurrentState)
        {
            case EnemyState.Pending: 
                Debug.Log("pending1");
                Pending();
                break;
            case EnemyState.Attack:
                    Attack();
                break;
            case EnemyState.Chase:
                Chase(); 
                break;
        }
      
            
    }

    void UpdateState()
    {
        EnemyState nextState;
        // Debug.Log("next state:" + nextState);
        if (_enemyZone == null) return;
        if (_enemyZone.isRun&& !_player._isDead) 
        { 
            nextState = EnemyState.Chase;
        }
        else if (_enemyZone.isAttack && !_player._isDead)
        {
            nextState = EnemyState.Attack;
        }
        else
        {
            nextState = EnemyState.Pending;
        }

        if (nextState != _enemyCurrentState)
        {
           ChangeState(nextState);
        }
        if (_enemyCurrentState == EnemyState.Pending)
        {
            Pending();
        }
    }
    
    void Pending()
    {
       
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            RandomPendingState(GetRandomPendingState());
            
        }
        
   
    }

   

    void Attack()
    {
        Debug.Log("Attack");
        _animatorEnemy.CrossFade(ATTACK_ANIM, 0.1f);
        
    }

    void Chase()
    {
        Debug.Log("Chase");
        _animatorEnemy.CrossFade(RUN_ANIM, 0.1f);
    }

    void RandomPendingState(PendingState newState)
    {
        _pendingCurrentState = newState;
        stateTimer = newState switch
        {
          PendingState.Idle =>Random.Range(3f,6f) ,
          PendingState.Patrol=>Random.Range(3f,6f),
          _=>3f
        };
        if (_animatorEnemy == null) return;
        switch (_pendingCurrentState)
        {
            case  PendingState.Idle:
                Debug.Log("Idle");
                _animatorEnemy.CrossFade(IDLE_ANIM, 0.1f);
                break;
            case PendingState.Patrol:
               
                _animatorEnemy.CrossFade(RUN_ANIM, 0.1f);
                break;
        }

    }
    private PendingState GetRandomPendingState()
    {
        int count = Enum.GetValues(typeof(PendingState)).Length;
        
        return (PendingState)Random.Range(0, count);
    }

    private void OnDisable()
    {
        _enemyCurrentState = EnemyState.Pending;
    }
}
