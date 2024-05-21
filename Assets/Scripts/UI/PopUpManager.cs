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

    private void Awake()
    {
        _objectPool = new(_popUpPrefab, GameManager.Instance.GameInfo.PopUpPoolSize, true, PopUpCanvas.transform);
    }

    public void ShowPopUp(Building building)
    {
        var popUp = _objectPool.Get();
        popUp.SetHouse(building);
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
