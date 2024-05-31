using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class Wall : MonoBehaviour, IBuilding
{
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
    
    // private void SetColor(int type)
    // {
    //     foreach (SpriteRenderer spr in GetComponentsInChildren<SpriteRenderer>())
    //     {
    //         spr.color = GameManager.Instance.GameInfo.buildingColors[type];
    //     }
    // }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponentInChildren<SpriteRenderer>().enabled = true;
        // SetColor(1);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponentInChildren<SpriteRenderer>().enabled = false;

        // SetColor(0);
    }
}
