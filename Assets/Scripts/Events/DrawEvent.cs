using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/DrawEvent")]
public class DrawEvent : ScriptableObject, IGameEvent
{
    [field:SerializeField] public IBuilding.BuildingType BuildingType { get; private set; }
    [field: SerializeField] public string StartDialogueKey { get; private set; }
}
