using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventService : IService
{
    public void StartEvent(IGameEvent e);
    // public void ResolveOutcomes(Outcomes outcomes);

    //public uint RewardMaxWait { get; }

    //public uint RewardMaxWait { get; }


}
