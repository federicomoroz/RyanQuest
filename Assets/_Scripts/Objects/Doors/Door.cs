using UnityEngine;

public class Door : MonoBehaviour
{
                     private   Animator _animator;
                     protected bool     _isOpen = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected void Open()
    {
        if (!_isOpen)
        {
            _isOpen = true;
            _animator.SetBool("isOpening", true);
            _animator.SetBool("isClosing", false);
        }
    }

    protected void Close()
    {
        if (_isOpen)
        {
            _isOpen = false;
            _animator.SetBool("isClosing", true);
            _animator.SetBool("isOpening", false);
        }
    }
}

    

