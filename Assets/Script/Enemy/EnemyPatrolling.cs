using SazenGames.Skeleton;
using UnityEngine;

public class EnemyPatrolling : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Movement Settings")] [SerializeField]
    private float moveSpeed = 3f; // toc do di chuyen

    // toc do quay
    [SerializeField] private float rotationSpeed = 5f;

    //pham vi hoat dong
    [SerializeField] private float roamRadius = 10f;

    private Vector3 startPosition;

    // check khoang cach cua player voi obstacle
    public float obstacleCheckDistance = 2f;

    //check layer
    public LayerMask obstacleLayerMask;
    private Vector3 moveDirection;
    private Vector3 _velocity;

    [Header("Setting")] private CharacterController _controller;
     
    private EnemyController _enemyController;

    private PlayerController _player;
    //animation
    [Header(" animation")] private Animator animator;
    public string ANIM_IDLE;
    public string ANIM_WALK;
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _enemyController = GetComponentInChildren<EnemyController>();
        _player = FindAnyObjectByType<PlayerController>();
         //vi tri spawn
         transform.position = startPosition;
    }
    void Update()
    {    
        if(_player._isDead) return;
        if(_enemyController== null) return;
        if (_enemyController._enemyCurrentState == EnemyController.EnemyState.Pending
            && _enemyController._pendingCurrentState == EnemyController.PendingState.Patrol)
        {
            Move();
        }
       
    }

    public void Move()
    {
        if (_enemyController == null || _controller == null) return;
        if (!_controller.enabled) return;
        if (_controller.isGrounded && _velocity.y < 2.0f)
        {
            _velocity.y = -5f;
        }
         //gravity   
        _velocity.y += (-15f) * Time.deltaTime;
        if (moveDirection == Vector3.zero|| Vector3.Distance(transform.position, startPosition)>roamRadius)
        {
            PickRandomDirection();
        }

        if (IsObstaclesAhead())
        {
            RotateRandom();
        }

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            Vector3 gravityMove = Vector3.up * _velocity.y;
            Vector3 horizontalMove = transform.forward * moveSpeed ;
            Vector3 motion = horizontalMove + gravityMove;
            _controller.Move(motion * Time.deltaTime);
//          animator.CrossFade(ANIM_WALK , 0.01f);
        }

    }

    private void RotateRandom()
    {
      
        float randomAngle = Random.Range(-120f, 120f);
        moveDirection = Quaternion.Euler(0f,randomAngle, 0f) * transform.forward;
    }

    private bool IsObstaclesAhead()
    {
     return Physics.Raycast(transform.position+Vector3.up *0.3f, transform.forward, obstacleCheckDistance, obstacleLayerMask);
    }

    public void PickRandomDirection()
    {
        float distanceToSpawn = Vector3.Distance(transform.position, startPosition);
        if (distanceToSpawn > roamRadius)
        {
            // 2. Nếu vượt quá bán kính, hướng di chuyển phải là hướng quay về tâm
            moveDirection = (startPosition - transform.position).normalized;
        }
        else
        {
            float randomAngle = Random.Range(-120f, 120f);
            moveDirection = Quaternion.Euler(0f,randomAngle, 0f) * Vector3.forward;
        }
       
    }
}