using UnityEngine;
using UnityEngine.EventSystems;

public interface IBuilding : IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum State
    {
        Idle,
        HasEvent,
        HasReward
    }
    
    public enum BuildingType
    {
        Any,
        House,
        Church,
        Park
    }
    public State CurrentState { get; }
    public BuildingType Type { get; }

    public void SetReward(uint reward); //a acambiar por el tipo que corresponda
    public void SetEvent(object buildingEvent); //a acambiar por el tipo que corresponda
}
