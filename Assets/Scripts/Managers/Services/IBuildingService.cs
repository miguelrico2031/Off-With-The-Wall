
//Servicio encargado de tener las referencias a todos los edificios, y asignarles las recompensas y eventos
public interface IBuildingService : IService
{
    public void AddBuilding(IBuilding building); //llamado por los edificios para registrarse en el servicio
    //Asignar una recompensa a un edifico (pop up tal)
    public bool SetReward(uint reward, IBuilding.BuildingType target = IBuilding.BuildingType.Any);
    //asignar un evento a un edificio
    public bool SetEvent(IGameEvent buildingEvent);
}
