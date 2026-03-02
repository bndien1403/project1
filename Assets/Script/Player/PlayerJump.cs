using SazenGames.Skeleton;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float jumpHeight = 1.5f;
    private Animator _animator;
    public string ANIM_JUMP;
  
    void Start()
    {
       
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.Instance.IsPlayingAction()) return;
       HandleClickJump();
    }

    void HandleClickJump()
    {
        var input = MobileButtonSystem.instance;
        if (input.isJump)
        {
            OnClickJump();
        }
        input.ResetInputJump();
    }

    void OnClickJump()
    { 
        var  _player = PlayerController.Instance;
            _player._velocity.y = Mathf.Sqrt(jumpHeight * -2f * _player.gravity);
                
            // Sử dụng Trigger cho Jump theo yêu cầu
            _animator.CrossFade(ANIM_JUMP, 0.01f);

            _player.LockState(0.1f); // Khóa ngắn để đảm bảo nhảy bắt đầu
        
        if (_player._isGrounded)
        {
            // Chỉ chuyển về Idle/Run nếu KHÔNG đang bận (Attack/Spawn)
            if (!_player.IsPlayingAction())
            {
                _player.ChangeAnimationState(_player.targetAnim);
            }
        }
    }
}
