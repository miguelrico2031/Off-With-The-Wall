using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour, IPoolObject
{
    public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }

    private Button _button;
    private Building _activeBuilding;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void ClickPopUp()
    {
        if (GameManager.Instance.CurrentGameState is not GameManager.GameState.OnPlay) return;
        if (_activeBuilding.CurrentState == IBuilding.State.HasReward)
        {
            _activeBuilding.CollectReward();
        }
        else if(_activeBuilding.CurrentState == IBuilding.State.HasEvent)
        {
            _activeBuilding.StartEvent();
        }

    }

    public void SetHouse(Building building)
    {
        _activeBuilding = building;
        _rectTransform.position = building.transform.position + building.PopUpOffset;

    }

    public void RemoveHouse()
    {
        _activeBuilding = null;
    }


    public void Clean() {}

    public IPoolObject Clone(Transform parent = null, bool active = false)
    {
        var instance = parent is null ? Instantiate(this) : Instantiate(this, parent);
        instance.gameObject.SetActive(active);
        return instance;
    }
}
