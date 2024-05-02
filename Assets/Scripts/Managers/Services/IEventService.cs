using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventService : IService
{
    public void StartEvent(Event _event);
    public void GiveReward(Event.EventReward _reward);

    //public uint RewardMaxWait { get; }

    //public uint RewardMaxWait { get; }


}
