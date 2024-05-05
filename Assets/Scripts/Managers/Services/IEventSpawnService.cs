using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventSpawnService :IService
{
   public bool addEvent(IGameEvent _event);
}
