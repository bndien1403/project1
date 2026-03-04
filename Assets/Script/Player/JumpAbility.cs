using UnityEngine;

namespace SazenGames.Skeleton
{
    [CreateAssetMenu(fileName = "New JumpAbility", menuName = "Abilities/Jump Ability")]
    public class JumpAbility : Ability
    {
        public float jumpForce = 10f;

        public override void Activate(AbilityComponent owner)
        {
            base.Activate(owner); // Quan trọng: để tính cooldown

            // Logic nhảy cụ thể
            // Giả sử Player có CharacterController hoặc Rigidbody
            // Nếu dùng CharacterController như trong PlayerController hiện tại:
            
            PlayerController playerController = owner.GetComponent<PlayerController>();
            if (playerController != null && playerController._isGrounded)
            {
                // Thay đổi vận tốc Y để nhảy
                // Công thức nhảy vật lý: v = sqrt(h * -2 * g)
                playerController._velocity.y = Mathf.Sqrt(jumpForce * -2f * playerController.gravity);
            }
        }
        
        public override bool CanActivate(AbilityComponent owner)
        {
            if (!base.CanActivate(owner)) return false;

            // Kiểm tra thêm điều kiện: Phải đang đứng trên mặt đất mới được nhảy
            PlayerController playerController = owner.GetComponent<PlayerController>();
            if (playerController != null)
            {
                return playerController._isGrounded;
            }
            return false;
        }
    }
}
