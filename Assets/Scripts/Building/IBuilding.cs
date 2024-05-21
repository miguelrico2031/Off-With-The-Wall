using System;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IBuilding : IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum State
    {
        Idle,
        HasEvent,
        HasReward,
        Burned
    }
    
    public enum BuildingType
    {
        House,
        Square,
        School,
        PoliceStation,
        FuckDonalds,
        Wall
    }
    public State CurrentState { get; }
    public BuildingType Type { get; }

    public void SetReward(uint reward, Action onCollected); //a acambiar por el tipo que corresponda
    public void SetEvent(IGameEvent _event, Action onDispatched); //a acambiar por el tipo que corresponda

    public void GetBurned();
}
