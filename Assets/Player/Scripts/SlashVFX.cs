using UnityEngine;

public class SlashVFX : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.enabled = false;
    }

    public void Play()
    {
        _spriteRenderer.enabled = true;
        _animator.Play("SwordSlash", 0, 0f);
    }

    public void Hide()
    {
        _spriteRenderer.enabled = false;
    }
}