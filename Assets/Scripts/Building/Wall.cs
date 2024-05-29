using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Wall : MonoBehaviour, IBuilding
{
    public IBuilding.BuildingType Type { get => IBuilding.BuildingType.Wall; }

    [SerializeField] private PassiveEvent _wallEvent0;
    [SerializeField] private RouletteEvent _wallEvent1;
    [SerializeField] private RouletteEvent _wallEvent2;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.CurrentGameState is not GameManager.GameState.OnPlay) return;
        var people = GameManager.Instance.Get<IPeopleService>().People;
        IGameEvent wallEvent = _wallEvent0;
        if (people >= GameManager.Instance.GameInfo.WallSecondPeopleThreshold) wallEvent = _wallEvent2;
        else if (people >= GameManager.Instance.GameInfo.WallFirstPeopleThreshold) wallEvent = _wallEvent1;

        GameManager.Instance.Get<IEventService>().StartEvent(wallEvent, this);
    }
    public void setColor(int type)
    {
        foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
        {
            spr.color = GameManager.Instance.GameInfo.buildingColors[type];
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    { 
        setColor(1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        setColor(0);
    }
}
