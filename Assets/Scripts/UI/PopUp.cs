using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour, IPoolObject
{
    public bool Active { get => gameObject.activeSelf; set => gameObject.SetActive(value); }

    private Button _button;
    private House _activeHouse;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void ClickPopUp()
    {
        _activeHouse.CollectReward();
    }

    public void SetHouse(House house)
    {
        _activeHouse = house;
        _rectTransform.position = house.transform.position + house.PopUpOffset;

    }

    public void RemoveHouse()
    {
        _activeHouse = null;
    }


    public void Clean()
    {
        
    }

    public IPoolObject Clone(Transform parent = null, bool active = false)
    {
        var instance = parent is null ? Instantiate(this) : Instantiate(this, parent);
        instance.gameObject.SetActive(active);
        return instance;
    }
}
