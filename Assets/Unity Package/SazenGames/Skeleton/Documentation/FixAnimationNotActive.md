# Fix: Animation Not Active - Set Animation Names in Inspector

## Vấn đề
Animation không chạy vì **tên state trong Animator Controller** không khớp với **tên animation** trong PlayerController.

## Giải pháp: Đặt tên animation trong Inspector

### Bước 1: Kiểm tra tên state trong Animator
1. Mở **Animator Controller** (double-click trong Project).
2. Xem **tên các state** (ví dụ: `root|combat idle`, `root|run`, `root|jump`, `root|slash01`, `root|death`, `root|spawn`, `root|revive`).
3. **Copy chính xác** tên state (bao gồm cả `root|` nếu có).

### Bước 2: Đặt tên trong PlayerController Inspector
1. Chọn **Player** (Skeleton) trong Hierarchy.
2. Trong **Inspector**, tìm component **Player Controller**.
3. Mở phần **"Animation – khi KHÔNG dùng Parameters"**.
4. Điền **chính xác** tên state từ Animator vào từng ô:

| Ô trong Inspector | Tên state trong Animator (ví dụ) |
|-------------------|----------------------------------|
| **Anim Idle**     | `root|combat idle`               |
| **Anim Run**      | `root|run`                       |
| **Anim Attack**   | `root|slash01` (hoặc `root|slash 02`) |
| **Anim Jump**     | `root|jump`                      |
| **Anim Respawn**  | `root|spawn`                     |
| **Anim Death**    | `root|death`                     |
| **Anim Revert**   | `root|revive`                    |

**Lưu ý:** Tên phải **khớp 100%**, kể cả khoảng trắng và ký tự đặc biệt (`|`, `-`, v.v.).

### Bước 3: Kiểm tra
1. Bấm **Play**.
2. Thử **WASD** (di chuyển) → animation Run phải chạy.
3. Thử **Space** (nhảy) → animation Jump phải chạy.
4. Thử **Chuột trái** hoặc **J** (đánh) → animation Attack phải chạy.

---

## Nếu vẫn không chạy

### Kiểm tra 1: Animator Controller đã gán chưa?
- Chọn Player → Inspector → **Animator** component → **Controller** phải có Animator Controller (không để trống).

### Kiểm tra 2: State có Motion chưa?
- Mở Animator Controller → chọn từng state → Inspector → **Motion** phải có animation clip (không để None).

### Kiểm tra 3: Entry → State có transition chưa?
- Trong Animator, phải có mũi tên **Entry → Idle state** (hoặc state mặc định).

### Kiểm tra 4: Script có tìm thấy Animator không?
- PlayerController → **Animator** field phải trỏ đúng Animator component (hoặc để trống nếu Animator trên cùng GameObject).

---

## Tên state phổ biến trong project này

Dựa trên Animator của bạn, các tên state có thể là:
- `root|combat idle` (idle)
- `root|run` (chạy)
- `root|walk forward` (đi bộ)
- `root|jump` (nhảy)
- `root|slash01`, `root|slash 02`, `root|stab` (đánh)
- `root|death` (chết)
- `root|spawn` (hồi sinh)
- `root|revive` (revert)
- `root|Fall` (rơi)
- `root|Take Damage` (bị đánh)

**Quan trọng:** Copy **chính xác** tên từ Animator Controller (có thể có `root|` ở đầu, khoảng trắng, chữ hoa/thường).
