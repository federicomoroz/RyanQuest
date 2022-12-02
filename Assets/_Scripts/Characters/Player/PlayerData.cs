using UnityEngine;

public class PlayerData
{
    public Vector3    lastPosition;
    public Quaternion lastRotation;

    public PlayerData(Vector3 position, Quaternion rotation)
    {
        this.lastPosition = position;
        this.lastRotation = rotation;
    }
}
