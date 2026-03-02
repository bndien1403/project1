using System;
using UnityEngine ;
using SazenGames.Skeleton;

public class PlayerSkillManager : MonoBehaviour
{
    [Header("Input Settings")] public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode attack1Key = KeyCode.Alpha1;
    public KeyCode attack2Key = KeyCode.Alpha2;
    public KeyCode attack3Key = KeyCode.Alpha3;
    public KeyCode attack4Key = KeyCode.Alpha4;
    [Header("animation")] private Animator _animator;
    public string ANIM_IDLE  ;
    public string ANIM_ATTACK;
    public string ANIM_SKILL1;
    public string ANIM_SKILL2;
    public string ANIM_SKILL3;
    [Header("State")] 
    private Vector3 _velocity;
    private bool _isGrounded;
    public bool _isDead = false;
    private string _currentAnimState;
    private float _lockedTill;
    private bool isBusy = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsPlayingAction()) return;
        HandleInputAttack();
    }

    void HandleInputAttack()
    {
        var input = MobileButtonSystem.instance;

        if (input.isAttack)
        {
            OnAttack();
        }
        else if(input.isSkill1)
        {
            OnSkill1();
        }
        input.ResetInputAttack();
        if (!IsPlayingAction())
        {
            ChangeAnimationState(ANIM_IDLE);
        }
    }


    void OnAttack()
    {
        Debug.Log("đã tấn công");
        ChangeAnimationState(ANIM_ATTACK);
        LockState(0.25f); // Khóa 0.25s để Animator kịp chuyển sang Attack
       
    }

    void OnSkill1()
    {
    
        ChangeAnimationState(ANIM_SKILL1);
        LockState(0.25f);
      
    }

    void OnSkill2()
    {
      
        ChangeAnimationState(ANIM_SKILL2);
        LockState(1f);
       
    }

    void OnSkill3()
    {
        isBusy = true;
        ChangeAnimationState(ANIM_SKILL3);
        LockState(1f);
        Invoke(nameof(ResetBusy), 0.8f);
    }

    private void ResetBusy()
    {
        isBusy = false;
    }


    void ChangeAnimationState(string newState, float transitionTime = 0.1f)
    {
        if (_currentAnimState == newState) return;

        _animator.CrossFade(newState, transitionTime);
        _currentAnimState = newState;
    }

    // Hàm khóa trạng thái trong một khoảng thời gian
    void LockState(float duration)
    {
        _lockedTill = Time.time + duration;
    }

    bool IsPlayingAction()
    {
        // 1. Ưu tiên: Nếu đang trong thời gian khóa (vừa bấm nút), coi như đang hành động
        if (Time.time < _lockedTill) return true;

        // 2. Kiểm tra Animator State thực tế
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(ANIM_ATTACK) || stateInfo.IsName(ANIM_SKILL1) || stateInfo.IsName(ANIM_SKILL2) ||
            stateInfo.IsName(ANIM_SKILL3))
        {
            return stateInfo.normalizedTime < 1.0f;
        }


        return false;
    }
}