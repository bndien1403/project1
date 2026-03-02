using System;
using SazenGames.Skeleton;
using UnityEngine;

public class MobileButtonSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isAttack;
    public bool isJump;
    public bool isSkill1;
    public static MobileButtonSystem instance;
    
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickJumpButton()
    {
        isJump = true;
        Debug.Log("Nhảy nhảy nhảy");
    }

    public void OnclickAttackButton()
    {
        isAttack = true;
        Debug.Log("Attack");
    }

    public void OnClickSkill1Button()
    {
        isSkill1 = true;
        Debug.Log("Skill1");
    }
  

    public void ResetInputAttack()
    {
         isAttack = false;
         isJump = false;
         isSkill1 = false;
    }

    public void ResetInputJump()
    {
        isJump = false;
    }

    
}
