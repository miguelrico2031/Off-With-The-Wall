using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(menuName = "ScriptableObjects/Events/ChoiceEvent")]

public class ChoiceEvent : ScriptableObject, IGameEvent
{
    [field: SerializeField] public IBuilding.BuildingType BuildingType { get; private set; }
    [field: SerializeField] public Dialogue StartDialogue { get; private set; }

    [field: SerializeField] public string ChoiceTextA { get; private set; }
    [field: SerializeField] public string ChoiceTextB { get; private set; }

    [field: SerializeField] public Dialogue EndDialogueA { get; private set; }
    [field: SerializeField] public Dialogue EndDialogueB { get; private set; }

    [field: SerializeField] public Outcomes OutcomesA { get; private set; }
    [field: SerializeField] public Outcomes OutcomesB { get; private set; }


}
