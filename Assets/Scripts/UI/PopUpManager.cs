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
    private Dictionary<House, PopUp> _housePopUps = new();

    private void Awake()
    {
        _objectPool = new(_popUpPrefab, GameManager.Instance.GameInfo.PopUpPoolSize, true, PopUpCanvas.transform);
    }

    public void ShowPopUp(House house)
    {
        var popUp = _objectPool.Get();
        popUp.SetHouse(house);
        _housePopUps.Add(house, popUp);    
    }

    public void HidePopUp(House house)
    {
        var popUp = _housePopUps[house];
        _housePopUps.Remove(house);
        popUp.RemoveHouse();
        _objectPool.Return(popUp);
    }
}
