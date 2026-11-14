using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController _swordlessController;
    [SerializeField] private RuntimeAnimatorController _swordController;

    private Animator _animator;
    private PlayerMode _lastMode;
    private bool _hasSetMode;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (PlayerModeManager.Instance == null)
            return;

        PlayerMode currentMode = PlayerModeManager.Instance.CurrentMode;

        if (!_hasSetMode || currentMode != _lastMode)
        {
            UpdateController(currentMode);
        }
    }

    private void UpdateController(PlayerMode mode)
    {
        if (mode == PlayerMode.Sword)
        {
            _animator.runtimeAnimatorController = _swordController;
        }
        else
        {
            _animator.runtimeAnimatorController = _swordlessController;
        }

        _lastMode = mode;
        _hasSetMode = true;
    }
}