# 进度限制锁 (Progress Lock)

![Terraria Mod](https://img.shields.io/badge/Terraria-Mod-orange.svg)
![TModLoader](https://img.shields.io/badge/TModLoader-1.4.4+-blue.svg)

这是一个能定时解锁 NPC 和事件的 Mod，旨在防止Pro速通服务器。
A progress-control mod that time-locks NPCs and Events to prevent server speedrunning.

---

### 核心功能 | Features

* **三种锁定模式 (Three Modes)**
    * **到点解锁 (Time-based Unlock)**: 遵循预设的时间线。
    * **手动锁定 (Hard Lock)**: 强制锁定，即使到点也不会解锁。
    * **手动解锁 (Force Unlock)**: 强制开启，无视时间限制。
* **指令查询 (Command Search)**: 支持通过指令实时查询限制名单与倒计时。
* **广泛兼容性 (NPC Compatibility)**: 支持所有 Mod 的 NPC。
* **事件支持 (Event Support)**: 事件目前仅支持原版 (Vanilla)。
* **Boss 特殊适配 (Boss Support)**: 支持多部位 Boss 和多体节 Boss 的判定。
* **别名系统 (Alias System)**: 支持为 Boss 和事件添加自定义别名，简化指令输入。

---

### 指令手册 | Command Manual

**主指令 (Main Command):** `/progress` 

#### 1. 查看进度 (View Progress)
用于查询当前的限制状态和解锁倒计时。

| 用法 (Usage) | 类型别名 (Type Aliases) | 说明 (Description) |
| :--- | :--- | :--- |
| `/progress <分类> [页码]` | **NPC**: `npc`, `boss`, `b`, `n`, `1` | 查看 NPC/Boss 锁定状态 |
| `/progress <type> [page]` | **Event**: `event`, `e`, `i`, `2`, `事件`, `入侵` | 查看事件/入侵锁定状态 |

#### 2. 切换模式 (Toggle Mode)
手动干预特定目标的锁定状态。

| 用法 (Usage) | 动作别名 (Action Aliases) | 说明 (Description) |
| :--- | :--- | :--- |
| `/progress toggle <分类> <名称>` | `toggle`, `switch`, `t`, `切换` | 循环切换：自动 -> 强制锁 -> 强开 |

---

### 使用示例 | Examples

* `/progress npc 1` — 查看第一页 NPC 进度。
* `/progress t npc Eye of Cthulhu` — 切换克苏鲁之眼的锁定模式。
* `/progress switch event 哥布林军队` — 切换哥布林入侵的锁定模式。

---

> **注意 (Note)**: 这是一个非强制同步(NoSync)模组。
> This is a NoSync mod.
