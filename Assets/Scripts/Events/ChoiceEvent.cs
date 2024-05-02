using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/ChoiceEvent")]

public class ChoiceEvent : ScriptableObject, IGameEvent
{
    [field:SerializeField] public IBuilding.BuildingType BuildingType { get; private set; }
    [field: SerializeField] public string StartDialogueKey { get; private set; }
    
    [field: SerializeField] public string ChoiceTextA { get; private set; }
    [field: SerializeField] public string ChoiceTextB {get; private set; }

    [field: SerializeField] public string EndDialogueAKey {get;private set;}
    [field: SerializeField] public string EndDialogueBKey {get;private set;}

    [field: SerializeField] public Outcomes OutcomesA {get;private set;}
    [field: SerializeField] public Outcomes OutcomesB {get;private set;}

}
