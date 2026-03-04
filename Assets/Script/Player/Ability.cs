using UnityEngine;

namespace SazenGames.Skeleton
{
    /// <summary>
    /// Lớp cơ sở (ScriptableObject) cho tất cả các kỹ năng trong game.
    /// Mỗi kỹ năng (Jump, Dash, Fireball) sẽ kế thừa từ lớp này.
    /// </summary>
    public abstract class Ability : ScriptableObject
    {
        [Header("Ability Info")]
        public string abilityName;
        [TextArea] public string description;

        [Header("Timing")]
        public float cooldown = 1f;

        // Biến lưu trữ thời gian sử dụng của từng instance
        protected float lastUsedTimestamp = -999f; // Khởi tạo giá trị âm để có thể dùng ngay lần đầu

        /// <summary>
        /// Kiểm tra xem kỹ năng có thể được kích hoạt hay không.
        /// (VD: Đã hết cooldown chưa? Có đủ mana không?)
        /// </summary>
        /// <param name="owner">Đối tượng đang sở hữu và muốn dùng kỹ năng này.</param>
        /// <returns>True nếu có thể kích hoạt.</returns>
        public virtual bool CanActivate(AbilityComponent owner)
        {
            // Kiểm tra cooldown
            if (Time.time < lastUsedTimestamp + cooldown)
            {
                // Debug.LogWarning($"Ability '{abilityName}' is on cooldown.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Kích hoạt logic của kỹ năng.
        /// Sẽ được gọi bởi AbilityComponent sau khi CanActivate trả về true.
        /// </summary>
        /// <param name="owner">Đối tượng đang sở hữu và dùng kỹ năng này.</param>
        public virtual void Activate(AbilityComponent owner)
        {
            // Đánh dấu thời điểm sử dụng để tính cooldown
            lastUsedTimestamp = Time.time;
            Debug.Log($"Activating '{abilityName}' on {owner.gameObject.name}");
        }
    }
}
