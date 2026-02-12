# Hướng dẫn thiết lập Animation trong Unity (Nhân vật Skeleton)

Dùng hướng dẫn này với cửa sổ **Animator** và các clip trong thư mục `Art/Animations`. **PlayerController** sẽ điều khiển Animator qua các tham số (parameters).

---

## Bước 1: Mở hoặc tạo Animator Controller

- Trong cửa sổ **Project**, vào: `Assets/SazenGames/Skeleton/Art/Demo Animator Controllers/`
- Double-click **idle** để mở trong cửa sổ **Animator**, **hoặc**
- Chuột phải trong thư mục đó → **Create** → **Animator Controller** và đặt tên, ví dụ `SkeletonPlayer`

---

## Bước 2: Thêm Parameters (Tham số)

Trong cửa sổ **Animator**, mở tab **Parameters** (thanh bên trái).

Bấm **+** và thêm lần lượt (tên và kiểu phải đúng như bảng):

| Kiểu    | Tên            |
|---------|-----------------|
| Int     | **State**       |
| Float   | **Speed**       |
| Bool    | **IsGrounded**  |
| Trigger | **Attack**      |
| Trigger | **Jump**       |
| Trigger | **Respawn**     |
| Trigger | **Death**       |
| Trigger | **Revert**      |

**State** dùng trong PlayerController: **0** = Idle, **1** = Run, **2** = Attack, **3** = Jump, **4** = Respawn, **5** = Death, **6** = Revert.

---

## Bước 3: Tạo các State và gán clip

Trong tab **Layers**, trên **Base Layer**:

1. **Đổi tên** state **anim** thành **Idle** (chuột phải → Rename), sau đó trong **Inspector** đặt **Motion** là `Skeleton_idle` (kéo từ `Art/Animations`).
2. **Thêm state mới** (chuột phải vùng trống → **Create State** → **Empty**), đặt tên và gán Motion:
   - **Run** → Motion: `Skeleton_run_forward`
   - **Attack** → Motion: `Skeleton_slash01` (hoặc `Skeleton_slash02`)
   - **Jump** → Motion: `Skeleton_jump`
   - **Respawn** → Motion: `Skeleton_spawn` (hoặc `Skeleton_revive`)
   - **Death** → Motion: `Skeleton_death`
   - **Revert** → Motion: `Skeleton_revive`

Kéo clip từ **Project** (`Art/Animations`) thả vào từng state, hoặc chọn state rồi chọn **Motion** trong **Inspector**.

Đặt **Idle** làm state mặc định: chuột phải **Idle** → **Set as Layer Default State** (biểu tượng play màu cam trên state).

---

## Bước 4: Thêm Transition (Chuyển trạng thái)

Transition quy định khi nào Animator chuyển từ state này sang state kia.

### Từ **Entry** sang **Idle**

- Thường đã có mũi tên **Entry → anim** (Idle). Nếu tạo controller mới: chuột phải **Entry** → **Make Transition** → bấm vào **Idle**.

### Từ **Any State** (hành động một lần)

Để Attack, Jump, Respawn, Death, Revert có thể chạy từ bất kỳ state nào:

- **Any State → Attack**: Chuột phải **Any State** → **Make Transition** → **Attack**. Chọn mũi tên, trong **Inspector** thêm **Condition**: **Attack** (Trigger). Bỏ chọn **Has Exit Time**.
- **Any State → Jump**: Tương tự, condition **Jump** (Trigger).
- **Any State → Respawn**: Condition **Respawn** (Trigger).
- **Any State → Death**: Condition **Death** (Trigger).
- **Any State → Revert**: Condition **Revert** (Trigger).

Với mỗi state trên, thêm transition **quay về Idle**:

- **Attack → Idle**: Attack → Idle, condition **State** equals **0**. Hoặc bật **Has Exit Time** (ví dụ 0.9) để tự về Idle khi clip attack gần xong.
- **Jump → Idle**: Jump → Idle, condition **IsGrounded** = true (hoặc **State** = 0).
- **Respawn → Idle**: **State** = 0 hoặc **Has Exit Time**.
- **Death** thường không cần transition về (hồi sinh bằng code, sẽ kích **Respawn**).
- **Revert → Idle**: **State** = 0 hoặc **Has Exit Time**.

### Idle ↔ Run (theo State)

- **Idle → Run**: Idle → Run, condition **State** equals **1**.
- **Run → Idle**: Run → Idle, condition **State** equals **0**.

Tùy chọn: dùng **Speed** (float) thay — ví dụ Idle → Run khi **Speed** > 0.5.

---

## Bước 5: Cài đặt Transition (Inspector)

Khi chọn một mũi tên transition:

- **Has Exit Time**: Bỏ chọn với các transition dùng trigger (Attack, Jump, …) để chạy ngay.
- **Transition Duration**: khoảng 0.1–0.2 cho phản hồi nhanh.
- **Conditions**: Chỉ dùng đúng điều kiện đã thêm (ví dụ trigger **Attack**, **State** = 1).

---

## Bước 6: Gán Controller cho nhân vật

1. Chọn **Skeleton** (ví dụ trong **Hierarchy**).
2. Trong **Inspector**, tìm component **Animator**.
3. Đặt **Controller** là Animator Controller vừa chỉnh (ví dụ `idle` hoặc `SkeletonPlayer`).
4. Đảm bảo **PlayerController** gắn trên cùng GameObject (hoặc ô **Animator** của PlayerController trỏ đúng Animator này).

Bấm Play; di chuyển (WASD), nhảy (Space), đánh (chuột trái/J) sẽ điều khiển đúng animation qua **State**, **Speed**, **IsGrounded** và các trigger.

---

## Bảng tra nhanh: clip ↔ state

| State   | Clip (trong Art/Animations) | Điều khiển bởi        |
|---------|-----------------------------|------------------------|
| Idle    | Skeleton_idle               | State = 0              |
| Run     | Skeleton_run_forward        | State = 1              |
| Attack  | Skeleton_slash01 / 02       | Trigger Attack         |
| Jump    | Skeleton_jump               | Trigger Jump           |
| Respawn | Skeleton_spawn / revive     | Trigger Respawn (code) |
| Death   | Skeleton_death              | Trigger Death (code)   |
| Revert  | Skeleton_revive             | Trigger Revert (code)  |

Nếu controller không có parameter **State**, **PlayerController** sẽ tự chơi clip theo tên thay vì dùng parameters.
