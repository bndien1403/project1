# Mỗi state là 1 Animator Controller – Cách chạy animation

Khi **mỗi state** (Idle, Run, Attack, Jump, …) là **một file Animator Controller riêng** (.controller), bạn không dùng Parameters hay tên state – bạn **đổi Controller** đang gán trên component Animator.

---

## Cách hoạt động

- **Một Controller, nhiều state:** Dùng 1 Animator Controller, bên trong có nhiều state (Idle, Run, Attack, …). Script gọi `Play("tên state")` hoặc SetTrigger/SetInt.
- **Mỗi state là 1 Controller:** Có nhiều file .controller (ví dụ Idle.controller, Run.controller, Attack.controller). Script **đổi** `animator.runtimeAnimatorController` = Controller tương ứng với state hiện tại.

---

## Cách làm (không chỉnh PlayerController cũ)

1. **Tạo từng Controller** (nếu chưa có):  
   Idle.controller, Run.controller, Attack.controller, Jump.controller, Death.controller, Respawn.controller, Revert.controller – mỗi file chỉ chứa 1 state (một clip tương ứng).

2. **Dùng script điều khiển riêng** (xem file mẫu `PlayerControllerByControllers.cs`):
   - Trong Inspector gán **mảng** Runtime Animator Controller: [0] Idle, [1] Run, [2] Attack, [3] Jump, [4] Respawn, [5] Death, [6] Revert.
   - Khi đổi state (idle → run, run → attack, …), gán:
     - `animator.runtimeAnimatorController = controllers[(int)state];`
   - Như vậy animation chạy vì **mỗi state đều là 1 Controller** và Animator đang dùng đúng Controller đó.

3. **Tạm thời chưa chỉnh dự án:** Chỉ cần giữ cách trên trong đầu; khi muốn dùng, tạo script mới (như mẫu), gán mảng Controller trong Inspector và gọi đổi state tương ứng.

---

## Tóm tắt

| Loại setup | Cách chạy animation |
|------------|----------------------|
| 1 Controller, nhiều state | `animator.Play("tên state")` hoặc SetTrigger/SetInt Parameters |
| **Mỗi state là 1 Controller** | `animator.runtimeAnimatorController = controllerIdle;` (hoặc Run, Attack, …) khi đổi state |

Script điều khiển animation: `Scripts/PlayerAnimationController.cs` – thêm vào cùng GameObject với PlayerController, gán từng Controller trong Inspector. Tự đồng bộ theo PlayerController.CurrentState.
