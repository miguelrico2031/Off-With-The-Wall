using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/DrawEvent")]
public class DrawEvent : ScriptableObject, IGameEvent
{
    [field:SerializeField] public IBuilding.BuildingType BuildingType { get; private set; }
    [field: SerializeField] public Dialogue StartDialogue { get; private set; }
}
