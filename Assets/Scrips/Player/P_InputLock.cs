using UnityEngine;

public static class PlayerControlLock
{
    private static bool _movementLocked = false;
    private static bool _attackLocked = false;
    private static bool _dashLocked = false;
    private static bool _globalLocked = false;

    // =========================
    // MOVIMIENTO
    // =========================
    public static bool MovementLocked => _globalLocked || _movementLocked;
    public static void LockMovement() => _movementLocked = true;
    public static void UnlockMovement() => _movementLocked = false;

    // =========================
    // ATAQUE
    // =========================
    public static bool AttackLocked => _globalLocked || _attackLocked;
    public static void LockAttack() => _attackLocked = true;
    public static void UnlockAttack() => _attackLocked = false;

    // =========================
    // DASH
    // =========================
    public static bool DashLocked => _globalLocked || _dashLocked;
    public static void LockDash() => _dashLocked = true;
    public static void UnlockDash() => _dashLocked = false;

    
    public static bool GlobalLocked => _globalLocked;
    public static void LockAll() => _globalLocked = true;
    public static void UnlockAll()
    {
        _globalLocked = false;
        _movementLocked = false;
        _attackLocked = false;
        _dashLocked = false;
    }
}
