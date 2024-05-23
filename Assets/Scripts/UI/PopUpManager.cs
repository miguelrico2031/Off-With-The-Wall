using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class PopUpManager : MonoBehaviour, IPopUpService
{
    [field:SerializeField] public Canvas PopUpCanvas { get; private set; }
    
    [SerializeField] private PopUp _popUpPrefab;

    private ObjectPool<PopUp> _objectPool;
    private Dictionary<Building, PopUp> _housePopUps = new();

    [SerializeField] private Sprite _rewardSprite;
    [SerializeField] private Sprite _choiceEventSprite;
    [SerializeField] private Sprite _chooseNameEventSprite;
    [SerializeField] private Sprite _chooseSloganEventSprite;
    [SerializeField] private Sprite _drawEventSprite;
    [SerializeField] private Sprite _passiveEventSprite;
    [SerializeField] private Sprite _rouletteEventSprite;

    private readonly Dictionary<Type, Sprite> _eventTypeSprites = new();

    private void Awake()
    {
        _objectPool = new(_popUpPrefab, GameManager.Instance.GameInfo.PopUpPoolSize, true, PopUpCanvas.transform);
        
        _eventTypeSprites.Add(typeof(ChoiceEvent), _choiceEventSprite);
        _eventTypeSprites.Add(typeof(ChooseNameEvent), _chooseNameEventSprite);
        _eventTypeSprites.Add(typeof(ChooseSloganEvent), _chooseSloganEventSprite);
        _eventTypeSprites.Add(typeof(DrawEvent), _drawEventSprite);
        _eventTypeSprites.Add(typeof(PassiveEvent), _passiveEventSprite);
        _eventTypeSprites.Add(typeof(RouletteEvent), _rouletteEventSprite);
    }

    public void ShowPopUp(Building building, Type eventType = null)
    {
        Sprite sprite = eventType is null ? _rewardSprite : _eventTypeSprites[eventType];
        var popUp = _objectPool.Get();
        popUp.SetHouse(building, sprite);
        _housePopUps.Add(building, popUp);    
    }

    public void HidePopUp(Building building)
    {
        var popUp = _housePopUps[building];
        _housePopUps.Remove(building);
        popUp.RemoveHouse();
        _objectPool.Return(popUp);
    }
}
