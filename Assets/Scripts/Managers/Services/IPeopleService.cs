using UnityEngine.Events;

//Servicio encargado de llevar la cuenta de las personas, y los multiplicadores para cuando aumenta el numero de personas
//asi como los eventos que se lanzan cuando cambia el num de personas y cuando llega a 0.
public interface IPeopleService : IService
{
    public uint People { get; }
    public uint AddPeople(uint people);
    public void RemovePeople(uint people);
    public void AddMultiplier(string key, float multiplier);
    public void AddMultiplier(string key, float multiplier, float duration);
    public bool RemoveMultiplier(string key);
    public UnityEvent<uint> OnPeopleChanged { get; }
    public UnityEvent OnZeroPeople { get; }
}
