using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/ChooseNameEvent")]
public class ChooseNameEvent : ScriptableObject, IGameEvent
{
    [field:SerializeField] public IBuilding.BuildingType BuildingType { get; private set; }
    [field: SerializeField] public Dialogue StartDialogue { get; private set; }
    [field: SerializeField] public string RequestPhrase { get; private set; }
    
}
