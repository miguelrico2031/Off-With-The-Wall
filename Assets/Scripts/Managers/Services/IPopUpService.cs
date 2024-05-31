
using UnityEngine;
using System;

public interface IPopUpService : IService
{
    public Canvas PopUpCanvas { get; }
    public void ShowPopUp(Building building, Type eventType = null);
    public void HidePopUp(Building building);

    public void HideAllPopUps();
    
}
