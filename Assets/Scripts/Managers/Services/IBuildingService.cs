
//Servicio encargado de tener las referencias a todos los edificios, y asignarles las recompensas y eventos
public interface IBuildingService : IService
{
    public bool EventLimitReached { get; }
    public bool HouseRewardLimitReached { get; }
    public void AddBuilding(Building building); //llamado por los edificios para registrarse en el servicio
    //Asignar una recompensa a un edifico (pop up tal)
    public bool SetHouseReward(uint reward);
    //asignar un evento a un edificio
    public bool SetEvent(IGameEvent buildingEvent);

    public void RegisterBuildingClick(Building building);


}
