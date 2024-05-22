using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Events/PassiveEvent")]

public class PassiveEvent : ScriptableObject, IGameEvent
{
    [field:SerializeField] public IBuilding.BuildingType BuildingType { get; private set; }
    [field: SerializeField] public Dialogue StartDialogue { get; private set; }
    [field: SerializeField] public Outcomes Outcomes { get; private set; }
}
