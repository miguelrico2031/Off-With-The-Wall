using System;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IBuilding : IPointerClickHandler//, IPointerEnterHandler, IPointerExitHandler 
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
    
}
