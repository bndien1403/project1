using System.Collections;
using UnityEngine;


public class EnemyHealth : MonoBehaviour
{
  public float enemyHealth = 50f;
  public float currentHealth;
  public float damage = 20f;
  public bool isDead = false;
  private Animator _animator;
  public string ANIM_DEAD;
    [Header("Reward Settings")]
    public int coinReward ;

    private EnemyPool _enemyPool;

  private void Start()
  {
       
        // Khởi tạo lần đầu
        _animator  = GetComponent<Animator>();
    _enemyPool = GetComponent<EnemyPool>();
    currentHealth = enemyHealth;
  }

  // Hàm này chạy mỗi khi object được bật lại từ pool
  private void OnEnable()
  {
      currentHealth = enemyHealth;
      isDead = false;
  }

  private void OnTriggerEnter(Collider hit)
  {
      Debug.Log(hit.gameObject.name);
      if (hit.gameObject.CompareTag("Player Kinght"))
      {
        
          currentHealth -= damage;

      }
  }

    private void Update()
    {
        if (currentHealth <= 0f && !isDead)
        {
            Die();
        }
       
    }

  void Die()
  {
      isDead = true;
      if(_animator != null)
      {
          _animator.CrossFade(ANIM_DEAD, 0.1f);
          _animator.ResetTrigger("TakeDamage");
            CoinPool.Instance.GetCoinFromPool();
        }
        for (int i = 0; i < Random.Range(1,11); i++)
        {
            SpawnCoin();
        }

        // Đợi 2 giây cho animation chết chạy xong rồi trả về pool
        StartCoroutine(ReturnToPoolRoutine());
  }

  IEnumerator ReturnToPoolRoutine()
  {
      yield return new WaitForSeconds(2f);
      
      if (_enemyPool != null)
      {
          _enemyPool.ReturnToPool(gameObject);
      }
      else
      {
          // Nếu không dùng pool thì destroy như thường
          Destroy(gameObject);
      }
  }
    public void SpawnCoin() {
        GameObject coin = CoinPool.Instance.GetCoinFromPool();
        Vector3 randomOffset = Random.insideUnitSphere*1f;
        if(randomOffset.y <0f) randomOffset.y = -randomOffset.y;

        coin.transform.position = transform.position + randomOffset;
        coin.transform.rotation = Quaternion.identity;
        coin.SetActive(true);

        Rigidbody coinRb = coin.GetComponent<Rigidbody>();
        Vector3 exploreDir = Random.onUnitSphere;
        exploreDir.y = Mathf.Abs(exploreDir.y);
        coinRb.AddForce(exploreDir * Random.Range(2f, 4f), ForceMode.Impulse);

        ;

    }
}
