
using UnityEngine;

public interface IPopUpService : IService
{
    public Canvas PopUpCanvas { get; }
    public void ShowPopUp(House house);
    public void HidePopUp(House house);
}
