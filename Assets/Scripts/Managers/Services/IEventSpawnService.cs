using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventSpawnService :IService
{
   public bool AddEvent(IGameEvent _event);
   public void StartSpawn();

    public void SpawnSetUpEvents();

    public void AddMultiplier(string key, float multiplier);
    public void AddMultiplier(string key, float multiplier, float duration);
    public bool RemoveMultiplier(string key);
}
