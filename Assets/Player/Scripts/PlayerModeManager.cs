using UnityEngine;

public class PlayerModeManager : MonoBehaviour
{
    public static PlayerModeManager Instance;

    public PlayerMode CurrentMode { get; private set; } = PlayerMode.Swordless;
    [SerializeField] private bool _startWithSword = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (_startWithSword)
        {
            CurrentMode = PlayerMode.Sword;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void EquipSword()
    {
        CurrentMode = PlayerMode.Sword;
    }

    public void UnequipSword()
    {
        CurrentMode = PlayerMode.Swordless;
    }
}
