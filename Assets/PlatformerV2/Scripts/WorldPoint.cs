using UnityEngine;

public class WorldPoint
{
    public Vector3 position;
    public Vector3 scale;
    public Vector3 velocity;

    public WorldPoint(Vector3 pos, Vector3 sc, Vector3 vel)
    {
        position = pos;
        scale = sc;
        velocity = vel;
    }
}
