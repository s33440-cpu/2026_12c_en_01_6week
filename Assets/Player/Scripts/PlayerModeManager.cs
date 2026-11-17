
using UnityEngine;

public class PlayerModeManager : MonoBehaviour
{
    public static PlayerModeManager Instance;

    public PlayerMode CurrentMode { get; private set; } = PlayerMode.Swordless;
    public bool RangedAttackUnlocked { get; private set; }

    [SerializeField] private bool _startWithSword = false;
    [SerializeField] private bool _startWithRangedAttack = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (_startWithSword)
            CurrentMode = PlayerMode.Sword;

        RangedAttackUnlocked = _startWithRangedAttack;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void EquipSword()
    {
        CurrentMode = PlayerMode.Sword;
    }

    public void UnequipSword()
    {
        CurrentMode = PlayerMode.Swordless;
    }

    public void EnableRangedAttack()
    {
        RangedAttackUnlocked = true;
    }

    public void DisableRangedAttack()
    {
        RangedAttackUnlocked = false;
    }
}
