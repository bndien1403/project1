using System;
using SazenGames.Skeleton;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public float totalHealth = 100f;
    public float playerHealth;
    
    public float damage =20f;
    private Animator _animator;
    public string ANIM_DIE;
    private PlayerController _player;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<PlayerController>();
        playerHealth = totalHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerHealth > 0)
        {
            if (other.gameObject.CompareTag("Knight"))
            {
                playerHealth -= damage;
                _animator.SetTrigger("TakeDamage");
                
            }
        }
      
    }
    

    private void Update()
    {   
        
        if (playerHealth <= 0f &&!_player._isDead )
        {
           
            Debug.Log("Die");
            _animator.ResetTrigger("TakeDamage");
            _animator.CrossFade(ANIM_DIE,0.1f);
            _player._isDead = true;
        }
    }
}
