using System;
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
}
