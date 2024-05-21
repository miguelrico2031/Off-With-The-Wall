
using UnityEngine;

public interface IPopUpService : IService
{
    public Canvas PopUpCanvas { get; }
    public void ShowPopUp(Building building);
    public void HidePopUp(Building building);
}
