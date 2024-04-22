
using UnityEngine;

// [CreateAssetMenu(menuName = "ScriptableObjects/GameInfo")]
public class GameInfo : ScriptableObject
{ //Objeto unico con informacion serializable sobre configuracion del juego, referenciada por el GameManager
    public uint InitialPeople { get => _initialPeople; }
    
    [SerializeField] private uint _initialPeople;
    
}
