using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SazenGames.Skeleton
{
    /// <summary>
    /// Component được gắn vào Player/Enemy để quản lý và thực thi các Ability.
    /// </summary>
    public class AbilityComponent : MonoBehaviour
    {
        // Kéo thả các file Ability (ScriptableObject) bạn đã tạo vào đây trong Inspector.
        [SerializeField]
        private List<Ability> startingAbilities;

        // Danh sách các instance của ability tại runtime.
        // Việc này đảm bảo mỗi nhân vật có bộ cooldown riêng.
        private List<Ability> runtimeAbilities;

        void Awake()
        {
            // Tạo các instance riêng biệt của từng ability để không dùng chung cooldown
            runtimeAbilities = new List<Ability>();
            if (startingAbilities != null)
            {
                foreach (var abilitySO in startingAbilities)
                {
                    if (abilitySO != null)
                    {
                        // Dùng Instantiate để tạo một bản sao của ScriptableObject tại runtime
                        var instance = Instantiate(abilitySO);
                        instance.name = abilitySO.name; // Giữ lại tên gốc để dễ tìm kiếm
                        runtimeAbilities.Add(instance);
                    }
                }
            }
        }

        /// <summary>
        /// Cố gắng kích hoạt một kỹ năng dựa theo tên.
        /// Đây là hàm sẽ được gọi từ PlayerController hoặc AI của Enemy.
        /// </summary>
        /// <param name="abilityName">Tên của kỹ năng muốn kích hoạt (phải trùng với tên trong ScriptableObject).</param>
        public void TryActivateAbility(string abilityName)
        {
            if (runtimeAbilities == null) return;

            // Tìm kỹ năng trong danh sách runtime
            var ability = runtimeAbilities.FirstOrDefault(a => a.abilityName == abilityName);

            if (ability == null)
            {
                Debug.LogError($"Ability '{abilityName}' not found on {gameObject.name}.");
                return;
            }

            // Ủy quyền việc kiểm tra điều kiện cho chính Ability đó
            if (ability.CanActivate(this))
            {
                // Nếu đủ điều kiện, thực thi kỹ năng
                ability.Activate(this);
            }
        }
    }
}
