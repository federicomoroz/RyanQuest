
public class HpBar_Player : HealthBar
{


    private void OnEnable()
    {
        EventManager.Subscribe(EventName.PlayerHpUpdate, HandleHpChange);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventName.PlayerHpUpdate, HandleHpChange);
    }

}


