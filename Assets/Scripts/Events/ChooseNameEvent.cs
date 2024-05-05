using UnityEngine;

public class ChooseNameEvent : ScriptableObject, IGameEvent
{
    [field:SerializeField] public IBuilding.BuildingType BuildingType { get; private set; }
    [field: SerializeField] public string StartDialogueKey { get; private set; }
    [field: SerializeField] public string RequestPhrase { get; private set; }
    
}
