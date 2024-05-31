

using System.Collections;

public interface IAIService : IService
{
    public void AddWaypoint(AIWaypoint waypoint);

    public void TargetWall();
    public IEnumerator TargetHouses();
}