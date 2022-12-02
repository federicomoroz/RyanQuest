using UnityEngine;
public class PlayerCollisions
{
    private Player   _player;
    private Animator _animator;
    private Rigidbody _rb;


    #region BUILDER
    public PlayerCollisions()   { }

    public PlayerCollisions SetPlayer(Player player)
    {
        _player = player;
        return this;
    }

    public PlayerCollisions SetAnimator(Animator animator)
    {
        _animator = animator;
        return this;
    }

    public PlayerCollisions SetRigidbody(Rigidbody rb)
    {
        _rb = rb;
        return this;
    }
    #endregion

    public void VirtualOnTriggerEnter(Collider other)
    {

        if (other.CompareTag("RampEdge"))
            _player.e_playerOnRampEdge?.Invoke();

    }

    public void VirtualOnTriggerStay(Collider other)
    {
        if (other.CompareTag("RampEdge") && (_rb.velocity.y > 0.5f || _rb.velocity.magnitude > 1f))
            _player.Locomotion.ApplyGravityCompensation();
    }

    public void VirtualOnTriggerExit(Collider other)
    {
        if (other.CompareTag("RampEdge"))
            _player.e_playerOffRampEdge?.Invoke();
    }



    public void HandleDamage(int dmg)
    {
        _player.SubstractHp(dmg);        
        EventManager.Trigger(EventName.PlayerGotHurt);
        FXManager.Instance.CameraShake(_player.transform.position, 0.9f);
        _animator.SetTrigger("Got_Hurt");
        float hpRatio = _player.CurrentHp / _player.MaxHp;
        Debug.Log($"Player health ratio is: {hpRatio}");  
        EventManager.Trigger(EventName.PlayerHpUpdate, hpRatio);
    }
    


    
}
