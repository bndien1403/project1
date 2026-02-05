# How to Set Up Animation in Unity (Skeleton Player)

Use this guide with your **Animator** window and the clips in `Art/Animations`. Your **PlayerController** drives the Animator via parameters.

---

## Step 1: Open or create the Animator Controller

- In the **Project** window go to: `Assets/SazenGames/Skeleton/Art/Demo Animator Controllers/`
- Double‑click **idle** to open it in the **Animator** window, **or**
- Right‑click in that folder → **Create** → **Animator Controller** and name it e.g. `SkeletonPlayer`

---

## Step 2: Add parameters

In the **Animator** window, open the **Parameters** tab (left sidebar).

Click **+** and add these (names and types must match exactly):

| Type     | Name         |
|----------|--------------|
| Int      | **State**    |
| Float    | **Speed**    |
| Bool     | **IsGrounded** |
| Trigger  | **Attack**   |
| Trigger  | **Jump**     |
| Trigger  | **Respawn**  |
| Trigger  | **Death**    |
| Trigger  | **Revert**   |

**State** values used by PlayerController: **0** Idle, **1** Run, **2** Attack, **3** Jump, **4** Respawn, **5** Death, **6** Revert.

---

## Step 3: Create states and assign clips

In the **Layers** tab, on the **Base Layer** grid:

1. **Rename** the existing **anim** state to **Idle** (right‑click → Rename), then in the **Inspector** set **Motion** to `Skeleton_idle` from `Art/Animations`.
2. **Add new states** (right‑click in empty space → **Create State** → **Empty**), then name and assign:
   - **Run** → Motion: `Skeleton_run_forward`
   - **Attack** → Motion: `Skeleton_slash01` (or `Skeleton_slash02`)
   - **Jump** → Motion: `Skeleton_jump`
   - **Respawn** → Motion: `Skeleton_spawn` (or `Skeleton_revive`)
   - **Death** → Motion: `Skeleton_death`
   - **Revert** → Motion: `Skeleton_revive`

Drag the clip from **Project** (`Art/Animations`) onto the state, or select the state and set **Motion** in the **Inspector**.

Set **Idle** as the **Default** state: right‑click **Idle** → **Set as Layer Default State** (orange “play” icon on the state).

---

## Step 4: Add transitions

Transitions define when the Animator switches from one state to another.

### From **Entry** to **Idle**

- There should already be an arrow **Entry → anim** (your Idle). If you created a new controller, create a transition: right‑click **Entry** → **Make Transition** → click **Idle**.

### From **Any State** (one‑shot actions)

These let Attack, Jump, Respawn, Death, and Revert play from any state:

- **Any State → Attack**: Right‑click **Any State** → **Make Transition** → **Attack**. Select the arrow, in **Inspector** add **Condition**: **Attack** (Trigger). Uncheck **Has Exit Time**.
- **Any State → Jump**: Same, condition **Jump** (Trigger).
- **Any State → Respawn**: Condition **Respawn** (Trigger).
- **Any State → Death**: Condition **Death** (Trigger).
- **Any State → Revert**: Condition **Revert** (Trigger).

For each of these transitions, add a **Transition** back:

- **Attack → Idle**: Attack → Idle, condition **State** equals **0**. Or use **Has Exit Time** (e.g. 0.9) so it returns to Idle when the attack clip is almost done.
- **Jump → Idle**: Jump → Idle, condition **IsGrounded** = true (or **State** = 0).
- **Respawn → Idle**: Respawn → Idle, **State** = 0 or **Has Exit Time**.
- **Death** often has no transition back (you respawn via code, which triggers Respawn).
- **Revert → Idle**: **State** = 0 or **Has Exit Time**.

### Idle ↔ Run (State)

- **Idle → Run**: Idle → Run, condition **State** equals **1**.
- **Run → Idle**: Run → Idle, condition **State** equals **0**.

Optional: use **Speed** (float) instead — e.g. Idle → Run when **Speed** > 0.5.

---

## Step 5: Transition settings (Inspector)

When you click a transition arrow:

- **Has Exit Time**: Uncheck for trigger‑based transitions (Attack, Jump, etc.) so they play immediately.
- **Transition Duration**: e.g. 0.1–0.2 for snappy response.
- **Conditions**: Only the ones you added (e.g. **Attack** trigger, **State** = 1).

---

## Step 6: Assign the controller to the character

1. Select your **Skeleton** (e.g. in **Hierarchy**).
2. In **Inspector**, find the **Animator** component.
3. Set **Controller** to your Animator Controller (e.g. `idle` or `SkeletonPlayer`).
4. Ensure **PlayerController** is on the same GameObject (or that its **Animator** field references this Animator).

Press Play; moving (WASD), jumping (Space), and attacking (mouse/J) should drive the correct animations via **State**, **Speed**, **IsGrounded**, and the triggers.

---

## Quick reference: clip → state

| State   | Clip (in Art/Animations)   | Driven by              |
|---------|----------------------------|-------------------------|
| Idle    | Skeleton_idle              | State = 0               |
| Run     | Skeleton_run_forward       | State = 1               |
| Attack  | Skeleton_slash01 / 02      | Attack trigger          |
| Jump    | Skeleton_jump              | Jump trigger            |
| Respawn | Skeleton_spawn / revive     | Respawn trigger (code)  |
| Death   | Skeleton_death             | Death trigger (code)    |
| Revert  | Skeleton_revive            | Revert trigger (code)   |

If your controller has no **State** parameter, **PlayerController** will fall back to playing clips by these names instead of using parameters.
