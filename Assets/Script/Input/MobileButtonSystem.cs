using SazenGames.Skeleton;
using UnityEngine;

public class MobileButtonSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    private PlayerController _player;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void JumpButton()
    {
       
    }

    void AttackButton()
    {
       
    }
    
}
