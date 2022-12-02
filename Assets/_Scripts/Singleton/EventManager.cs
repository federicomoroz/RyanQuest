using System.Collections.Generic;
public static class EventManager
{
    public delegate void EventReceiver(params object[] parameters);

    private static Dictionary<EventName, EventReceiver> _eventsData = new Dictionary<EventName, EventReceiver>();

    #region Events Methods
    public static void Subscribe(EventName name, EventReceiver method)
    {
        if (_eventsData.ContainsKey(name))
            _eventsData[name] += method;

        else
            _eventsData.Add(name, method);
    }

    public static void Unsubscribe(EventName name, EventReceiver method)
    {
        if (_eventsData.ContainsKey(name))
        {
            _eventsData[name] -= method;

            if (_eventsData[name] == null)
                _eventsData.Remove(name);
        }
    }
    public static void Trigger(EventName name, params object[] parameters)
    {
        if (_eventsData.ContainsKey(name))
            _eventsData[name].Invoke(parameters);
    }
    #endregion
}

public enum EventName
{
    PlayerDeath,
    PlayerInKillZone,
    PlayerCanPickObj,
    PlayerCanPullObj,
    PlayerCanShootObj,
    PlayerHpUpdate,
    PlayerRespawn,
    PlayerSpawned,
    PlayerCheckpoint,
    PlayerGotHurt,
    Pause,
    Boss1Death,
    VictoryOrb,
    PlayerInBossArea,
}
